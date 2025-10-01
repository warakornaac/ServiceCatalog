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
    public class GetProductImageList : MsSQL
    {
        public GetProductImageList() : base(Utils.GetConfig("ServiceCatalogDB"))
        {

        }
        public List<ProductImageModel> Get(string Stkcode)
        {
            var p = new SqlParameters();
            p.AddParams("@inStkcode", Stkcode);

            var table = GetData(CmdStore("P_Get_Product_Image", p));
            return ConvertExtension.ConvertDataTable<ProductImageModel>(GetData(CmdStore("P_Get_Product_Image", p)));
        }

    }
}
