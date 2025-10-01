using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class ProductCompetitorModel
    {
        public string Stkcode { get; set; }
        public string SeqCompetitor { get; set; }
        public string PartNo { get; set; }
        public string BrandName { get; set; }
        public string InsertedBy { get; set; }
        public string InsertedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
}
