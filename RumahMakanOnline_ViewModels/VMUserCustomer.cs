using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumahMakanOnline_ViewModels
{
    public class VMUserCustomer
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }

        public int CusId { get; set; }
        public string CusName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string ContactNum { get; set; } = null!;

        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
