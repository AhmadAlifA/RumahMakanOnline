using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumahMakanOnline_ViewModels
{
    public class VMFoodVariant
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        public string CatName { get; set; }
        public string VarName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
