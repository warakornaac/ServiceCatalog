using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceCatalog.Library;
using ServiceCatalog.Models;
using My.Data;

namespace ServiceCatalog.Data
{
    public class GetProductLinkageList : MsSQL
    {
        public GetProductLinkageList() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<ProductLinkageModel> Get(string Stkcode)
        {
            var p = new SqlParameters();
            p.AddParams("@inStkcode", Stkcode);

            var table = GetData(CmdStore("P_Get_Product_Linkage", p));
            return ConvertExtension.ConvertDataTable<ProductLinkageModel>(GetData(CmdStore("P_Get_Product_Linkage", p)));
        }
    }
}
