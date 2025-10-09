using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class StoreGetLinkageDataModel
    {
        public string Stkcode { get; set; }
        public int SeqLinkage { get; set; }
        public string KType { get; set; }
        public string TruType { get; set; }
        public string Maker { get; set; }
        public string Model { get; set; }
        public string Body { get; set; }
        public string Engine { get; set; }
        public string DriveType { get; set; }
        public string YearFrom { get; set; }
    }
}
