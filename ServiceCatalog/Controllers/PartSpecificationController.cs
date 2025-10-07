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
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ServiceCatalog.Models;
using ServiceCatalog.Data;
using ServiceCatalog.Library;

namespace ServiceCatalog.Controllers
{
    public class PartSpecificationController : Controller
    {
        // GET: PartSpecification
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ManagementPart()
        {
            List<SelectListItem> listProductName = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("P_Search_Product_Name", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inUserId", "warakorn.pra");
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listProductName.Add(new SelectListItem
                    {
                        Value = reader["PROD"].ToString(),
                        Text = $"{reader["PROD"]}/{reader["PRODNAM"]}"
                    });
                }
            }
            @ViewBag.listProductName = listProductName;
            return View("IndexManagementPart", new
            {
                @ViewBag.listProductName
            });
        }
        public ActionResult getDetailPartByStkcode(string Stkcode)
        {
            var ListProductApiCount = new List<StoredProductApiCountModel>();
            string message = string.Empty;
            try
            {
                ListProductApiCount = new SearchProductApiCount().Get(Stkcode);
                ViewBag.ListProductApiCount = ListProductApiCount;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ViewBag.ListProductApiCount = new List<object>();
            }
            ViewBag.Message = message;
            return PartialView("_DeatilPart", new
            {
                @ViewBag.Message,
                @ViewBag.ListProductApiCount,
            });
        }  
        //get detail part by tab
        public ActionResult getListDetailPartByTab(string tabName, string Stkcode)
        {
            string filePartName = string.Empty;
            string message = string.Empty;
            var listProductApiCount = new List<StoredProductApiCountModel>();
            var listProductDescription = new List<ProductDescriptionModel>();
            var listProductSpec = new List<ProductSpecModel>();
            var listProductImage = new List<ProductImageModel>();
            var listProductOem = new List<ProductOemModel>();
            var listProductLinkage = new List<StoredGetLinkageDataModel>();
            //listProductApiCount = new SearchProductApiCount().Get(Stkcode);
            try
            {
                if (!string.IsNullOrEmpty(tabName))
                {
                    switch (tabName)
                    {
                        case "tabDescription":
                            filePartName = "_ListPartDescription";
                            listProductDescription = new GetProductDescriptionList().Get(Stkcode);
                            ViewBag.ListPartDetail = listProductDescription;
                            break;
                        case "tabSpec":
                            filePartName = "_ListPartSpec";
                            listProductSpec = new GetProductSpecList().Get(Stkcode);
                            ViewBag.ListPartDetail = listProductSpec;
                            break;
                        case "tabImage":
                            filePartName = "_ListPartImage";
                            listProductImage = new GetProductImageList().Get(Stkcode);
                            ViewBag.ListPartDetail = listProductImage;
                            break;
                        case "tabOem":
                            filePartName = "_ListPartOem";
                            listProductOem = new GetProductOemList().Get(Stkcode);
                            ViewBag.ListPartDetail = listProductOem;
                            break;  
                        case "tabLinkage":
                            filePartName = "_ListPartLinkage";
                            listProductLinkage = new GetLinkageDataList().Get(Stkcode);
                            ViewBag.ListPartDetail = listProductLinkage;
                            break;
                        default:
                            filePartName = "";
                            break;
                    }
                }

                ViewBag.ListProductApiCount = new List<object>();
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ViewBag.ListProductApiCount = new List<object>();
                ViewBag.ListPartDetail = new List<object>();

            }
            ViewBag.Message = message;
            return PartialView(filePartName, new
            {
                @ViewBag.Message,
                @ViewBag.ListProductApiCount,
                @ViewBag.ListPartDetail
            });
        }
        public JsonResult getStockGroup(string prodCode, string company)
        {
            List<stockGroupList> stockGroupList = new List<stockGroupList>();

            var connectionString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            Connection.Open();

            var command = new SqlCommand("P_Search_StockGroup", Connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inProdCode", prodCode);
            command.Parameters.AddWithValue("@inCompany", company);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                stockGroupList.Add(new stockGroupList()
                {
                    STKGRP = dr["STKGRP"].ToString(),
                    GRPNAM = dr["GRPNAM"].ToString(),
                });
            }
            dr.Close();
            dr.Dispose();

            Connection.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(stockGroupList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getStockCode(string txtSearch, string company, string stockGroup, string prodCode)
        {
            string CUSCOD = string.Empty;
            List<string> StockCode = new List<string>();

            var connectionString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Search_Stockcode_By_Stockgroup", Connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@inTxtSearch", txtSearch);
            command.Parameters.AddWithValue("@inCompany", company);
            command.Parameters.AddWithValue("@inStockGroup", stockGroup);
            command.Parameters.AddWithValue("@inProdCode", prodCode);
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                StockCode.Add(dr.GetString(1));
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(StockCode, JsonRequestBehavior.AllowGet);
        }
        public class stockGroupList
        {
            public string STKGRP { get; set; }
            public string GRPNAM { get; set; }
        }
    }
}