using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_API.Models;
using RumahMakanOnline_FE.Services;
using RumahMakanOnline_ViewModels;

namespace RumahMakanOnline_FE.Controllers
{
    public class FoodCategoryController : Controller
    {
        private FoodCategoryService foodcategory_service;
        private readonly IHttpContextAccessor contextAccessor;
        private int idUser = 1;
        public FoodCategoryController(FoodCategoryService _foodcategoryservice, IHttpContextAccessor _contextAccessor)
        {
            this.foodcategory_service = _foodcategoryservice;
            this.contextAccessor = _contextAccessor;
            this.idUser = contextAccessor.HttpContext.Session.GetInt32("IdUser") ?? 1;
        }
        public async Task<IActionResult> Index(VMSearchPage pg)
        {
            List<FoodCategory> dataCategory = await foodcategory_service.AllFoodCategory();

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
                dataCategory = dataCategory.Where(a => a.CatName.ToLower().Contains(pg.searchString.ToLower())).ToList();
            }

            switch (pg.sortOrder)
            {
                case "name_desc":
                    dataCategory = dataCategory.OrderByDescending(c => c.CatName).ToList();
                    break;
                case "Id":
                    dataCategory = dataCategory.OrderBy(c => c.Id).ToList();
                    break;
                case "Id_desc":
                    dataCategory = dataCategory.OrderByDescending(c => c.Id).ToList();
                    break;
                default:
                    dataCategory = dataCategory.OrderBy(c => c.CatName).ToList();
                    break;
            }
            return View(await PaginatedList<FoodCategory>.CreateAsync(dataCategory, pg.pageNumber ?? 1, pg.pageSize ?? 3));
        }
        public IActionResult Create()
        {
            FoodCategory data = new FoodCategory();
            return PartialView(data);
        }
        [HttpPost]
        public async Task<IActionResult> Create(FoodCategory data)
        {
            data.CreateBy = idUser;
            VMRespons respons = await foodcategory_service.PostFoodCategory(data);
            if (respons.Success)
            {
                return RedirectToAction("Index");
            }
            return View(data);

        }
        public async Task<IActionResult> Edit(int id)
        {
            //ViewBag.id = id;
            FoodCategory dataCategory = await foodcategory_service.GetById(id);
            return PartialView(dataCategory);
            //return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FoodCategory data)
        {
            data.UpdateBy = idUser;
            //panggil service put
            VMRespons respons = await foodcategory_service.PutFoodCategory(data);

            if (respons.Success)
            {
                return Json(new { dataRespon = respons });
            }

            return Json(new { dataRespon = respons });
        }

        public async Task<IActionResult> Detail(int id)
        {
            FoodCategory dataCategory = await foodcategory_service.GetById(id);
            return PartialView(dataCategory);
        }
        public async Task<IActionResult> Delete(int id)
        {
            FoodCategory data = await foodcategory_service.GetById(id);
            return PartialView(data);
        }
        public async Task<IActionResult> SureDelete(int Id)
        {
            VMRespons respons = await foodcategory_service.DeleteFoodCategory(Id);

            if (respons.Success)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", Id);
        }
    }
}
