using System;
using System.Collections.Generic;

namespace RumahMakanOnline_API.Models
{
    public partial class OrderHeader
    {
        public int Id { get; set; }
        public string CodeTransaction { get; set; } = null!;
        public int CustId { get; set; }
        public decimal Amount { get; set; }
        public int? TotalQty { get; set; }
        public bool? IsCheckout { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
