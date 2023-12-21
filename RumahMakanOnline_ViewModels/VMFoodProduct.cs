using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumahMakanOnline_ViewModels
{
    public class VMFoodProduct
    {
        public int Id { get; set; }
        public string NameProduct { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int VarId { get; set; }
        public string VarName { get; set; }
        public int CatId { get; set; }
        public string CatName { get; set; }
        public string? Image { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
