using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using PersistenceManager;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestaurantManager.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly RestaurantContext context;
        // GET: /<controller>/
        public OrdersController(RestaurantContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }


        [HttpGet]
        public IActionResult GetOrders()
        {
            try
            {
                return Ok(context.Orders.ToList().Select(order => new OrderDTO
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Address,
                    PhoneNumber = order.PhoneNumber,
                    Items = context.ShoppingCartItems.ToList().Where(sh => sh.OrderId == order.Id)
                        .Select(sh => new ShoppingCartItemDTO
                        {
                            Id = sh.Id,
                            Name = context.Products.Where(p => p.Id == sh.ProductId).Select(p => p.Name).FirstOrDefault(),
                            Price = context.Products.Where(p => p.Id == sh.ProductId).Select(p => p.Price).Sum(),
                        }),
                    CompletionDate = new DateTime(),
                    TransmittingDate = new DateTime()
                }));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
