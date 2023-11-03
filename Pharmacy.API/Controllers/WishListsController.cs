using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domian;
using Pharmacy.Domian.Entities;
using Pharmacy.Domian.Entities.Identity;
using Pharmacy.Domian.IdentityDtos;
using Pharmacy.Domian.Interfaces;
using Pharmacy.Infrastructure.Consts;
using Pharmacy.Infrastructure.Dtos;
using System;
using System.Security.Claims;

namespace Pharmacy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListsController : ControllerBase
    {

      
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public WishListsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _authService = authService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetUserWishList")]
        public async Task<ActionResult<WishListDto>> GetUserWishList( int pgnum, int? pgsize = 30)
        {

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _authService.GetCurrentUserById(email);
            if (user == null)
                return BadRequest("NO user was found");

            if (pgnum == 0)
                return BadRequest("Page number is required");

            var wishList = _unitOfWork.WishLists.GetUserWishList(user.Id, pgnum,(int)pgsize).ToList();
            return Ok(_mapper.Map<List<WishListDto>>(wishList));

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("AddToWishList")]
        public async Task<ActionResult<WishList>> CreateWishListAsync(int productId )
      {


            if (productId == 0)
                return BadRequest("productId is required!");

           var email =  User.FindFirst(ClaimTypes.Email)?.Value;
           var user = await _authService.GetCurrentUserById(email);
            
            var result = await _unitOfWork.WishLists.checkProductInUserWishlist(user.Id, productId);
            if (result!=null)
                return BadRequest("Product already in user wishlist");
         
          if (user == null)
              return BadRequest("No user found!");

          var WishList = new WishList
          {
               UserId = user.Id, 
              ProductId= productId

          };

             _unitOfWork.WishLists.Create_(WishList);
             _unitOfWork.Complete();

          return Ok("your product added successfully. ");
      }
       

        [HttpDelete]
        public IActionResult Remove(int id)
        {
            if (id == 0)
                return BadRequest("Id field is required");
            var wishList=_unitOfWork.WishLists.GetWishListById(id);
            if (wishList == null)
                return BadRequest($"No wishlist was found my this id: {id}");

            _unitOfWork.WishLists.Remove_(wishList);
            _unitOfWork.Complete();
            return Ok();
        }

    }
}
