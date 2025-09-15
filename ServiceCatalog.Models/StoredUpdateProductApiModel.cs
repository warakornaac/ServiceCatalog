using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class StoredUpdateProductApiModel
    {
        public string Table { get; set; }
        public string Stkcod { get; set; }
        public int BrandId { get; set; }
        public string Seq { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Maker { get; set; }
        public string Model { get; set; }
        public string InsertedBy { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
