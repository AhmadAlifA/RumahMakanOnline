using System;
using System.Collections.Generic;

namespace RumahMakanOnline_API.Models
{
    public partial class Customer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string? ContactNum { get; set; }
        public string? Address { get; set; }
        public bool? IsDelete { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
