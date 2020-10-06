using System.Threading.Tasks;
using Core.Entitites;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            
            return Ok(basket ?? new CustomerBasket(basketId));
        }
        
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);

            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task<ActionResult<CustomerBasket>> DeleteBasket(string id)
        {
            if(await _basketRepository.DeleteBasketAsync(id))
                return Ok();

            return BadRequest("Could not delete basket.");
        }
    }
}