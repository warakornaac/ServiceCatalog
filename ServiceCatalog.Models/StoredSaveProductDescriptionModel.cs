using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class StoredSaveProductDescriptionModel
    {
        public string Stkcode { get; set; }
        public string Seq { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InsertedBy { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
