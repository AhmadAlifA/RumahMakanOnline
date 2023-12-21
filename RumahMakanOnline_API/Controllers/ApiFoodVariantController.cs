using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_API.Models;
using RumahMakanOnline_ViewModels;

namespace RumahMakanOnline_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiFoodVariantController : ControllerBase
    {
        private readonly RestaurantKakiTujuhContext db;
        private VMRespons respon = new VMRespons();

        public ApiFoodVariantController(RestaurantKakiTujuhContext _db)
        {
            this.db = _db;
        }

        [HttpGet("GetAllFoodVariant")]
        public List<VMFoodVariant> GetAllFoodVariant()
        {
            List<VMFoodVariant> dataVariant = new List<VMFoodVariant>();
            //dataVariant = db.TblVariants.Where(a => a.IsDelete == false).ToList();
            dataVariant = (from v in db.FoodVariants
                           join c in db.FoodCategories
                           on v.CatId equals c.Id
                           where v.IsDelete == false
                           select new VMFoodVariant
                           {
                               Id = v.Id,
                               VarName = v.VarName,
                               Description = v.Description,

                               CatId = v.CatId,
                               CatName = c.CatName,

                               IsDelete = v.IsDelete
                           }).ToList();

            return dataVariant;
        }

        [HttpGet("GetById/{id}")]
        public VMFoodVariant GetById(int id)
        {
            VMFoodVariant dataVariant = new VMFoodVariant();
            dataVariant = (from v in db.FoodVariants
                           join c in db.FoodCategories
                           on v.CatId equals c.Id
                           where v.Id == id && v.IsDelete == false
                           select new VMFoodVariant
                           {
                               Id = v.Id,
                               VarName = v.VarName,
                               Description = v.Description,

                               IsDelete = v.IsDelete,
                               CreateBy = v.CreateBy,
                               CreateDate = v.CreateDate,

                               UpdateBy = v.UpdateBy,
                               UpdateDate = v.UpdateDate,

                               CatId = v.CatId,
                               CatName = c.CatName,
                           }).FirstOrDefault();

            return dataVariant;
        }

        [HttpGet("GetFoodVariantByCategory/{idcategory}")]
        public List<VMFoodVariant> GetDataByCategory(int idcategory)
        {
            List<VMFoodVariant> dataList = (from v in db.FoodVariants
                                        where v.CatId == idcategory && v.IsDelete == false
                                        select new VMFoodVariant
                                        {
                                            Id = v.Id,
                                            VarName = v.VarName,
                                        }).ToList();
            return dataList;
        }

        [HttpPost("PostFoodVariant")]
        public VMRespons PostFoodVariant(FoodVariant data)
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

        [HttpPut("PutFoodVariant")]
        public VMRespons PutFoodVariant(FoodVariant data)
        {
            FoodVariant dataOld = db.FoodVariants.Where(a => a.Id == data.Id).FirstOrDefault();

            if (dataOld != null)
            {
                dataOld.CatId = data.CatId;
                dataOld.VarName = data.VarName;
                dataOld.Description = data.Description;

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

        [HttpDelete("DeleteFoodVariant/{id}")]
        public VMRespons DeleteFoodVariant(int id)
        {
            FoodVariant data = db.FoodVariants.Where(a => a.Id == id).FirstOrDefault();

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
