using Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.Models;
using Microsoft.EntityFrameworkCore;
using PersistenceManager;

namespace RestaurantManager.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(RestaurantContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            ProductsCategoriesViewModel viewmodel = new ProductsCategoriesViewModel();
            var products = from p in context.Products select p;
            List<Category> categories = context.Categories.ToList();
            List<Product> snOrderedProducts = products.OrderByDescending(p => p.SaleNumber).ToList();

            List<Product> favouriteProducts = snOrderedProducts.GetRange(0, 10);

            viewmodel.ProductList = favouriteProducts;
            viewmodel.CategoryList = categories;

            return View("Index", viewmodel);
        }

        public IActionResult Categories(Int32 id)
        {
            FilterCategoryViewModel viewmodel = new FilterCategoryViewModel();
            Category cat = context.Categories.FirstOrDefault(c => c.Id == id);
            CategoryType type = cat.Type;

            IList<Product> categoryFilteredProducts = context.Products
                .Where(item => item.Category == type).ToList();

            viewmodel.Type = type;
            viewmodel.Products = categoryFilteredProducts;

            return View("Categories", viewmodel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
