using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie_Web_Api.Models;
using Movie_Web_Api.Repository;
using System.Security.Claims;

namespace Movie_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository Orderrepo;
        public OrdersController(OrderRepository _orderrepo)
        {
            Orderrepo = _orderrepo;
        }
        [HttpGet("myorders")]
        public async Task<ActionResult<List<Order>>> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var userOrders = await Orderrepo.GetOrdersByUserIdAndRoleAsync(userId, userRole);

            if (userOrders == null)
            {
                return NotFound();
            }

            return Ok(userOrders);
        }

    }
}
