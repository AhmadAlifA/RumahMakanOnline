using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_API.Models;
using RumahMakanOnline_ViewModels;

namespace RumahMakanOnline_API.Controllers
{
    [Route("[controller]")]

    [ApiController]
    public class ApiAuthController : Controller
    {
        private readonly RestaurantKakiTujuhContext db;
        private VMRespons respon = new VMRespons();

        public ApiAuthController(RestaurantKakiTujuhContext db)
        {
            this.db = db;
        }
        [HttpGet("GetAllRole")]
        public List<TblRole> GetAllRole()
        {
            List<TblRole> dataRole = new List<TblRole>();

            dataRole = db.TblRoles.Where(a => a.IsDelete == false).ToList();

            return dataRole;
        }
        [HttpPost("Register")]
        public VMRespons Register(VMUserCustomer data)
        {
            TblUser user = new TblUser();
            user.Email = data.Email;
            user.Password = data.Password;
            user.RoleId = data.RoleId;
            user.IsDelete = false;
            user.CreateBy = data.CreateBy;
            user.CreateDate = DateTime.Now;

            try
            {
                db.Add(user);
                db.SaveChanges();

                Customer customer = new Customer();
                customer.UserId = user.Id;
                customer.Name = data.CusName;
                customer.Address = data.Address;
                customer.ContactNum = data.ContactNum;
                customer.IsDelete = false;
                customer.CreateBy = data.CreateBy;
                customer.CreateDate = DateTime.Now;

                try
                {
                    db.Add(customer);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    respon.Success = false;
                    respon.Message = e.Message;
                }
            }
            catch (Exception e)
            {
                respon.Success = false;
                respon.Message = e.Message;
            }

            return respon;
        }
        [HttpGet("CheckEmail/{email}")]
        public bool CheckEmail(string email)
        {
            bool checkEmail = false;
            int countEmail = 0;
            countEmail = db.TblUsers.Where(a => a.Email == email).Count();

            //TblUser data = db.TblUsers.Where(a => a.Email == email).FirstOrDefault();
            //if(data != null)
            //{
            //    checkEmail = true;
            //}

            if (countEmail > 0)
            {
                checkEmail = true;
            }
            return checkEmail;
        }
        [HttpGet("Login/{email}/{password}")]
        public VMUserCustomer Login(string email, string password)
        {
            VMUserCustomer data = (from u in db.TblUsers
                                   join c in db.Customers
                                   on u.Id equals c.UserId
                                   where u.Email == email && u.Password == password
                                   select new VMUserCustomer
                                   {
                                       UserId = u.Id,
                                       Email = u.Email,
                                       CusName = c.Name,
                                       RoleId = u.RoleId,
                                       CusId = c.Id
                                   }).FirstOrDefault();
            return data;
        }
    }
}
