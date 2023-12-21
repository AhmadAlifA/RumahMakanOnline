using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_API.Models;
using RumahMakanOnline_FE.Services;
using RumahMakanOnline_ViewModels;

namespace RumahMakanOnline_FE.Controllers
{
    public class AuthController : Controller
    {
        private AuthService auth_service;
        private int idUser = 1;

        public AuthController(AuthService _authService)
        {
            this.auth_service = _authService;
        }

        public IActionResult Login()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Login(VMUserCustomer data)
        {
            VMUserCustomer dataUserCus = await auth_service.Login(data);
            VMRespons respon = new VMRespons();
            if (dataUserCus != null)
            {
                HttpContext.Session.SetString("NameCustomer", dataUserCus.CusName);
                HttpContext.Session.SetInt32("IdCustomer", dataUserCus.CusId);
                HttpContext.Session.SetInt32("IdUser", dataUserCus.UserId);
                HttpContext.Session.SetInt32("RoleId", dataUserCus.RoleId);
                respon.Message = $"Welcome, {dataUserCus.CusName} to Rumah Makan Online application";
            }
            else
            {
                respon.Success = false;
                respon.Message = "Sorry your email or password not register";
            }
            return Json(respon);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Json(true);
        }
        public async Task<IActionResult> Register()
        {
            List<TblRole> dataRole = await auth_service.AllRole();
            ViewBag.DataRole = dataRole;
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Register(VMUserCustomer data)
        {
            VMRespons respon = new VMRespons();

            List<TblRole> dataRole = await auth_service.AllRole();
            ViewBag.DataRole = dataRole;



            data.CreateBy = idUser;

            respon = await auth_service.Register(data);

            return Json(respon);
            //return Json(true);
        }
        public async Task<JsonResult> CheckEmailDuplicate(string email)
        {
            bool isDuplicate = await auth_service.CheckEmailDuplicate(email);
            return Json(isDuplicate);
        }
    }
}
