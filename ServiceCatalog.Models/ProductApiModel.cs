using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class ProductApiModel
    {
        public string Stkcode { get; set; }
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string ProductDescriptionCount { get; set; }
        public string ProductDescriptionDate { get; set; }
        public string ProductSpecCount { get; set; }
        public string ProductSpecDate { get; set; }
        public string ProductOemCount { get; set; }
        public string ProductOemDate { get; set; }
        public string ProductCompetitorCount { get; set; }
        public string ProductCompetitorDate { get; set; }
        public string ProductImageCount { get; set; }
        public string ProductImageDate { get; set; }
        public string ProductLinkageCount { get; set; }
        public string ProductLinkageDate { get; set; }
        public string ApiStatus { get; set; }
        public string ApiStatusRemark { get; set; }
        public string ApiStartDate { get; set; }
        public string ApiEndDate { get; set; }
        public string InsertedBy { get; set; }
        public string InsertedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
}
