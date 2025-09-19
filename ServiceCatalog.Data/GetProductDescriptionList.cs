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
    public class GetProductDescriptionList : MsSQL
    {
        public GetProductDescriptionList() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<ProductDescriptionModel> Get(string Stkcode)
        {
            var p = new SqlParameters();
            p.AddParams("@inStkcode", Stkcode);

            var table = GetData(CmdStore("P_Get_Product_Description", p));
            return ConvertExtension.ConvertDataTable<ProductDescriptionModel>(GetData(CmdStore("P_Get_Product_Description", p)));
        }
    }
}
