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
    public class GetProductSpecList : MsSQL
    {
        public GetProductSpecList() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<ProductSpecModel> Get(string Stkcode)
        {
            var p = new SqlParameters();
            p.AddParams("@inStkcode", Stkcode);

            var table = GetData(CmdStore("P_Get_Product_Spec", p));
            return ConvertExtension.ConvertDataTable<ProductSpecModel>(GetData(CmdStore("P_Get_Product_Spec", p)));
        }
    }
}
