using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaStock.Web.Filters;
using SistemaStock.Domain.Entities;
using SistemaStock.Application.ViewModels;
using SistemaStock.Infrastructure.Persistence;
using SistemaStock.Web.Models;

namespace SistemaStock.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [RequiereSesion]
        public IActionResult Index()
        {
            return View();
        }

        [RequiereSesion]
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
