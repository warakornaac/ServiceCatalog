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
    public class GetLinkage : MsSQL
    {
        public GetLinkage() : base(Utils.GetConfig("ServiceCatalogDB")) { }
        public List<StoreGetLinkageDataModel> GetLinkageData(string STKCOD)
        {
            var p = new SqlParameters();
            p.AddParams("@inSTKCOD", STKCOD);

            return ConvertExtension.ConvertDataTable<StoreGetLinkageDataModel>(GetData(CmdStore("P_Get_LinkageData", p)));
        }
    }
}
