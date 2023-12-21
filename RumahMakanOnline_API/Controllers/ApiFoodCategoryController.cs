using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_API.Models;
using RumahMakanOnline_ViewModels;

namespace RumahMakanOnline_API.Controllers
{
        [Route("[controller]")]
        [ApiController]
    public class ApiFoodCategoryController : Controller
    {
        private readonly RestaurantKakiTujuhContext db;
        private VMRespons respon = new VMRespons();
        public ApiFoodCategoryController(RestaurantKakiTujuhContext _db)
        {
            this.db = _db;
        }
        [HttpGet("GetAllFoodCategory")]
        public List<FoodCategory> GetAllFoodCategory()
        {
            List<FoodCategory> dataFoodCategory = new List<FoodCategory>();
            dataFoodCategory = db.FoodCategories.Where(a => a.IsDelete == false).ToList();

            return dataFoodCategory;
        }
        [HttpGet("GetById/{id}")]
        public FoodCategory GetById(int id)
        {
            FoodCategory dataFoodCategory = new FoodCategory();
            dataFoodCategory = db.FoodCategories.Where(a => a.Id == id).FirstOrDefault();
            return dataFoodCategory;
        }

        [HttpPost("PostFoodCategory")]
        public VMRespons PostFoodCategory(FoodCategory data)
        {
            data.IsDelete = false;
            data.CreateDate = DateTime.Now;

            try
            {
                db.Add(data);
                db.SaveChanges();
                respon.Message = "Data Success Save";
            }
            catch (Exception e)
            {
                respon.Success = false;
                respon.Message = "failed saved : " + e.InnerException;
            }
            return respon;
        }

        [HttpPut("PutFoodCategory")]
        public VMRespons PutFoodCategory(FoodCategory data)
        {
            //cari data lama di database yang akan di update
            FoodCategory dataOld = db.FoodCategories.Where(a => a.Id == data.Id).FirstOrDefault();
            //jika data ada 
            if (dataOld != null)
            {
                //data lama akan update dengan data baru
                dataOld.CatName = data.CatName;
                dataOld.Description = data.Description;
                dataOld.UpdateDate = DateTime.Now;
                dataOld.UpdateBy = data.UpdateBy;

                try
                {
                    db.Update(dataOld);
                    db.SaveChanges();
                    respon.Message = "Data success update";
                }
                catch (Exception e)
                {
                    respon.Success = false;
                    respon.Message = "Update Failed" + e.Message;
                }
            }
            else
            {
                respon.Success = false;
                respon.Message = "Data not found";
            }
            return respon;
        }

        [HttpDelete("DeleteFoodCategory/{id}")]
        public VMRespons DeleteFoodCategory(int id)
        {
            FoodCategory data = db.FoodCategories.Where(a => a.Id == id).FirstOrDefault();

            if (data != null)
            {
                data.IsDelete = true;
                try
                {
                    db.Update(data);
                    db.SaveChanges();
                    respon.Message = "Data succes deleted";
                }
                catch (Exception e)
                {
                    respon.Success = false;
                    respon.Message = "Delete failed : " + e.Message;
                }
            }
            else
            {
                respon.Success = false;
                respon.Message = "Data not found";

            }
            return respon;
        }
    }
}
