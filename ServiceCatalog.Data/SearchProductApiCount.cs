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
    public class SearchProductApiCount : MsSQL
    {
        public SearchProductApiCount() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<StoredProductApiCountModel> Get(string Stkcode)
        {
            var p = new SqlParameters();
            p.AddParams("@inStkcode", Stkcode);

            var table = GetData(CmdStore("P_Get_Product_Api_Count", p));
            return ConvertExtension.ConvertDataTable<StoredProductApiCountModel>(GetData(CmdStore("P_Get_Product_Api_Count", p)));
        }
    }
}
