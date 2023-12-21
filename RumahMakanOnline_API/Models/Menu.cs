using System;
using System.Collections.Generic;

namespace RumahMakanOnline_API.Models
{
    public partial class Menu
    {
        public int Id { get; set; }
        public string MenuName { get; set; } = null!;
        public string? MenuAction { get; set; }
        public string? MenuController { get; set; }
        public string? MenuIcon { get; set; }
        public int? MenuSorting { get; set; }
        public bool? IsParent { get; set; }
        public int? MenuParent { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
