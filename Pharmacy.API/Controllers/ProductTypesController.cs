using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domian;
using Pharmacy.Domian.Entities;
using Pharmacy.Infrastructure.Dtos;
using System.Collections;
using System.ComponentModel;

namespace Pharmacy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypesController : ControllerBase
    {
       

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductTypesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        


        [HttpGet("AllproductTypeswithSearchAndPagination")]
        public ActionResult<List<ProductTypeDto>> AllproductTypeswithSearch(string? term, int pgnum, int? pgsize=30)
        {
           if(pgnum==0)
                return BadRequest("Page number is required");


            var productTypes = _unitOfWork.ProductTypes.GetByProductTypeNameAndSearchWithPagination(term, pgnum,(int)pgsize);
            if (productTypes == null)
                return NotFound($"No results  was found  with this word: {term}");
            return Ok(_mapper.Map<List<ProductTypeDto>>(productTypes));
        }




        [HttpGet("GetProductTypeById")]
        public async Task<IActionResult> GetProductTypeById(int id)
        {
            var ProductType = await _unitOfWork.ProductTypes.GetByIdAsync(t=>t.Id==id);
            if (ProductType == null)
                return NotFound($"No Product Type was found  with this Id: {id}");
            return Ok(_mapper.Map<ProductTypeDto>(ProductType));
        }





    }
}
