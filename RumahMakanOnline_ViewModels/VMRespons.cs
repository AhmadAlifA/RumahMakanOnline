using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumahMakanOnline_ViewModels
{
    public class VMRespons
    {
        public VMRespons()
        {
            Success = true;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public object Content { get; set; }
    }
}
