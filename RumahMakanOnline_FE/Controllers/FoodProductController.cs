using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_ViewModels;
using RumahMakanOnline_API.Models;
using RumahMakanOnline_FE.Services;

namespace RumahMakanOnline_FE.Controllers
{
    public class FoodProductController : Controller
    {
        private FoodVariantService variant_service;
        private FoodCategoryService category_service;
        private FoodProductService product_service;

        private readonly IWebHostEnvironment webHost;
        private readonly IHttpContextAccessor contextAccessor;

        private int idUser = 1;
        public FoodProductController(FoodProductService _productService, FoodVariantService _variantService,
            FoodCategoryService _categoryService, IWebHostEnvironment webHost, IHttpContextAccessor _contextAccessor)
        {
            this.variant_service = _variantService;
            this.category_service = _categoryService;
            this.product_service = _productService;
            this.webHost = webHost;
            this.contextAccessor = _contextAccessor;
            this.idUser = contextAccessor.HttpContext.Session.GetInt32("IdUser") ?? 1;
        }

        #region CRUD
        public async Task<IActionResult> Index(VMSearchPage pg)
        {
            List<VMFoodProduct> dataProduct = await product_service.AllProduct();

            ViewBag.IdSort = pg.sortOrder == "Id" ? "Id_desc" : "Id";
            ViewBag.NameSort = String.IsNullOrEmpty(pg.sortOrder) ? "name_desc" : "";
            ViewBag.PriceSort = pg.sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.StockSort = pg.sortOrder == "Stock" ? "Stock_desc" : "Stock";
            ViewBag.CurrentSort = pg.sortOrder;
            ViewBag.pageSize = pg.pageSize;
            ViewBag.filterMinPrice = pg.MinPrice;
            ViewBag.filterMaxPrice = pg.MaxPrice;

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
                dataProduct = dataProduct.Where(a => a.NameProduct.ToLower().Contains(pg.searchString.ToLower())).ToList();
            }

            if (pg.MinPrice != null || pg.MaxPrice != null)
            {
                pg.MinPrice = pg.MinPrice == null ? decimal.MinValue : pg.MinPrice;
                pg.MaxPrice = pg.MaxPrice == null ? decimal.MaxValue : pg.MaxPrice;

                dataProduct = dataProduct.Where(a => a.Price >= pg.MinPrice && a.Price <= pg.MaxPrice).ToList();
            }

            switch (pg.sortOrder)
            {
                case "name_desc":
                    dataProduct = dataProduct.OrderByDescending(c => c.NameProduct).ToList();
                    break;
                case "Price":
                    dataProduct = dataProduct.OrderBy(c => c.Price).ToList();
                    break;
                case "Price_desc":
                    dataProduct = dataProduct.OrderByDescending(c => c.Price).ToList();
                    break;
                case "Stock":
                    dataProduct = dataProduct.OrderBy(c => c.Stock).ToList();
                    break;
                case "Stock_desc":
                    dataProduct = dataProduct.OrderByDescending(c => c.Stock).ToList();
                    break;
                case "Id":
                    dataProduct = dataProduct.OrderBy(c => c.Id).ToList();
                    break;
                case "Id_desc":
                    dataProduct = dataProduct.OrderByDescending(c => c.Id).ToList();
                    break;
                default:
                    dataProduct = dataProduct.OrderBy(c => c.NameProduct).ToList();
                    break;
            }

            return View(await PaginatedList<VMFoodProduct>.CreateAsync(dataProduct, pg.pageNumber ?? 1, pg.pageSize ?? 3));
        }

        public async Task<IActionResult> Search()
        {
            return PartialView();
        }

        public async Task<IActionResult> Create()
        {
            VMFoodProduct data = new VMFoodProduct();
            List<FoodCategory> listCategory = await category_service.AllFoodCategory();
            ViewBag.listCategory = listCategory;
            List<VMFoodVariant> listVariant = await variant_service.AllFoodVariant();
            ViewBag.listVariant = listVariant;
            return PartialView(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VMFoodProduct data)
        {
            data.CreateBy = idUser;
            data.Image = Upload(data);
            VMRespons respons = await product_service.PostProduct(data);

            List<FoodCategory> listCategory = await category_service.AllFoodCategory();
            ViewBag.listCategory = listCategory;
            List<VMFoodVariant> listVariant = await variant_service.AllFoodVariant();
            ViewBag.listVariant = listVariant;
            return Json(new { dataRespon = respons });
        }
        public async Task<IActionResult> Edit(int id)
        {
            List<FoodCategory> listCategory = await category_service.AllFoodCategory();
            ViewBag.listCategory = listCategory;
            VMFoodProduct dataProduct = await product_service.GetById(id);
            List<VMFoodVariant> listVariant = await variant_service.AllFoodVariant();
            ViewBag.listVariant = listVariant;
            return PartialView(dataProduct);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(VMFoodProduct data)
        {
            data.UpdateBy = idUser;
            data.Image = Upload(data);
            VMRespons respons = await product_service.PutProduct(data);

            List<FoodCategory> listCategory = await category_service.AllFoodCategory();
            ViewBag.listCategory = listCategory;
            List<VMFoodVariant> listVariant = await variant_service.AllFoodVariant();
            ViewBag.listVariant = listVariant;
            return Json(new { dataRespon = respons });
        }

        public async Task<IActionResult> Detail(int id)
        {
            VMFoodProduct dataVariant = await product_service.GetById(id);

            return PartialView(dataVariant);
        }
        public async Task<IActionResult> Delete(int id)
        {
            VMFoodProduct data = await product_service.GetById(id);
            return PartialView(data);
        }

        public async Task<IActionResult> SureDelete(int id)
        {
            VMRespons respons = await product_service.DeleteProduct(id);
            if (respons.Success)
            {
                //return RedirectToAction("Index");
                return Json(new { dataRespon = respons });

            }
            //return RedirectToAction("Delete", id);
            return Json(new { dataRespon = respons });
        }

        #endregion
        #region addons function
        public string Upload(VMFoodProduct data)
        {
            string fileName = "";
            if (data.ImageFile != null)
            {
                string uploadFolder = Path.Combine(webHost.WebRootPath, "images");
                fileName = data.ImageFile.FileName;
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    data.ImageFile.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        public async Task<JsonResult> GetVariantByCategory(int idcategory)
        {
            List<VMFoodVariant> listVariant = await variant_service.GetFoodVariantByCategory(idcategory);
            return Json(listVariant);
        }
        #endregion
    }
}
