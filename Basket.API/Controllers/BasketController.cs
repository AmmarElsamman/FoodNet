using System.Security.Claims;
using Basket.API.Entities;
using Basket.API.Interfaces;
using Basket.API.Repositories;
using FoodNet.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetBasket()
        {
            var userName = User.FindFirstValue(ClaimTypes.Email);
            if (userName == null) return Unauthorized();
            var basket = await basketRepository.GetBasketAsync(userName);
            return Ok(basket ?? await basketRepository.UpdateBasketAsync(new ShoppingCart(userName)));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart shoppingCart)
        {
            var userName = User.FindFirstValue(ClaimTypes.Email);
            if (userName == null) return Unauthorized();
            shoppingCart.UserName = userName;
            if (shoppingCart == null) return BadRequest();
            var result = await basketRepository.UpdateBasketAsync(shoppingCart);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBasket()
        {
            var userName = User.FindFirstValue(ClaimTypes.Email);
            if (userName == null) return NotFound();
            await basketRepository.DeleteBasketAsync(userName);
            return NoContent();
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult> Checkout(BasketCheckout basketCheckout)
        {
            var userName = User.FindFirstValue(ClaimTypes.Email);
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userName == null) return Unauthorized();
            var basket = await basketRepository.GetBasketAsync(userName);
            if (basket == null) return NotFound();
            if (basket.Items.Count == 0) return BadRequest("Cart is empty!!");

            var eventMessage = new BasketCheckoutEvent
            {
                UserId = userId,
                TotalPrice = basket.TotalPrice,

                FirstName = basketCheckout.FirstName,
                LastName = basketCheckout.LastName,
                EmailAddress = basketCheckout.EmailAddress,
                AddressLine = basketCheckout.AddressLine,

                CardName = basketCheckout.CardName,
                CardNumber = basketCheckout.CardNumber,
                Expiration = basketCheckout.Expiration,
                CVV = basketCheckout.CVV,

                Items = basket.Items.Select(item => new BasketItemDetails
                {
                    Quantity = item.Quantity,
                    ItemName = item.ItemName,
                    ItemId = item.ItemId,
                    Price = item.Price
                }).ToList()
            };

            await publishEndpoint.Publish(eventMessage);
            await basketRepository.DeleteBasketAsync(userName);

            return Accepted();

        }

    }
}
