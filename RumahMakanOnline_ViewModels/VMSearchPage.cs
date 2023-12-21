using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumahMakanOnline_ViewModels
{
    public class VMSearchPage
    {
        public string sortOrder { get; set; }
        public string searchString { get; set; }
        public string CurrentFilter { get; set; }
        public int? pageNumber { get; set; }
        public int? pageSize { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? MinOrderDate { get; set; }
        public DateTime? MaxOrderDate { get; set; }
    }
}
