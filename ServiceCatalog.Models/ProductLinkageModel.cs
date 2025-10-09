using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class ProductLinkageModel
    {
        public string Stkcode { get; set; }
        public string SeqLinkage { get; set; }
        public string KType { get; set; }
        public string TruType { get; set; }
        public string ProductId { get; set; }
        public string SegmentName { get; set; }
        public string MakerName { get; set; }
        public string ModelName { get; set; }
        public string InsertedBy { get; set; }
        public string InsertedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
}
