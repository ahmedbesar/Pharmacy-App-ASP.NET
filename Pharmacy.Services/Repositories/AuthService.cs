using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domian.Interfaces;
using Pharmacy.Domian.Entities.Identity;
using Pharmacy.Services.Settings;
using Pharmacy.Domian.IdentityDtos;

namespace Pharmacy.Services.Repositories
{
    public class AuthService : IAuthService
    {
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
            IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<AuthDto> RegisterAsync(RegisterDto model)
        {
            if (model.ProfileImage == null)
                return new AuthDto { Message = "Profile Image is required" };

            if (!_allowedExtenstions.Contains(Path.GetExtension(model.ProfileImage.FileName).ToLower()))
                return new AuthDto { Message = "Only .png and .jpg images are allowed!" };

            if (model.ProfileImage.Length > _maxAllowedPosterSize)
                return new AuthDto { Message = "Max allowed size for poster is 1MB!" };


            if (model.Number == null)
                return new AuthDto { Message = "Number is required" };

            if (model.Number.Length!=11)
                return new AuthDto { Message = "Number field must contain 11 number" };
            

            if (model.Location == null)
                return new AuthDto { Message = "Location is required" };

            if (model.Password == null)
                return new AuthDto { Message = "Password is required" };

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthDto { Message = "Email is already registered!" };

            if (await _userManager.FindByNameAsync(model.Username) is not null)
                return new AuthDto { Message = "Username is already registered!" };

            using var dataStream = new MemoryStream();

            await model.ProfileImage.CopyToAsync(dataStream);

            

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber=model.Number,
                Location = model.Location,
                ProfileImage = dataStream.ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthDto { Message = errors };
            }
            user.ProfileImage = await UploadImages.UploadImage(model.ProfileImage , "users");


            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens?.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new AuthDto
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
            };
        }
        public async Task<AuthDto> GetTokenAsync(TokenRequestDto model)
        {
            var AuthDto = new AuthDto();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                AuthDto.Message = "Email or Password is incorrect!";
                return AuthDto;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            AuthDto.IsAuthenticated = true;
            AuthDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            AuthDto.Email = user.Email;
            AuthDto.Username = user.UserName;
            AuthDto.ExpiresOn = jwtSecurityToken.ValidTo;
            AuthDto.Roles = rolesList.ToList();

            if(user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                AuthDto.RefreshToken = activeRefreshToken.Token;
                AuthDto.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                AuthDto.RefreshToken = refreshToken.Token;
                AuthDto.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return AuthDto;
        }
      
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        public async Task<AuthDto> RefreshTokenAsync(string token)
        {
            var AuthDto = new AuthDto();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if(user == null)
            {
                AuthDto.Message = "Invalid token";
                return AuthDto;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                AuthDto.Message = "Inactive token";
                return AuthDto;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user);
            AuthDto.IsAuthenticated = true;
            AuthDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            AuthDto.Email = user.Email;
            AuthDto.Username = user.UserName;
            var roles = await _userManager.GetRolesAsync(user);
            AuthDto.Roles = roles.ToList();
            AuthDto.RefreshToken = newRefreshToken.Token; 
            AuthDto.RefreshTokenExpiration = newRefreshToken.ExpiresOn; 

            return AuthDto;
        }
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
        }
        public async Task<bool> UpdateUser( string token, UpdateUserDto dto )
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            user.UserName = dto.Username;
            user.Email = dto.Email;
            user.PhoneNumber = dto.Number;
            user.PasswordHash =  _userManager.PasswordHasher.HashPassword( user, dto.Password);
            if (dto.ProfileImage != null)
            {
                using var dataStream = new MemoryStream();
                await dto.ProfileImage.CopyToAsync(dataStream);
                user.ProfileImage = await UploadImages.UploadImage(dto.ProfileImage, "users");
            }
        
            await _userManager.UpdateAsync(user);

            return true;
        }
        public async Task<ApplicationUser> GetCurrentUserById(string email)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email== email);
            return user;
        }
        public async Task<ApplicationUser> CheckUserByEmail(string email)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == email);
            return user;
        }
        public async Task<bool> UpdateUserVerificationCode( ApplicationUser user,string code)
        {
            user.VerificationCode =code;
            await _userManager.UpdateAsync(user);
            return true;
        }
        public async Task<bool> resetPassword(ApplicationUser user, string password)
        {
            user.PasswordHash =  _userManager.PasswordHasher.HashPassword(user, password);
            await _userManager.UpdateAsync(user);
            return true;
        }
        public async Task<bool> checkVerificationCode(ApplicationUser user, string code)
        {
            if (user.VerificationCode == code)  return true; 
           
            return false;
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}