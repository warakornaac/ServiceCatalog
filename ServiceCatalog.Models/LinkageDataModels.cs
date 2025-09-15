using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class LinkageData
    {
        public List<LinkageDetails> LinkageDetails { get; set; }
    }
    public class LinkageDetails
    {
        public int? LinkageTargetId { get; set; }
        public string LinkageTargetType { get; set; }
        public string SubLinkageTargetType { get; set; }
        public string MfrName { get; set; }
        public string VehicleModelSeriesName { get; set; }
        public string BeginYearMonth { get; set; }
        public string EndYearMonth { get; set; }
        public string ImageURL { get; set; }
        public string DriveType { get; set; }
        public string BodyStyle { get; set; }
        public string FuelMixtureFormationType { get; set; }
        public string FuelType { get; set; }
        public string EngineType { get; set; }
        public string Engines { get; set; }
    }
}
