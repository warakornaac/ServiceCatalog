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
            SearchProductTru = new SearchProductTruByStatus().SearchProductTru("", "161", "");
         
            @ViewBag.listBrand = listBrandMaster;
            @ViewBag.listSearchProductTru = SearchProductTru;

            return View("IndexCallItemAutomate", new
            {
                @ViewBag.listBrand,
                @ViewBag.listSearchProductTru
            });
        }
        public ActionResult GetListItemAutomate(string Stkcode, string BrandId, string apiStatus)
        {
            var SearchProductTru = new List<StoredSearchProductTruByStatusModel>();
            //if (!string.IsNullOrEmpty(apiStatus))
            //{
                SearchProductTru = new SearchProductTruByStatus().SearchProductTru(Stkcode, BrandId, apiStatus);
            //}
            @ViewBag.listSearchProductTru = SearchProductTru;
            return PartialView("_ListItemAutomate", new
            {
                @ViewBag.listSearchProductTru
            });
        }
    }
}