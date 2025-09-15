using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Models
{
    //รวมข้อมูลรายละเอียดสินค้า
    public class ArticleDataModels
    {
        public List<GenericArticles> GenericArticles { get; set; }
        public List<Image> Images { get; set; }
        public List<OemNumber> OemNumbers { get; set; }
        public List<ArticleCriteria> ArticleCriteria { get; set; }
        public List<Pdf> Pdf { get; set; }
        public List<TradeNumberDetail> TradeNumberDetail { get; set; }
    }
    public class GenericArticles
    {
        public int genericArticleId { get; set; }
        //ชื่อสินค้า
        public string genericArticleDescription { get; set; } 
        public int assemblyGroupNodeId { get; set; }
        //ประเภทสินค้า
        public string assemblyGroupName { get; set; }
        public int legacyArticleId { get; set; }
        public List<string> linkageTargetTypes { get; set; }
    }
    public class Image
    {
        public string imageURL50 { get; set; }
        public string imageURL100 { get; set; }
        public string imageURL200 { get; set; }
        public string imageURL400 { get; set; }
        public string imageURL800 { get; set; }
        public string imageURL1600 { get; set; }
        public string imageURL3200 { get; set; }
        public string fileName { get; set; }
        public string typeDescription { get; set; }
        public int typeKey { get; set; }
        public string headerDescription { get; set; }
        public int headerKey { get; set; }
        public int sortNumber { get; set; }
        public string assetSource { get; set; }
    }
    public class OemNumber
    {
        public string articleNumber { get; set; }
        public int mfrId { get; set; }
        public string mfrName { get; set; }
        public bool matchesSearchQuery { get; set; }
    }
    //สเปคสินค้า
    public class ArticleCriteria
    {
        public int criteriaId { get; set; }
        public string criteriaDescription { get; set; }
        public string criteriaAbbrDescription { get; set; }
        public string criteriaUnitDescription { get; set; }
        public string criteriaType { get; set; }
        public string rawValue { get; set; }
        public string formattedValue { get; set; }
        public bool immediateDisplay { get; set; }
        public bool isMandatory { get; set; }
        public bool isInterval { get; set; }
    }
    public class Pdf
    {
        public string url { get; set; }
        public string fileName { get; set; }
        public string typeDescription { get; set; }
        public string headerDescription { get; set; }
        public int sortNumber { get; set; }
        public string assetSource { get; set; }
    }
    public class TradeNumberDetail
    {
        public string tradeNumber { get; set; }
        public bool isImmediateDisplay { get; set; }
    }
}
