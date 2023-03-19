using Movie_Web_Api.Repository;
using Movie_Web_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Movie_Web_Api.Controllers
{
    [Route("api/CartItems")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IMovieRepository moviesRepo;
        private readonly CartItemReposatory cartItemRepo;
        private readonly IOrderRepository ordersRepo;

        public OrdersController(IMovieRepository _moviesRepo, CartItemReposatory _cartItemRepo, IOrderRepository _ordersRepo)
        {
            moviesRepo = _moviesRepo;
            cartItemRepo = _cartItemRepo;
            ordersRepo = _ordersRepo;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await ordersRepo.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return Ok(orders);
        }

        public IActionResult ShoppingCart()
        {
            var items = cartItemRepo.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(response);
        }

        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var item = await _moviesRepo.GetMovieById(id);

            if (item != null)
            {
                _shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _moviesRepo.GetMovieById(id);

            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();

            return View("OrderCompleted");
        }
    }
}
