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
    public class GetExportProductApi : MsSQL
    {
        public GetExportProductApi() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<string> Get(string sheet)
        {
            var p = new SqlParameters();
            p.AddParams("@inSheet", sheet);

            var table = GetData(CmdStore("P_Export_Product_Api", p));
            return ConvertExtension.ConvertDataTable<string>(GetData(CmdStore("P_Export_Product_Api", p)));
        }
    }
}
