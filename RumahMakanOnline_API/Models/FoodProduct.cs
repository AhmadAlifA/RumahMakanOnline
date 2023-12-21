using System;
using System.Collections.Generic;

namespace RumahMakanOnline_API.Models
{
    public partial class FoodProduct
    {
        public int Id { get; set; }
        public int VarId { get; set; }
        public string NameProduct { get; set; } = null!;
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
