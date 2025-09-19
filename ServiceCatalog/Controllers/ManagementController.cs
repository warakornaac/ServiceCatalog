using ServiceCatalog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceCatalog.Controllers
{
    public class ManagementController : Controller
    {
        // GET: Management
        public ActionResult Index()
        {
            List<VIO_MarketSegment> listMarketSeg = new List<VIO_MarketSegment>();
            List<VIO_VehicleSegment> listVehicelSeg = new List<VIO_VehicleSegment>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO", conn))
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
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO", conn))
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
                    ViewBag.MarketSeg = listMarketSeg;
                    ViewBag.VehicleSeg = listVehicelSeg;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex;
            }
            return View();
        }
        public JsonResult GetVIO_MarketSeg()
        {
            string message = string.Empty;
            List<VIO_MarketSegment> list = new List<VIO_MarketSegment>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inModule", "1");
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(new VIO_MarketSegment()
                            {
                                ID = reader["ID"] != DBNull.Value ? reader["ID"].ToString() : string.Empty,
                                MarketSegment = reader["MarketSegment"] != DBNull.Value ? reader["MarketSegment"].ToString() : string.Empty
                            });
                        }
                    }
                }
                return Json(new { respone = true, message = message, result = list }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { respone = false, message = e.Message, result = list }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetVIO_VehicleSeg()
        {
            string message = string.Empty;
            List<VIO_VehicleSegment> listVehicelSeg = new List<VIO_VehicleSegment>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO", conn))
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
                }
                return Json(new { respone = true, message = message, result = listVehicelSeg }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { respone = false, message = e.Message, result = listVehicelSeg }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetVIO_Maker()
        {
            List<VIO_Maker> list = new List<VIO_Maker>();
            string message = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("P_SearchVIO", conn);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inModule", "3");

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new VIO_Maker()
                        {
                            ID = reader["ID"] != DBNull.Value ? reader["ID"].ToString() : "",
                            Maker = reader["Maker"] != DBNull.Value ? reader["Maker"].ToString() : ""
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                }
                return Json(new { respone = true, message = message, result = list }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new { respone = false, message = message, result = list }, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GetVIO_Model(string MarketSegID, string MakerID, string ModelRangID)
        {
            List<VIO_Model> list = new List<VIO_Model>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("P_SearchVIO", conn);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@inModule", "4");
                    cmd.Parameters.AddWithValue("@inmarketseID", MarketSegID);
                    cmd.Parameters.AddWithValue("@inmakerID", MakerID);
                    cmd.Parameters.AddWithValue("@inmodelrangeID", ModelRangID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new VIO_Model()
                        {
                            ID = reader["ID"] != DBNull.Value ? reader["ID"].ToString() : "",
                            Model = reader["Model"] != DBNull.Value ? reader["Model"].ToString() : "",
                            Maker_ID = reader["Maker_ID"] != DBNull.Value ? reader["Maker_ID"].ToString() : "",
                            ModelRange_ID = reader["ModelRange_ID"] != DBNull.Value ? reader["ModelRange_ID"].ToString() : "",
                            MarketSegment_ID = reader["MarketSegment_ID"] != DBNull.Value ? reader["MarketSegment_ID"].ToString() : ""
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                }
                return Json(new { respone = true, result = list }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { respone = false, result = list }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetVIO_ModelRange(string MakerID)
        {
            string message = string.Empty;
            List<VIO_ModelRange> list = new List<VIO_ModelRange>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inModule", "5");
                        cmd.Parameters.AddWithValue("@inmakerID", MakerID);

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(new VIO_ModelRange()
                            {
                                ID = reader["ID"] != DBNull.Value ? reader["ID"].ToString() : "",
                                ModelRange = reader["ModelRange"] != DBNull.Value ? reader["ModelRange"].ToString() : "",
                                Maker_ID = reader["Maker_ID"] != DBNull.Value ? reader["Maker_ID"].ToString() : ""
                            });
                        }
                    }
                }
                return Json(new { respone = true, message = message, result = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                list = null;
                return Json(new { respone = false, message = message, result = list }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetVIO_Body(string marketSeg, string vehicleSeg, string makerID, string modelID, string modelRangeID)
        {
            string message = string.Empty;
            List<VIO_Body> list = new List<VIO_Body>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inModule", "6");
                        cmd.Parameters.AddWithValue("@inmarketseID", marketSeg);
                        cmd.Parameters.AddWithValue("@invehiclesegID", vehicleSeg);
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", modelRangeID);

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(new VIO_Body()
                            {
                                ID = reader["ID"] != DBNull.Value ? reader["ID"].ToString() : string.Empty,
                                Body = reader["Body"] != DBNull.Value ? reader["Body"].ToString() : string.Empty,
                                BodyType = reader["BodyType"] != DBNull.Value ? reader["BodyType"].ToString() : string.Empty,
                                Maker_ID = reader["Maker_ID"] != DBNull.Value ? reader["Maker_ID"].ToString() : string.Empty,
                                ModelRange_ID = reader["ModelRange_ID"] != DBNull.Value ? reader["ModelRange_ID"].ToString() : string.Empty,
                                Model_ID = reader["Model_ID"] != DBNull.Value ? reader["Model_ID"].ToString() : string.Empty,
                                VehicleSegment_ID = reader["VehicleSegment_ID"] != DBNull.Value ? reader["VehicleSegment_ID"].ToString() : string.Empty,
                                MarketSegment_ID = reader["MarketSegment_ID"] != DBNull.Value ? reader["MarketSegment_ID"].ToString() : string.Empty,

                            });
                        }
                        return Json(new { respone = true, message = message, result = list }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message, result = list }, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GETVIO_Engine(string makerID, string modelRangeID, string modelID, string bodyID)
        {
            string message = string.Empty;
            List<VIO_Engine> list = new List<VIO_Engine>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inModule", "7");
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", modelRangeID);
                        cmd.Parameters.AddWithValue("@inBodyID", bodyID);

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(new VIO_Engine()
                            {
                                ID = reader["ID"] != DBNull.Value ? reader["ID"].ToString() : string.Empty,
                                EngineType = reader["EngineType"] != DBNull.Value ? reader["EngineType"].ToString() : string.Empty,
                                FuelType = reader["FuelType"] != DBNull.Value ? reader["FuelType"].ToString() : string.Empty,
                                Strokes = reader["Strokes"] != DBNull.Value ? reader["Strokes"].ToString() : string.Empty,
                                Maker_ID = reader["Maker_ID"] != DBNull.Value ? reader["Maker_ID"].ToString() : string.Empty,
                                ModelRange_ID = reader["ModelRange_ID"] != DBNull.Value ? reader["ModelRange_ID"].ToString() : string.Empty,
                                Model_ID = reader["Model_ID"] != DBNull.Value ? reader["Model_ID"].ToString() : string.Empty,
                                Body_ID = reader["Body_ID"] != DBNull.Value ? reader["Body_ID"].ToString() : string.Empty
                            });
                        }
                    }
                    return Json(new { respone = true, message = message, result = list }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message, result = list }, JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult GetVIO_TruData(string marketSegID, string vehicleSegID, string makerID, string modelRangeID, string modelID, string bodyID, string engineID)
        {
            string message = string.Empty;
            List<VIO_TruData> list = new List<VIO_TruData>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inModule", "8");
                        cmd.Parameters.AddWithValue("@inmarketseID", marketSegID);
                        cmd.Parameters.AddWithValue("@invehiclesegID", vehicleSegID);
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", modelRangeID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inBodyID", bodyID);
                        cmd.Parameters.AddWithValue("@inEngineID", engineID);

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(new VIO_TruData()
                            {
                                MarketSegment_ID = reader["MarketSegment_ID"] != DBNull.Value ? reader["MarketSegment_ID"].ToString() : string.Empty,
                                VehicleSegment_ID = reader["VehicleSegment_ID"] != DBNull.Value ? reader["VehicleSegment_ID"].ToString() : string.Empty,
                                Maker_ID = reader["Maker_ID"] != DBNull.Value ? reader["Maker_ID"].ToString() : string.Empty,
                                ModelRange_ID = reader["ModelRange_ID"] != DBNull.Value ? reader["ModelRange_ID"].ToString() : string.Empty,
                                Model_ID = reader["Model_ID"] != DBNull.Value ? reader["Model_ID"].ToString() : string.Empty,
                                Body_ID = reader["Body_ID"] != DBNull.Value ? reader["Body_ID"].ToString() : string.Empty,
                                Engine_ID = reader["Engine_ID"] != DBNull.Value ? reader["Engine_ID"].ToString() : string.Empty,
                                KType = reader["KType"] != DBNull.Value ? reader["KType"].ToString() : string.Empty,
                                DriveType = reader["DriveType"] != DBNull.Value ? reader["DriveType"].ToString() : string.Empty,
                                YearFrom = reader["YearFrom"] != DBNull.Value ? reader["YearFrom"].ToString() : string.Empty,
                                YearTo = reader["YearTo"] != DBNull.Value ? reader["YearTo"].ToString() : string.Empty,
                                ThaiVIO = reader["ThaiVIO"] != DBNull.Value ? reader["ThaiVIO"].ToString() : string.Empty,
                                TruType = reader["TruType"] != DBNull.Value ? reader["TruType"].ToString() : string.Empty,
                            });
                        }
                    }
                }//conn
                return Json(new { respone = true, message = message, result = list }, JsonRequestBehavior.AllowGet);
            }//end try
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message, result = list }, JsonRequestBehavior.AllowGet);
            }


        }

        //// Input Func
        //VIO Main text
        public JsonResult InsertVIOText(string moduleID, string marketID, string makerID, string rangeID, string val)
        {
            string message = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Insert_VIO_MarketSegment_VehicleSegment_Maker_ModelRange_Model", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inModule", moduleID);
                        cmd.Parameters.AddWithValue("@inmarketseID", marketID);
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", rangeID);
                        cmd.Parameters.AddWithValue("@inVal", val);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Json(new { respone = true, message = message });
            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message });
            }

        }
        public JsonResult UpdateVIOText(string moduleID, string marketID, string oldMarket, string vehicleID, string oldVehicle, string makerID, string oldMaker, string rangeID, string oldRange, string modelID, string oldModel, string val)
        {
            string message = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Update_VIO_MarketSegment_VehicleSegment_Maker_ModelRange_Model", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inModule", moduleID);
                        cmd.Parameters.AddWithValue("@inmarketseID", marketID);
                        cmd.Parameters.AddWithValue("@invehicleseID", vehicleID);
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", rangeID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inVal", val);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Json(new { respone = true, message = message });
            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message });
            }

        }
        //VIO BODY
        public JsonResult InsertVIOBody(string marketID, string vehicleID, string makerID, string rangeID, string modelID, string bodyCode, string bodyType)
        {
            string message = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Insert_VIOBody", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inmarketseID", marketID);
                        cmd.Parameters.AddWithValue("@invehicleseID", vehicleID);
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", rangeID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inBodyCode", bodyCode);
                        cmd.Parameters.AddWithValue("@inBodyType", bodyType);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Json(new { respone = true, message = message });
            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message });

            }
        }

        public JsonResult UpdateVIOBody(string marketID, string vehicleID, string makerID, string rangeID, string modelID, string bodyID, string bodyCode, string bodyType)
        {
            string message = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Update_VIOBody", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inmarketseID", marketID);
                        cmd.Parameters.AddWithValue("@invehicleseID", vehicleID);
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", rangeID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inBodyID", bodyID);
                        cmd.Parameters.AddWithValue("@inBodyCode", bodyCode);
                        cmd.Parameters.AddWithValue("@inBodyType", bodyType);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Json(new { respone = true, message = message });
            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message });

            }
        }
        //VIO Engine
        public JsonResult InsertVIOEngine(string makerID, string rangeID, string modelID, string bodyID, string valEngine, string valFuel, string valStrokes)
        {
            string message = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Insert_VIOEngine", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", rangeID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inBodyID", bodyID);
                        cmd.Parameters.AddWithValue("@inEngine", valEngine);
                        cmd.Parameters.AddWithValue("@inFuelType", valFuel);
                        cmd.Parameters.AddWithValue("@inStrokes", valStrokes);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Json(new { respone = true, message = message });
            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message });

            }
        }

        public JsonResult UpdateVIOEngine(string makerID, string rangeID, string modelID, string bodyID, string engID, string valEngine, string valFuel, string valStrokes)
        {
            string message = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Update_VIOEngine", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", rangeID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inBodyID", bodyID);
                        cmd.Parameters.AddWithValue("@inEngineID", engID);
                        cmd.Parameters.AddWithValue("@inEngine", valEngine);
                        cmd.Parameters.AddWithValue("@inFuelType", valFuel);
                        cmd.Parameters.AddWithValue("@inStrokes", valStrokes);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Json(new { respone = true, message = message });
            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message });

            }
        }




        //VIO TruData
        public JsonResult InsertVIOTrudata(string marketSegID, string vehicleSegID, string makerID, string rangeID, string modelID, string bodyID, string engineID, string valKtype, string valDrive, string valYearF, string valYearT, string valTHvio)
        {
            string message = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Insert_VIOTruData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inmarketseID", marketSegID);
                        cmd.Parameters.AddWithValue("@invehicleseID", vehicleSegID);
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", rangeID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inbodyID", bodyID);
                        cmd.Parameters.AddWithValue("@inengineID", engineID);
                        cmd.Parameters.AddWithValue("@invalKtype", valKtype);
                        cmd.Parameters.AddWithValue("@invalDrive", valDrive);
                        cmd.Parameters.AddWithValue("@invalYearF", valYearF);
                        cmd.Parameters.AddWithValue("@invalYearT", valYearT);
                        cmd.Parameters.AddWithValue("@invalTHVIO", valTHvio);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Json(new { respone = true, message = message });

            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message });
            }

        }

        public JsonResult UpdateVIOTrudata(string marketSegID, string vehicleSegID, string makerID, string rangeID, string modelID, string bodyID, string engineID, string valKtype, string valDrive, string valYearF, string valYearT, string valTHvio)
        {
            string message = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Update_VIOTruData", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inmarketseID", marketSegID);
                        cmd.Parameters.AddWithValue("@invehicleseID", vehicleSegID);
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", rangeID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inbodyID", bodyID);
                        cmd.Parameters.AddWithValue("@inengineID", engineID);
                        cmd.Parameters.AddWithValue("@invalKtype", valKtype);
                        cmd.Parameters.AddWithValue("@invalDrive", valDrive);
                        cmd.Parameters.AddWithValue("@invalYearF", valYearF);
                        cmd.Parameters.AddWithValue("@invalYearT", valYearT);
                        cmd.Parameters.AddWithValue("@invalTHVIO", valTHvio);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Json(new { respone = true, message = message });

            }
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message });
            }

        }



        //delete VIO
        public JsonResult DeleteVIO_TruData(string moduleID, string marketSegID, string vehicleSegID, string makerID, string modelRangeID, string modelID, string bodyID, string engineID)
        {
            string message = string.Empty;
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Delete_VIO", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inModule", moduleID);
                        cmd.Parameters.AddWithValue("@inmarketseID", marketSegID);
                        cmd.Parameters.AddWithValue("@invehiclesegID", vehicleSegID);
                        cmd.Parameters.AddWithValue("@inmakerID", makerID);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", modelRangeID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inBodyID", bodyID);
                        cmd.Parameters.AddWithValue("@inEngineID", engineID);

                        cmd.ExecuteNonQuery();

                    }
                }//conn
                return Json(new { respone = true, message = message }, JsonRequestBehavior.AllowGet);
            }//end try
            catch (Exception e)
            {
                message = e.Message;
                return Json(new { respone = false, message = message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}