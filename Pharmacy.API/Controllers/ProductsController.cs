using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domian;
using Pharmacy.Domian.Entities;
using Pharmacy.Domian.Interfaces;
using Pharmacy.Infrastructure.Dtos;
using System.Reflection.Metadata.Ecma335;
using IUnitOfWork = Pharmacy.Domian.IUnitOfWork;

namespace Pharmacy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        

    [HttpGet("AllproductswithSearchAndPagination")]
        public ActionResult<List<ProductDto>> AllproductswithSearch(string? term, int pgnum, int? pgsize = 30)
        {
            if (pgnum == 0)
                return BadRequest("Page number is required");


            var products = _unitOfWork.Products.GetByProductNameOrProductTypeNameWithPagination(term, pgnum, (int)pgsize);
            if (products == null)
                return NotFound($"No results  was found  with this word: {term}");
            return Ok(_mapper.Map<List<ProductDto>>(products));
        }

        [HttpGet("GetProductsByProductTypeIdWithPagination")]
        public ActionResult<List<ProductDto>> GetProductsByTypeIdWithPagination(int id, int pgnum, int? pgsize = 30)
        {
            if (pgnum == 0)
                return BadRequest("Page number is required");

            var isValidType = _unitOfWork.ProductTypes.IsvalidProductType(id);

            if (!isValidType)
                return BadRequest("Invalid Product Type ID!");

            var products = _unitOfWork.Products.GetAllProductWithSameType(id, pgnum, (int)pgsize);
            if (products == null)
                return NotFound($"No Product  was found  with this Id: {id}");
            return Ok(_mapper.Map<List<ProductDto>>(products));
        }

        [HttpGet("SearchingByProductNameInProductsWithSameTyppeWithPagination")]
        public ActionResult<List<ProductDto>> SearchingByProductNameInProductsWithSameTyppeWithPagination( int ProductTypeId, string term, int pgnum, int? pgsize=30)
        {
            if (pgnum == 0)
                return BadRequest("Page number is required");


            if (term == null)
                return BadRequest("Please enter valid word for search");


            var isValidType = _unitOfWork.ProductTypes.IsvalidProductType(ProductTypeId);

            if (!isValidType)
                return BadRequest("Invalid Product Type ID!");
            var products = _unitOfWork.Products.GetProductsWithSameProductTypeAndSearchUsingNameAndPagination(ProductTypeId, term, pgnum,(int) pgsize);
            if (products == null)
                return NotFound($"No results  was found  with this word: {term}");
            return Ok(_mapper.Map<List<ProductDto>>(products));
        }
   
        [HttpGet("GetProductById")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {

            if (id == 0)
            {
                return BadRequest("Id field is required");
            }

            var Product = await _unitOfWork.Products.GetByIdAsync(b => b.Id==id, new[] { "ProductType" });

            if (Product == null)
                return NotFound($"No Product  was found  with this Id: {id}");
            return Ok(_mapper.Map<ProductDto>(Product));
        }
        [HttpGet("GetRecommendedProducts")]
        public IActionResult GetRecommendedProducts(int id, int pgnum, int? pgsize=30)
        {
            if (pgnum == 0)
                return BadRequest("Page number is required");


            if (id == 0)
                return BadRequest("Id field is required");
            var isValidproduct = _unitOfWork.Products.IsvalidProductType(id);

            if (!isValidproduct)
                return BadRequest("Invalid Product  ID!");

            var products = _unitOfWork.Products.GetRecommendedProductsWithPagination(id, pgnum, (int)pgsize);
            if (products == null)
                return NotFound($"No related products was found");
            return Ok(_mapper.Map<List<ProductDto>>(products));
        }


    }
}
