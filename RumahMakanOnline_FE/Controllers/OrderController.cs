using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_FE.Services;
using RumahMakanOnline_ViewModels;

namespace RumahMakanOnline_FE.Controllers
{
    public class OrderController : Controller
    {
        private OrderService order_service;
        private FoodProductService product_service;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IWebHostEnvironment webHost;
        private int IdUser = 1;
        private int IdCustomer = 1;

        public OrderController(FoodProductService _productService, OrderService _orderService, IWebHostEnvironment webHost, IHttpContextAccessor _contextAccessor)
        {
            this.product_service = _productService;
            this.order_service = _orderService;
            this.webHost = webHost;
            this.contextAccessor = _contextAccessor;
            this.IdUser = contextAccessor.HttpContext.Session.GetInt32("IdUser") ?? 1;
            this.IdCustomer = contextAccessor.HttpContext.Session.GetInt32("IdCustomer") ?? 1;
        }
        public async Task<IActionResult> Menu()
        {
            List<VMFoodProduct> dataProduct = await product_service.AllProduct();

            return View(dataProduct);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(VMOrderHeader data)
        {
            return PartialView(data);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitPayment(VMOrderHeader dataHeader)
        {
            VMRespons respon = new VMRespons();

            dataHeader.CustId = IdUser;
            dataHeader.CreateBy = IdUser;

            respon = await order_service.SubmitPayment(dataHeader);

            return Json(respon);
        }
        public async Task<IActionResult> HistoryTransaction(VMSearchPage pg)
        {
            List<VMOrderHeader> dataOrderHeader = await order_service.AllOrderHeader(IdCustomer);
            int count = await order_service.countTransaction(IdUser);

            var cultureinfo = new System.Globalization.CultureInfo("en-US");

            if (pg.MinOrderDate != null)
            {
                string minDate = pg.MinOrderDate?.ToString("MM/dd/yyyy");
                pg.MinOrderDate = DateTime.Parse(minDate, cultureinfo);
            }
            if (pg.MaxOrderDate != null)
            {
                string maxDate = pg.MaxOrderDate?.ToString("MM/dd/yyyy");
                pg.MaxOrderDate = DateTime.Parse(maxDate, cultureinfo);
            }

            ViewBag.CodeTransaction = String.IsNullOrEmpty(pg.sortOrder) ? "CodeTransaction_desc" : "";
            ViewBag.AmountSort = pg.sortOrder == "Amount" ? "Amount_desc" : "Amount";
            ViewBag.OrderSort = pg.sortOrder == "OrderDate" ? "OrderDate_desc" : "OrderDate";
            ViewBag.CurrentSort = pg.sortOrder;
            ViewBag.pageSize = pg.pageSize;
            ViewBag.filterMinAmount = pg.MinPrice;
            ViewBag.filterMaxAmount = pg.MaxPrice;
            ViewBag.filterMinDate = pg.MinOrderDate;
            ViewBag.filterMaxDate = pg.MaxOrderDate;
            ViewBag.countTransaction = count;

            if (pg.searchString != null)
            {
                pg.pageNumber = 1;
            }
            else
            {
                pg.searchString = pg.CurrentFilter;
            }

            ViewBag.CurrentFilter = pg.searchString;

            if (!string.IsNullOrEmpty(pg.searchString))
            {
                dataOrderHeader = dataOrderHeader.Where(a => a.CodeTransaction.Contains(pg.searchString)).ToList();
            }
            if (pg.MinPrice != null || pg.MaxPrice != null)
            {
                pg.MinPrice = pg.MinPrice == null ? decimal.MinValue : pg.MinPrice;
                pg.MaxPrice = pg.MaxPrice == null ? decimal.MaxValue : pg.MaxPrice;

                dataOrderHeader = dataOrderHeader.Where(a => a.Amount >= pg.MinPrice && a.Amount <= pg.MaxPrice).ToList();
            }

            if (pg.MinOrderDate != null || pg.MaxOrderDate != null)
            {
                pg.MinOrderDate = pg.MinOrderDate == null ? DateTime.MinValue : pg.MinOrderDate;
                pg.MaxOrderDate = pg.MaxOrderDate == null ? DateTime.MaxValue : pg.MaxOrderDate;

                dataOrderHeader = dataOrderHeader.Where(a => a.CreateDate >= pg.MinOrderDate && a.CreateDate <= pg.MaxOrderDate).ToList();
            }
            switch (pg.sortOrder)
            {
                case "CodeTransaction_desc":
                    dataOrderHeader = dataOrderHeader.OrderByDescending(c => c.CodeTransaction).ToList();
                    break;
                case "Amount":
                    dataOrderHeader = dataOrderHeader.OrderBy(c => c.Amount).ToList();
                    break;
                case "Amount_desc":
                    dataOrderHeader = dataOrderHeader.OrderByDescending(c => c.Amount).ToList();
                    break;
                case "OrderDate":
                    dataOrderHeader = dataOrderHeader.OrderBy(c => c.CreateDate).ToList();
                    break;
                case "OrderDate_desc":
                    dataOrderHeader = dataOrderHeader.OrderByDescending(c => c.CreateDate).ToList();
                    break;
                default:
                    dataOrderHeader = dataOrderHeader.OrderBy(c => c.CodeTransaction).ToList();
                    break;
            }

            return View(await PaginatedList<VMOrderHeader>.CreateAsync(dataOrderHeader, pg.pageNumber ?? 1, pg.pageSize ?? 3));
        }

        public async Task<IActionResult> Search()
        {
            return PartialView();
        }
    }
}
