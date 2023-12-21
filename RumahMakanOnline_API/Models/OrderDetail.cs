﻿using System;
using System.Collections.Generic;

namespace RumahMakanOnline_API.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public decimal SubTotal { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
