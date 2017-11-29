using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp2._0.Models;
using WebApp2._0.Services;

namespace WebApp2._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataService _dataService;

        public HomeController(IDataService dataServices, IServiceProvider serviceProvider)
        {
            _dataService = dataServices;
            //_dataService = (IDataService)serviceProvider.GetService(typeof(IDataService));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            var name = _dataService.GetName();
            //var name = ((IDataService)this.HttpContext.RequestServices.GetService(typeof(IDataService))).GetName();
            ViewData["Message"] = $"Your contact page. Name:  {name}";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
