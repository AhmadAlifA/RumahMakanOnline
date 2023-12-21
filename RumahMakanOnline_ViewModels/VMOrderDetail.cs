﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumahMakanOnline_ViewModels
{
    public class VMOrderDetail
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public int ProductId { get; set; }
        public string NameProduct { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal SubTotal { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
