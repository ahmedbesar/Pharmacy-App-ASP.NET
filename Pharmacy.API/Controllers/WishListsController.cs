using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domian;
using Pharmacy.Domian.Entities;
using Pharmacy.Infrastructure.Consts;
using Pharmacy.Infrastructure.Dtos;
using System;

namespace Pharmacy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListsController : ControllerBase
    {

      
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public WishListsController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }



        [HttpGet("GetUserWishList")]
        public IActionResult GetUserWishList( int pgnum, int? pgsize = 30)
        {
             string userId = _userManager.GetUserId(HttpContext.User);

            if (pgnum == 0)
                return BadRequest("Page number is required");

            var wishList = _unitOfWork.WishLists.GetUserWishList(userId, pgnum,(int)pgsize).ToList(); ;
            return Ok(_mapper.Map<List<WishListDto>>(wishList));

        }


        [HttpPost("AddToWishList")]
        public  IActionResult CreateWishListAsync(int productId )
      {
         string userId = _userManager.GetUserId(HttpContext.User);
          if (productId == 0)
              return BadRequest("productId is required!");
          if (userId == null)
              return BadRequest("userId is required!");

          var WishList = new WishList
          {
              UserId = userId, 
              ProductId= productId

          };

          _unitOfWork.WishLists.Create_(WishList);
          _unitOfWork.Complete();

          return Ok(WishList);
      }

        [HttpDelete]
        public IActionResult Remove(int id)
        {
            if (id == 0)
                return BadRequest("Id field is required");
            var wishList=_unitOfWork.WishLists.GetWishListById(id);
            _unitOfWork.WishLists.Remove_(wishList);
            _unitOfWork.Complete();
            return Ok(wishList);
        }






    }
}
