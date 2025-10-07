using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class StoredProductApiCountModel
    {
        public string Stkcode { get; set; }
        public string Stkdes { get; set; }
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public int countProductDes { get; set; }
        public int countProductSpec { get; set; }
        public int countProductImage { get; set; }
        public int countProductOem { get; set; }
        public int countProductCom { get; set; }
        public int countProductLinkage { get; set; }
        public string ApiStatus { get; set; }
        public string ApiCallDate { get; set; }
    }
}
