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
    public class GetProductCompetitorList : MsSQL
    {
        public GetProductCompetitorList() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<ProductCompetitorModel> Get(string Stkcode)
        {
            var p = new SqlParameters();
            p.AddParams("@inStkcode", Stkcode);

            var table = GetData(CmdStore("P_Get_Product_Competitor", p));
            return ConvertExtension.ConvertDataTable<ProductCompetitorModel>(GetData(CmdStore("P_Get_Product_Competitor", p)));
        }
    }
}
