using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using PlutoNetCoreTemplate.Mvc.Models;

using System.Diagnostics;

namespace PlutoNetCoreTemplate.Mvc.Controllers
{
    using Infrastructure.Providers;

    public class HomeController : BaseController<HomeController>
    {
        public HomeController(ILazyLoadServiceProvider lazyLoad) : base(lazyLoad)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
