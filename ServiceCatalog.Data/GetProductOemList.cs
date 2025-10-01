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
    public class GetProductOemList : MsSQL
    {
        public GetProductOemList() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<ProductOemModel> Get(string Stkcode)
        {
            var p = new SqlParameters();
            p.AddParams("@inStkcode", Stkcode);

            var table = GetData(CmdStore("P_Get_Product_Oem", p));
            return ConvertExtension.ConvertDataTable<ProductOemModel>(GetData(CmdStore("P_Get_Product_Oem", p)));
        }
    }
}
