using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.Data;
using ServiceCatalog.Models;
using ServiceCatalog.Library;

namespace ServiceCatalog.Data
{
    public class SearchItemProduct : MsSQL
    {
        public SearchItemProduct() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<StoredSearchItemProductsModel> SearchItem(string Stkcode, string BrandId, string RowNumber, string ApiStatus)
        {
            var p = new SqlParameters();
            p.AddParams("@inStkcode", Stkcode);
            p.AddParams("@inBrandId", BrandId);
            p.AddParams("@inStatus", ApiStatus.ToTrim());
            p.AddParams("@inRowNumber", RowNumber);

            var table = GetData(CmdStore("P_Search_Item_Product", p));
            return ConvertExtension.ConvertDataTable<StoredSearchItemProductsModel>(GetData(CmdStore("P_Search_Item_Product", p)));
        }
    }
}
