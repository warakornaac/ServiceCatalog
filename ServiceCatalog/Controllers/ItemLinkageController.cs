using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceCatalog.Models;
using ServiceCatalog.Data;
using System.Web.Services.Description;
using System.Windows.Documents;

namespace ServiceCatalog.Controllers
{
    public class ItemLinkageController : Controller
    {
        // GET: ItemLinkage
        public ActionResult Index()
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
            return View();
        }

        public JsonResult GetPartNoDes(string TxtSearch)
        {
            string message = string.Empty;
            List<string> parts = new List<string>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("P_Get_PartNo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inTxtSearch", TxtSearch);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            parts.Add(reader.GetString(1));
                        }
                    }

                }
            }
            return Json(parts, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDetailPN(string STKCOD)
        {
            var DetailPN = new List<PNDescripsModels>();
            DetailPN = new SearchPNDetail().SearchPN(STKCOD);
            @ViewBag.DetailPN = DetailPN;
            return PartialView("_PNDetail", new
            {
                @ViewBag.DetailPN
            });
        }

        public ActionResult GetLinkageData(string STKCOD)
        {
            var ListLinkage = new List<StoreGetLinkageDataModel>();
            ListLinkage = new GetLinkage().GetLinkageData(STKCOD);
            @ViewBag.ListLinkage = ListLinkage;
            return PartialView("_LinkageData", new
            {
                @ViewBag.ListLinkage
            });
        }

        public ActionResult GetVehicleSelector(string STKCOD)
        {
            List<VIO_MarketSegment> listMarketSeg = new List<VIO_MarketSegment>();
            List<VIO_VehicleSegment> listVehicelSeg = new List<VIO_VehicleSegment>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO_Selector", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inModule", "1");
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            listMarketSeg.Add(new VIO_MarketSegment()
                            {
                                ID = reader["ID"] != DBNull.Value ? reader["ID"].ToString() : string.Empty,
                                MarketSegment = reader["MarketSegment"] != DBNull.Value ? reader["MarketSegment"].ToString() : string.Empty
                            });
                        }
                    }
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO_Selector", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inModule", "2");
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            listVehicelSeg.Add(new VIO_VehicleSegment()
                            {
                                ID = reader["ID"] != DBNull.Value ? reader["ID"].ToString() : string.Empty,
                                VehicleSegment = reader["VehicleSegment"] != DBNull.Value ? reader["VehicleSegment"].ToString() : string.Empty
                            });
                        }
                    }
                    @ViewBag.MarketSeg = listMarketSeg;
                    @ViewBag.VehicleSeg = listVehicelSeg;
                    @ViewBag.STKCOD = STKCOD;
                }
            }
            catch (Exception ex)
            {
                @ViewBag.Exception = ex;
            }
            return PartialView("_VehicleSelector", new
            {
                @ViewBag.STKCOD,
                @ViewBag.MarketSeg,
                @ViewBag.VehicleSeg,
                @ViewBag.Exception
            });
        }

        public JsonResult SearchVehicleLinkage(string STKCOD, string marketID, string vehicleID, string maker, string rangID, string modelID, string bodyID, string engineID)
        {
            string message = string.Empty;
            List<StoreSearchVehicleLinkageModel> list = new List<StoreSearchVehicleLinkageModel>();
            try
            {
                list = new SearchLinkageVehicle().SearchVehicle(STKCOD, marketID, vehicleID, maker, rangID, modelID, bodyID, engineID);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { message = message, result = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertLinkage(string STKCOD, string KTYPE, string TRUTYPE, string MARKETSEG)
        {
            string message = string.Empty;
            string res = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Add_LinkageData_dev", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inSTKCOD", STKCOD);
                        cmd.Parameters.AddWithValue("@inKtype", KTYPE);
                        cmd.Parameters.AddWithValue("@inTruType", TRUTYPE);
                        cmd.Parameters.AddWithValue("@inMarketSeg", MARKETSEG);
                        cmd.Parameters.AddWithValue("@inUser", "thiraphon.pra");

                        cmd.ExecuteNonQuery();
                    }
                }
                res = "Y";
            }
            catch (Exception ex) { message = ex.Message; res = "N"; }


            return Json(new { message = message, result = res }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertLinkageFilter(string STKCOD, string MARKET, string VEHICLE, string MAKER, string RANGE, string MODEL, string BODY, string ENGINE)
        {
            string message = string.Empty;
            string res = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Add_LinkageData_Filter", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inSTKCOD", STKCOD);
                        cmd.Parameters.AddWithValue("@inMarketSeg", MARKET);
                        cmd.Parameters.AddWithValue("@inVehicleSeg", VEHICLE);
                        cmd.Parameters.AddWithValue("@inMaker", MAKER);
                        cmd.Parameters.AddWithValue("@inRange", RANGE);
                        cmd.Parameters.AddWithValue("@inModel", MODEL);
                        cmd.Parameters.AddWithValue("@inBody", BODY);
                        cmd.Parameters.AddWithValue("@inEngine", ENGINE);
                        cmd.Parameters.AddWithValue("@inUser", "thiraphon.pra");

                        cmd.ExecuteNonQuery();
                    }
                }
                res = "Y";
            }
            catch (Exception ex) { message = ex.Message; res = "N"; }


            return Json(new { message = message, result = res }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteLinkage(string STKCOD, string KTYPE, string TRUTYPE, string SEQLINK)
        {
            string message = string.Empty;
            string res = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Delete_LinkageData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inSTKCOD", STKCOD);
                        cmd.Parameters.AddWithValue("@inKtype", KTYPE);
                        cmd.Parameters.AddWithValue("@inTruType", TRUTYPE);
                        cmd.Parameters.AddWithValue("@inSeq", SEQLINK);
                        cmd.Parameters.AddWithValue("@inUser", "thiraphon.pra");

                        cmd.ExecuteNonQuery();
                    }
                }
                res = "Y";
            }
            catch (Exception ex) { message = ex.Message; res = "N"; }


            return Json(new { message = message, result = res }, JsonRequestBehavior.AllowGet);
        }

    }
}