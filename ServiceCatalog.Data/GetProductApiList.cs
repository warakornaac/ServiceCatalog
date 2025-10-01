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
    public class GetProductApiList : MsSQL
    {
        public GetProductApiList() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<ProductApiModel> Get()
        {
            var p = new SqlParameters();
           // p.AddParams("@inStkcode", Stkcode);

            var table = GetData(CmdStore("P_Get_Product_Api", p));
            return ConvertExtension.ConvertDataTable<ProductApiModel>(GetData(CmdStore("P_Get_Product_Api", p)));
        }
    }
}
