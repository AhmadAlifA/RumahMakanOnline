using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumahMakanOnline_ViewModels
{
    public class VMMenu
    {
        public int Id { get; set; }
        public string MenuName { get; set; } = null!;
        public string? MenuAction { get; set; } = null!;
        public string? MenuController { get; set; } = null!;
        public string? MenuIcon { get; set; }
        public int? MenuSorting { get; set; }
        public bool? IsParent { get; set; }
        public int? MenuParent { get; set; }
        public int RoleId { get; set; }
        public List<VMMenu> ChildMenu { get; set; }
    }
}
