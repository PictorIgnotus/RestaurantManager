using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using PersistenceManager;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestaurantManager.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly RestaurantContext context;
        // GET: /<controller>/
        public ProductsController(RestaurantContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.context = context;
        }


        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                return Ok(context.Products.ToList().Select(product => new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category,
                    Price = product.Price,
                    Hot = product.Hot,
                    Vegetarian = product.Vegetarian,
                    Description = product.Description,
                    SaleNumber = product.SaleNumber
                }));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize(Roles="administrator")]
        public IActionResult PostBuilding([FromBody] ProductDTO productDTO)
        {
            try
            {
                var addedProduct = context.Products.Add(new Product
                {
                    Name = productDTO.Name,
                    Category = productDTO.Category,
                    Price = productDTO.Price,
                    Description = productDTO.Description,
                    Hot = productDTO.Hot,
                    Vegetarian = productDTO.Vegetarian,
                    SaleNumber = 0
                });

                context.SaveChanges();

                productDTO.Id = addedProduct.Entity.Id;

                return Created(Request.GetUri() + addedProduct.Entity.Id.ToString(), productDTO);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
