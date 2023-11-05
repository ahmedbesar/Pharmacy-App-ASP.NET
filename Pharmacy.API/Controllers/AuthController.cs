using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domian.Entities.Identity;
using Pharmacy.Domian.IdentityDtos;
using Pharmacy.Domian.Interfaces;
using Pharmacy.Infrastructure.Dtos;
using System.Data.Entity;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Pharmacy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IMailingService _mailingService;
        public AuthController(IAuthService authService, IMapper mapper, IMailingService mailingService)
        {
            _authService = authService;
            _mapper = mapper;
            _mailingService = mailingService;
        }
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;



        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] string email)
        {
            var user = await _authService.CheckUserByEmail(email);

            if (user == null)
            {
                return BadRequest(" Write a valid Email ");
            }
            var code =  new Random().Next(1000, 9999).ToString();

            if (string.IsNullOrEmpty(code) || !(code.Length == 4))
                return BadRequest(" Code is not valid ");

            await _authService.UpdateUserVerificationCode(user, code);
            

            await SendMail(new MailRequestDto { ToEmail = user.Email, Subject = "Verification code", Body = code });

            Response.Cookies.Append("ForgotPasswordEmail", user.Email, new CookieOptions
            {
                // Set additional options as needed, e.g., expiration, secure, etc.
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                // Add more options as needed
            }) ;
            return Ok(user.Email);
        }
        [HttpPost("resendVereficationCode")]
        public async Task<IActionResult> resendCode()
        {
            string email = Request.Cookies["ForgotPasswordEmail"];
            var user = await _authService.CheckUserByEmail(email);
            if (user == null) return BadRequest($"No user was found by email: {email}");
            var code = new Random().Next(1000, 9999).ToString();

            if (string.IsNullOrEmpty(code) || !(code.Length == 4))
                return BadRequest(" Code is not valid ");
            await _authService.UpdateUserVerificationCode(user, code);

            await SendMail(new MailRequestDto { ToEmail = user.Email, Subject = "Verification code", Body = code });
            return Ok(user.VerificationCode);
        }

        [NonAction]
        public async Task<IActionResult> SendMail(MailRequestDto dto)
        {
            await _mailingService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body, dto.Attachments);
            return Ok();
        }

        [HttpPost("Verification")]
        public async Task<IActionResult> VerificationCode( string code)
         {
             string email =  Request.Cookies["ForgotPasswordEmail"];
             var user = await _authService.CheckUserByEmail(email);
             if (user == null) return BadRequest($"No user was found by email: {email}");
       
             if (string.IsNullOrEmpty(code) )
                 return BadRequest(" You must send code ");
             if (!(code.Length == 4))
                 return BadRequest(" Code must consists of 4 numbers ");
             var checkedCode = await _authService.checkVerificationCode(user, code);
             if (!checkedCode)
                 return BadRequest("Code is wrong");
       
             return Ok("Code is Verified");
         }
              
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
         {
            if (dto.Password == null) return BadRequest("Password field is required");


            string email = Request.Cookies["ForgotPasswordEmail"];
            var user = await _authService.CheckUserByEmail(email);
            if (user == null) return BadRequest($"No user was found by email: {email}");
            var result = await _authService.resetPassword(user, dto.Password);
             if (!result)
                 return BadRequest(" Please write valid password ");

            var TokenRequestDto = new TokenRequestDto
            {
                Email=user.Email,
                Password=dto.Password
            };



            var loginResult = await _authService.GetTokenAsync(TokenRequestDto);
            if (!loginResult.IsAuthenticated)
                return BadRequest(loginResult.Message);
            if (!string.IsNullOrEmpty(loginResult.RefreshToken))
                SetRefreshTokenInCookie(loginResult.RefreshToken, loginResult.RefreshTokenExpiration);
            loginResult.Message = "You reset your password  and logged in successfully. ";
            return Ok(loginResult);

         }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>>GetCurrentUserInfo()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _authService.GetCurrentUserById(email);
            if (user == null)
              return BadRequest("NO user was found");

            var currentUser= _mapper.Map<UserDto>(user);

            return Ok(currentUser);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        //login
        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestDto model)
        {
            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if(!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        
        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authService.RevokeTokenAsync(token);

            if(!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }
        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto dto)
        {
            if (dto.Number == null)
                return BadRequest("Number field is required");

            if (dto.Number.Length != 11)
                return BadRequest("Number field must contain 11 number");

            if (dto.Password == null)
                return BadRequest("Password field is required");

            if (dto.Email == null)
                return BadRequest("Email field is required");

            if (dto.Username == null)
                return BadRequest("Name field is required");

            if (dto.ProfileImage != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.ProfileImage.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (dto.ProfileImage.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");     
            }

            var token = dto.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");
            var result = await _authService.UpdateUser(token, dto);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                 HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
