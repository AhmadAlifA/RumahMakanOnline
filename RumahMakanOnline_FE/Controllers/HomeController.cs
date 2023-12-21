using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_FE.Models;
using RumahMakanOnline_FE.Services;
using System.Diagnostics;

namespace RumahMakanOnline_FE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor contextAccessor;
        private OrderService order_service;
        private int IdUser = 1;
        private int IdCustomer = 1;
        public HomeController(ILogger<HomeController> logger, OrderService _orderService, IHttpContextAccessor _contextAccessor)
        {
            _logger = logger;
            this.order_service = _orderService;
            this.contextAccessor = _contextAccessor;
            this.IdUser = contextAccessor.HttpContext.Session.GetInt32("IdUser") ?? 1;
            this.IdCustomer = contextAccessor.HttpContext.Session.GetInt32("IdCustomer") ?? 1;
        }

        public async Task<IActionResult> Index()
        {
            int count = await order_service.countTransaction(IdUser);
            decimal sum = await order_service.AmounYear(IdUser);
            ViewBag.countTransaction = count;
            ViewBag.amounYear = sum;

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