using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantManager.Controllers
{
    public class BaseController : Controller
    {
        protected readonly RestaurantContext context;

        public BaseController(RestaurantContext context)
        {
            this.context = context;
        }
    }
}
