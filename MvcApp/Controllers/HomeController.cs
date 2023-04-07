using Microsoft.AspNetCore.Mvc;
using MvcApp.Models;
using System.Diagnostics;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GRPC
        public IActionResult GrpcWebAssembly() => View();
        public IActionResult GrpcWebAssemblyPrerendered() => View();
        public IActionResult GrpcTelerikGridWebAssembly() => View();
        public IActionResult GrpcTelerikGridWebAssemblyPrerendered() => View();

        // REST
        public IActionResult RestWebAssembly() => View();
        public IActionResult RestWebAssemblyPrerendered() => View();
        public IActionResult RestTelerikGridWebAssembly() => View();
        public IActionResult RestTelerikGridWebAssemblyPrerendered() => View();


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}