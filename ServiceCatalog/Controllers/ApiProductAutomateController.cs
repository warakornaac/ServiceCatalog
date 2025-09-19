using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceCatalog.Models;
using ServiceCatalog.Data;
using ServiceCatalog.Library;

namespace ServiceCatalog.Controllers
{
    public class ApiProductAutomateController : Controller
    {
        // GET: ApiProductAutomate
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CallByItemAutomate()
        {
            List<SelectListItem> listBrandMaster = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("P_Search_Brand", connection);
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listBrandMaster.Add(new SelectListItem
                    {
                        Value = reader["BrandId"].ToString(),
                        Text = $"{reader["BrandId"]}/{reader["BrandName"]}"
                    });
                }
            }
            var SearchProductTru = new  List<StoredSearchProductTruByStatusModel>();
            SearchProductTru = new SearchProductTruByStatus().SearchProductTru("", "", "", "");
         
            @ViewBag.listBrand = listBrandMaster;
            @ViewBag.listSearchProductTru = SearchProductTru;

            return View("IndexCallItemAutomate", new
            {
                @ViewBag.listBrand,
                @ViewBag.listSearchProductTru
            });
        }
        public ActionResult GetListItemAutomate(string Stkcode, string BrandId, string RowNumber, string ApiStatus)
        {
            var SearchProductTru = new List<StoredSearchProductTruByStatusModel>();
            //if (!string.IsNullOrEmpty(apiStatus))
            //{
                SearchProductTru = new SearchProductTruByStatus().SearchProductTru(Stkcode, BrandId, RowNumber, ApiStatus);
            //}
            @ViewBag.listSearchProductTru = SearchProductTru;
            return PartialView("_ListItemAutomate", new
            {
                @ViewBag.listSearchProductTru
            });
        }
        public JsonResult GetProductApiAutomateCount()
        {
            List<ListProductApiAutomateCount> ListProductApiAutomateCount = new List<ListProductApiAutomateCount>();
            var connectionString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Product_Api_Automate_Count", Connection);
            command.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                ListProductApiAutomateCount.Add(new ListProductApiAutomateCount()
                {
                    sumItemAll = dr["sumItemAll"].ToString(),
                    sumItemAllSuccess = dr["sumItemAllSuccess"].ToString(),
                    sumItemCurrentAll = dr["sumItemCurrentAll"].ToString(),
                    sumItemCurrentAllSuccess = dr["sumItemCurrentAllSuccess"].ToString(),
                    sumItemCurrentTaskPendding = dr["sumItemCurrentTaskPendding"].ToString(),
                    sumItemCurrentTaskProcess = dr["sumItemCurrentTaskProcess"].ToString(),
                    sumItemCurrentTaskComplete = dr["sumItemCurrentTaskComplete"].ToString(),
                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(ListProductApiAutomateCount, JsonRequestBehavior.AllowGet);
        }
        public class ListProductApiAutomateCount
        {
            public string sumItemAll { get; set; }
            public string sumItemAllSuccess { get; set; }
            public string sumItemCurrentAll { get; set; }
            public string sumItemCurrentAllSuccess { get; set; }
            public string sumItemCurrentTaskPendding { get; set; }
            public string sumItemCurrentTaskProcess { get; set; }
            public string sumItemCurrentTaskComplete { get; set; }
        }
    }
}