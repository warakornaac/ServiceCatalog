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
    public class SearchLinkageVehicle : MsSQL
    {
        public SearchLinkageVehicle() : base(Utils.GetConfig("ServiceCatalogDB")) { }
        public List<StoreSearchVehicleLinkageModel> SearchVehicle(string STKCOD, string marketID, string vehicleID, string maker, string rangID, string modelID, string bodyID, string engineID)
        {
            var p = new SqlParameters();
            p.AddParams("@inSTKCOD", STKCOD);
            p.AddParams("@inmarketseID", marketID);
            p.AddParams("@invehicleseID", vehicleID);
            p.AddParams("@inmakerID", maker);
            p.AddParams("@inmodelrangeID", rangID);
            p.AddParams("@inmodelID", modelID);
            p.AddParams("@inBodyID", bodyID);
            p.AddParams("@inEngineID", engineID);

            return ConvertExtension.ConvertDataTable<StoreSearchVehicleLinkageModel>(GetData(CmdStore("P_Search_Vehicle_Linkage", p)));
        }
    }
}
