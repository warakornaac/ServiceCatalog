using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    public class VIO_MarketSegment
    {
        public string ID { get; set; }
        public string MarketSegment { get; set; }
    }
    public class VIO_VehicleSegment
    {
        public string ID { get; set; }
        public string VehicleSegment { get; set; }
    }
    public class VIO_Maker
    {
        public string ID { get; set; }
        public string Maker { get; set; }
    }
    public class VIO_ModelRange
    {
        public string ID { get; set; }
        public string ModelRange { get; set; }
        public string Maker_ID { get; set; }
    }
    public class VIO_Model
    {
        public string ID { get; set; }
        public string Model { get; set; }
        public string Maker_ID { get; set; }
        public string MarketSegment_ID { get; set; }
        public string ModelRange_ID { get; set; }
    }
    public class VIO_Body
    {
        public string ID { get; set; }
        public string Body { get; set; }
        public string BodyType { get; set; }
        public string Model_ID { get; set; }
        public string ModelRange_ID { get; set; }
        public string Maker_ID { get; set; }
        public string VehicleSegment_ID { get; set; }
        public string MarketSegment_ID { get; set; }
    }
    public class VIO_Engine
    {
        public string ID { get; set; }
        public string EngineType { get; set; }
        public string FuelType { get; set; }
        public string Strokes { get; set; }
        public string Maker_ID { get; set; }
        public string ModelRange_ID { get; set; }
        public string Model_ID { get; set; }
        public string Body_ID { get; set; }
    }
    public class VIO_TruData
    {
        public string MarketSegment_ID { get; set; }
        public string VehicleSegment_ID { get; set; }
        public string Maker_ID { get; set; }
        public string ModelRange_ID { get; set; }
        public string Model_ID { get; set; }
        public string Body_ID { get; set; }
        public string Engine_ID { get; set; }
        public string KType { get; set; }
        public string DriveType { get; set; }
        public string YearFrom { get; set; }
        public string YearTo { get; set; }
        public string ThaiVIO { get; set; }
        public string TruType { get; set; }
    }
}
