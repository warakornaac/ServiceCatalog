using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class StoredSearchItemProductsModel
    {
        public string Stkcode { get; set; }
        public string Stkdesc { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string ApiStatus { get; set; }
        public string ApiCallDate { get; set; }
        public string ApiStartDate { get; set; }
        public string ApiEndDate { get; set; }
    }
}
