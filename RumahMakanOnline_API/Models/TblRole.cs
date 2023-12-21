using System;
using System.Collections.Generic;

namespace RumahMakanOnline_API.Models
{
    public partial class TblRole
    {
        public int Id { get; set; }
        public string? RoleName { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
