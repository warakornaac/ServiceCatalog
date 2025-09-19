using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class VehicleInfo
    {
        public string KType { get; set; }
        public string MarketSegment { get; set; }
        public string VehicleSegment { get; set; }
        public string Maker { get; set; }
        public string ModelRange { get; set; }
        public string Model { get; set; }
        public string Body { get; set; }
        public string BodyType { get; set; }
        public string DriveType { get; set; }
        public string EngineType { get; set; }
        public string Strokes { get; set; }
        public string FuelType { get; set; }
        public string YearFrom { get; set; }
        public string YearTo { get; set; }
        public string ThailandVIO { get; set; }
        public string Flag { get; set; }
    }

    public class VIO_DATA
    {
        public string KType { get; set; }
        public string MarketSegment { get; set; }
        public string VehicleSegment { get; set; }
        public string Maker { get; set; }
        public string ModelRange { get; set; }
        public string Model { get; set; }
        public string Body { get; set; }
        public string BodyType { get; set; }
        public string DriveType { get; set; }
        public string EngineType { get; set; }
        public string Strokes { get; set; }
        public string FuelType { get; set; }
        public string YearFrom { get; set; }
        public string YearTo { get; set; }
        public string ThailandVIO { get; set; }
        public string TruType { get; set; }
    }
}
