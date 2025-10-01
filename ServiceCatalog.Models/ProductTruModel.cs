using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class ProductTruModel
    {
        public string Stkcode { get; set; }
        public string Stkdesc { get; set; }
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string Category { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Day { get; set; }
        public string ApiCallDate { get; set; }
        public string ApiStatus { get; set; }
        public string ApiDate { get; set; }
    }
}
