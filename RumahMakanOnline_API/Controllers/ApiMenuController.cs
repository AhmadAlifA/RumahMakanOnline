using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_API.Models;
using RumahMakanOnline_ViewModels;

namespace RumahMakanOnline_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiMenuController : Controller
    {
        private readonly RestaurantKakiTujuhContext db;
        private VMRespons respon = new VMRespons();
        public ApiMenuController(RestaurantKakiTujuhContext db)
        {
            this.db = db;
        }
        [HttpGet("GetParent/{RoleId}")]
        public List<VMMenu> GetParent(int RoleId)
        {
            List<VMMenu> dataMenu = new List<VMMenu>();

            dataMenu = (from m in db.Menus
                        join ma in db.MenuAccesses
                        on m.Id equals ma.MenuId
                        where m.IsParent == true && ma.RoleId == RoleId && m.IsDelete == false
                        select new VMMenu
                        {
                            Id = m.Id,
                            MenuName = m.MenuName,
                            MenuAction = m.MenuAction,
                            MenuController = m.MenuController,
                            MenuIcon = m.MenuIcon,
                            MenuSorting = m.MenuSorting,
                            IsParent = m.IsParent,
                            MenuParent = m.MenuParent,
                            RoleId = ma.RoleId,
                            ChildMenu = (from cm in db.Menus
                                         join mac in db.MenuAccesses
                                         on cm.Id equals mac.MenuId
                                         where cm.IsDelete == false && mac.RoleId == RoleId && cm.MenuParent == m.Id
                                         select new VMMenu
                                         {
                                             Id = cm.Id,
                                             MenuName = cm.MenuName,
                                             MenuAction = cm.MenuAction,
                                             MenuController = cm.MenuController,
                                             MenuIcon = cm.MenuIcon,
                                             MenuSorting = cm.MenuSorting,
                                             IsParent = cm.IsParent,
                                             MenuParent = cm.MenuParent,
                                             RoleId = mac.RoleId
                                         }).ToList()
                        }).ToList();
            return dataMenu;
        }
    }
}
