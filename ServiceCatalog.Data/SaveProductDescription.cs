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
    public class SaveProductDescription : MsSQL
    {
        public SaveProductDescription() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<StoredSaveProductDescriptionModel> Save(string tableName, string stkcode, int seq, string title, string description, string userName)
        {
            var p = new SqlParameters();
            p.AddParams("@inTableName", tableName.ToTrim());
            p.AddParams("@inStkcode", stkcode.ToTrim());
            p.AddParams("@inSeq", seq);
            p.AddParams("@inTitle", title.ToTrim());
            p.AddParams("@inDescription", description.ToTrim());
            p.AddParams("@inUserName", userName.ToTrim());

            var table = GetData(CmdStore("P_Save_Product_Description", p));
            return ConvertExtension.ConvertDataTable<StoredSaveProductDescriptionModel>(table);
        }
    }
}
