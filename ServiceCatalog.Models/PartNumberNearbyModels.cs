using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class PartNumberNearbyData
    {
        public List<PartNumberNearby> PartNumberNearby { get; set; }
    }
    public class PartNumberNearby
    {
        public int? articleId { get; set; }
        public string articleName { get; set; }
        public string articleNo { get; set; }
        public string articleSearchNo { get; set; }
        public int articleStateId { get; set; }
        public string brandName { get; set; }
        public int brandNo { get; set; }
        public int numberType { get; set; }
    }
}
