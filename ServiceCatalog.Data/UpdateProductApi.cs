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
    public class UpdateProductApi : MsSQL
    {
        public UpdateProductApi() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<StoredUpdateProductApiModel> SaveProductApi(string tableName, string stkcod, int brandId, int seq, string title, string description, string maker, string model, string userName)
        {
            var p = new SqlParameters();
            p.AddParams("@inTableName", tableName.ToTrim());
            p.AddParams("@inStkcode", stkcod.ToTrim());
            p.AddParams("@inBrandId", brandId);
            p.AddParams("@inSeq", seq);
            p.AddParams("@inTitle", title.ToTrim());
            p.AddParams("@inDescription", description.ToTrim());
            p.AddParams("@inMaker", maker.ToTrim());
            p.AddParams("@inModel", model.ToTrim());
            p.AddParams("@inUserName", userName.ToTrim());

            var table = GetData(CmdStore("P_Save_Product_Api", p));
            //return ConvertExtension.ConvertDataTable<StoredUpdateProductApiModel>(GetData(CmdStore("P_Save_Product_Api", p)));
            return ConvertExtension.ConvertDataTable<StoredUpdateProductApiModel>(table);
        }
    }
}
