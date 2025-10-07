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
    public class GetLinkageDataList : MsSQL
    {
        public GetLinkageDataList() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<StoredGetLinkageDataModel> Get(string Stkcode)
        {
            var p = new SqlParameters();
            p.AddParams("@inSTKCOD", Stkcode);

            var table = GetData(CmdStore("P_Get_LinkageData", p));
            return ConvertExtension.ConvertDataTable<StoredGetLinkageDataModel>(GetData(CmdStore("P_Get_LinkageData", p)));
        }
    }
}
