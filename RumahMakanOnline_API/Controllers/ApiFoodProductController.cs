using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_API.Models;
using RumahMakanOnline_ViewModels;

namespace RumahMakanOnline_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiFoodProductController : Controller
    {
        private readonly RestaurantKakiTujuhContext db;
        private VMRespons respon = new VMRespons();

        public ApiFoodProductController(RestaurantKakiTujuhContext _db)
        {
            this.db = _db;
        }
        [HttpGet("GetAllFoodProduct")]
        public List<VMFoodProduct> GetAllProduct()
        {
            List<VMFoodProduct> dataProduct = new List<VMFoodProduct>();
            dataProduct = (from p in db.FoodProducts
                           join v in db.FoodVariants
                           on p.VarId equals v.Id
                           join c in db.FoodCategories
                           on v.CatId equals c.Id
                           where p.IsDelete == false
                           select new VMFoodProduct
                           {
                               Id = p.Id,
                               NameProduct = p.NameProduct,
                               Price = p.Price,
                               Stock = p.Stock,

                               Image = p.Image,

                               VarId = v.Id,
                               VarName = v.VarName,

                               CatId = v.CatId,
                               CatName = c.CatName,

                               IsDelete = p.IsDelete
                           }).ToList();

            return dataProduct;
        }

        [HttpGet("GetById/{id}")]
        public VMFoodProduct GetById(int id)
        {
            VMFoodProduct dataProduct = new VMFoodProduct();
            dataProduct = (from p in db.FoodProducts
                           join v in db.FoodVariants
                           on p.VarId equals v.Id
                           join c in db.FoodCategories
                           on v.CatId equals c.Id
                           where p.IsDelete == false && p.Id == id
                           select new VMFoodProduct
                           {
                               Id = p.Id,
                               NameProduct = p.NameProduct,
                               Price = p.Price,
                               Stock = p.Stock,

                               Image = p.Image,


                               IsDelete = v.IsDelete,
                               CreateBy = v.CreateBy,
                               CreateDate = v.CreateDate,

                               UpdateBy = p.UpdateBy,
                               UpdateDate = p.UpdateDate,

                               VarId = v.Id,
                               VarName = v.VarName,

                               CatId = v.CatId,
                               CatName = c.CatName
                           }).FirstOrDefault();

            return dataProduct;
        }

        [HttpPost("PostFoodProduct")]
        public VMRespons PostProduct(FoodProduct data)
        {
            data.IsDelete = false;
            data.CreateDate = DateTime.Now;
            try
            {
                db.Add(data);
                db.SaveChanges();

                respon.Message = "Data success saved";
            }
            catch (Exception ex)
            {
                respon.Success = false;
                respon.Message = "Failed save: " + ex.InnerException;
            }
            return respon;
        }

        [HttpPut("PutFoodProduct")]
        public VMRespons PutProduct(FoodProduct data)
        {
            FoodProduct dataOld = db.FoodProducts.Where(a => a.Id == data.Id).FirstOrDefault();

            if (dataOld != null)
            {
                dataOld.VarId = data.VarId;
                dataOld.NameProduct = data.NameProduct;
                dataOld.Price = data.Price;
                dataOld.Stock = data.Stock;

                if (data.Image != "")
                {
                    dataOld.Image = data.Image;
                }

                dataOld.UpdateDate = DateTime.Now;
                dataOld.UpdateBy = data.UpdateBy;

                try
                {
                    db.Update(dataOld);
                    db.SaveChanges(true);

                    respon.Message = "Data success update";
                }
                catch (Exception e)
                {
                    respon.Success = false;
                    respon.Message = "Failed save: " + e.InnerException;
                }
            }
            else
            {
                respon.Success = false;
                respon.Message = "Data not found";
            }

            return respon;
        }

        [HttpDelete("DeleteFoodProduct/{id}")]
        public VMRespons DeleteProduct(int id)
        {
            FoodProduct data = db.FoodProducts.Where(a => a.Id == id).FirstOrDefault();

            if (data != null)
            {
                data.IsDelete = true;
                try
                {
                    db.Update(data);
                    db.SaveChanges();

                    respon.Message = "Data Success Delete";
                }
                catch (Exception e)
                {
                    respon.Success = false;
                    respon.Message = "Delete failed: " + e.Message;
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
