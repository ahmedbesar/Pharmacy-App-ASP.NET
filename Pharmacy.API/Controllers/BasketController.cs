using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domian.Entities;
using Pharmacy.Domian.Interfaces;

namespace Pharmacy.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new Basket(id));
        }

        [HttpPost]
        public async Task<ActionResult<Basket>> UpdateBasket(Basket basket)
        {
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
            
        }
    }
}
