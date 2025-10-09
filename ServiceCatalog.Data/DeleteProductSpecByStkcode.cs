using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.Data;
using ServiceCatalog.Models;
using ServiceCatalog.Library;

namespace ServiceCatalog.Data
{
    public class DeleteProductSpecByStkcode : MsSQL
    {
        public DeleteProductSpecByStkcode() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<object> Delete(string tableName, string stkcode, int seq)
        {
            var p = new SqlParameters();
            p.AddParams("@inTableName", tableName.ToTrim());
            p.AddParams("@inStkcode", stkcode.ToTrim());
            p.AddParams("@inSeq", seq);
            p.AddParams("@inUserName", "");

            var table = GetData(CmdStore("P_Delete_Part_Spec_By_Stkcode", p));
            return ConvertExtension.ConvertDataTable<object>(table);
        }
    }
}
