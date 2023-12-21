using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_ViewModels;
using RumahMakanOnline_FE.Services;
using RumahMakanOnline_API.Models;

namespace RumahMakanOnline_FE.Controllers
{
    public class FoodVariantController : Controller
    {
        private FoodVariantService variant_service;
        private FoodCategoryService category_service;
        private int idUser = 1;

        private readonly IHttpContextAccessor contextAccessor;
        public FoodVariantController(FoodVariantService _variantService, FoodCategoryService _categoryService, IHttpContextAccessor _contextAccessor)
        {
            this.variant_service = _variantService;
            this.category_service = _categoryService;
            this.contextAccessor = _contextAccessor;
            this.idUser = contextAccessor.HttpContext.Session.GetInt32("IdUser") ?? 1;
        }
        public async Task<IActionResult> Create()
        {
            VMFoodVariant data = new VMFoodVariant();
            List<FoodCategory> listCategory = await category_service.AllFoodCategory();
            ViewBag.listCategory = listCategory;
            return PartialView(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VMFoodVariant data)
        {
            data.CreateBy = idUser;

            VMRespons respons = await variant_service.PostFoodVariant(data);

            List<FoodCategory> listCategory = await category_service.AllFoodCategory();
            ViewBag.listCategory = listCategory;
            return Json(new { dataRespon = respons });
        }
        public async Task<IActionResult> Index(VMSearchPage pg)
        {
            List<VMFoodVariant> dataVariant = await variant_service.AllFoodVariant();

            ViewBag.NameSort = String.IsNullOrEmpty(pg.sortOrder) ? "name_desc" : "";
            ViewBag.IdSort = pg.sortOrder == "Id" ? "Id_desc" : "Id";
            ViewBag.CurrentSort = pg.sortOrder;
            ViewBag.pageSize = pg.pageSize;

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
                dataVariant = dataVariant.Where(a => a.VarName.ToLower().Contains(pg.searchString.ToLower())).ToList();
            }

            switch (pg.sortOrder)
            {
                case "name_desc":
                    dataVariant = dataVariant.OrderByDescending(c => c.VarName).ToList();
                    break;
                case "Id":
                    dataVariant = dataVariant.OrderBy(c => c.Id).ToList();
                    break;
                case "Id_desc":
                    dataVariant = dataVariant.OrderByDescending(c => c.Id).ToList();
                    break;
                default:
                    dataVariant = dataVariant.OrderBy(c => c.VarName).ToList();
                    break;
            }
            return View(await PaginatedList<VMFoodVariant>.CreateAsync(dataVariant, pg.pageNumber ?? 1, pg.pageSize ?? 3));
        }
        public async Task<IActionResult> Edit(int id)
        {
            VMFoodVariant dataVariant = await variant_service.GetById(id);
            List<FoodCategory> listCategory = await category_service.AllFoodCategory();
            ViewBag.listCategory = listCategory;
            return PartialView(dataVariant);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(VMFoodVariant data)
        {
            data.UpdateBy = idUser;

            VMRespons respons = await variant_service.PutFoodVariant(data);

            List<FoodCategory> listCategory = await category_service.AllFoodCategory();
            ViewBag.listCategory = listCategory;
            return Json(new { dataRespon = respons });

        }

        public async Task<IActionResult> Detail(int id)
        {
            VMFoodVariant dataVariant = await variant_service.GetById(id);

            return PartialView(dataVariant);
        }
        public async Task<IActionResult> Delete(int id)
        {
            VMFoodVariant data = await variant_service.GetById(id);
            return PartialView(data);
        }

        public async Task<IActionResult> SureDelete(int id)
        {
            VMRespons respons = await variant_service.DeleteFoodVariant(id);

            return Json(new { dataRespon = respons });

        }
    }
}
