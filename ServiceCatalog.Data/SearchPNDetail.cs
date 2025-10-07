using My.Data;
using ServiceCatalog.Library;
using ServiceCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCatalog.Data
{
    public class SearchPNDetail : MsSQL
    {
        public SearchPNDetail() : base(Utils.GetConfig("ServiceCatalogDB")) { }

        public List<PNDescripsModels> SearchPN(string STKCOD)
        {
            var p = new SqlParameters();
            p.AddParams("@inSTKCOD", STKCOD);

            return ConvertExtension.ConvertDataTable<PNDescripsModels>(GetData(CmdStore("P_Get_PNDescription", p)));
        }
    }
}
