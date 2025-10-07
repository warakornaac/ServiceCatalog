using ServiceCatalog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;

namespace ServiceCatalog.Controllers
{
    public class UpLoadDataVIOController : Controller
    {
        // GET: UpLoadDataVIO
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewVIO()
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
        [HttpPost]
        public ActionResult Excel(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, Path.GetFileName(file.FileName));
                file.SaveAs(filePath);


                try
                {
                    ImportStoreRun();
                    return Json(new { success = true, message = "File uploaded and processed successfully." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }
            return Json(new { success = false, message = "No file received." });
        }

        //[HttpPost]
        //public async Task<ActionResult> ExcelFile(HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        if (file != null && file.ContentLength > 0)
        //        {
        //            string folderPath = Server.MapPath("~/UploadedFiles/");
        //            string filePath = Path.Combine(folderPath, Path.GetFileName(file.FileName));

        //            if (!Directory.Exists(folderPath))
        //            {
        //                Directory.CreateDirectory(folderPath);
        //            }

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await file.InputStream.CopyToAsync(stream);
        //            }

        //            DataTable dt = ReadExcelToDataTable(filePath);

        //            SaveDataTableToSql(dt, "VIO_PrepairData");

        //            var result = await ImportStoreRun();
        //            var vehicles = result.Item1;
        //            var outResult = result.Item2;
        //            return Json(new { success = true, message = "อัพโหลดสำเร็จ", data = vehicles, outResult = outResult });
        //        }
        //        return Json(new { success = false, message = "ไม่พบไฟล์" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        [HttpPost]
        public async Task<ActionResult> ExcelFile(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    string folderPath = Server.MapPath("~/UploadedFiles/");
                    string filePath = Path.Combine(folderPath, Path.GetFileName(file.FileName));

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.InputStream.CopyToAsync(stream);
                    }


                    if (!System.IO.File.Exists(filePath))
                        throw new Exception("ไม่พบไฟล์ Excel ที่อัปโหลด");

                    if (Path.GetExtension(filePath).ToLower() != ".xlsx")
                        throw new Exception("รองรับเฉพาะไฟล์ .xlsx เท่านั้น");


                    DataTable dt = ReadExcelToDataTable(filePath);


                    SaveDataTableToSql(dt, "VIO_PrepairData");

                    var result = await ImportStoreRun();
                    var vehicles = result.Item1;
                    var outResult = result.Item2;

                    return Json(new { success = true, message = "อัปโหลดสำเร็จ", data = vehicles, outResult });
                }

                return Json(new { success = false, message = "ไม่พบไฟล์สำหรับอัปโหลด" });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "เกิดข้อผิดพลาด: " + ex.Message });
            }
        }



        ////Add Data From Excel To DataTable
        //private DataTable ReadExcelToDataTable(string path)
        //{
        //    var dt = new DataTable();

        //    using (var workbook = new XLWorkbook(path))
        //    {
        //        var worksheet = workbook.Worksheet("MasterData");
        //        if (worksheet == null)
        //            throw new Exception("ไม่พบชีทชื่อ 'MasterData' ในไฟล์ Excel");

        //        bool hasHeader = true;
        //        var firstRow = worksheet.FirstRowUsed();
        //        var columnCount = firstRow.CellCount();

        //        // สร้างคอลัมน์
        //        foreach (var cell in firstRow.Cells())
        //        {
        //            string colName = hasHeader ? cell.GetString() : $"Column {cell.Address.ColumnNumber}";
        //            dt.Columns.Add(colName, typeof(string));
        //        }

        //        // เริ่มอ่านจากแถวที่ 2 ถ้ามี Header
        //        int startRow = hasHeader ? 2 : 1;

        //        foreach (var row in worksheet.RowsUsed().Skip(startRow - 1))
        //        {
        //            var dataRow = dt.NewRow();

        //            for (int i = 0; i < columnCount; i++)
        //            {
        //                string header = dt.Columns[i].ColumnName;
        //                string cellValue = row.Cell(i + 1).GetString();

        //                if (cellValue == "NULL" || cellValue.Contains("(blank"))
        //                {
        //                    dataRow[i] = DBNull.Value;
        //                }
        //                else if (header.Equals("strokes", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    if (decimal.TryParse(cellValue, out decimal value))
        //                        dataRow[i] = value.ToString("F1");
        //                    else
        //                        dataRow[i] = DBNull.Value;
        //                }
        //                else
        //                {
        //                    dataRow[i] = cellValue;
        //                }
        //            }

        //            dt.Rows.Add(dataRow);
        //        }
        //    }

        //    return dt;
        //}
        private DataTable ReadExcelToDataTable(string path)
        {
            var dt = new DataTable();

            using (var workbook = new XLWorkbook(path))
            {
                var worksheet = workbook.Worksheet("MasterData");
                if (worksheet == null)
                    throw new Exception("ไม่พบชีทชื่อ 'MasterData' ในไฟล์ Excel");

                bool hasHeader = true;
                var firstRow = worksheet.FirstRowUsed();
                var columnCount = firstRow.LastCellUsed().Address.ColumnNumber; // ✅ ใช้ LastCellUsed เพื่อป้องกันคอลัมน์หาย

                // ✅ สร้างคอลัมน์ให้ครบตามจำนวนจริง
                for (int i = 1; i <= columnCount; i++)
                {
                    string colName = hasHeader ? firstRow.Cell(i).GetString().Trim() : $"Column {i}";
                    if (string.IsNullOrEmpty(colName))
                        colName = $"Column{i}";
                    if (!dt.Columns.Contains(colName))
                        dt.Columns.Add(colName, typeof(string));
                }

                // ✅ เริ่มอ่านจากแถวที่ 2 ถ้ามี Header
                int startRow = hasHeader ? 2 : 1;

                foreach (var row in worksheet.RowsUsed().Skip(startRow - 1))
                {
                    var dataRow = dt.NewRow();

                    int lastCell = row.LastCellUsed()?.Address.ColumnNumber ?? 0;
                    for (int i = 0; i < lastCell; i++)
                    {

                        if (i >= dt.Columns.Count)
                        {
                            dt.Columns.Add($"ExtraColumn{i + 1}", typeof(string));
                        }

                        string header = dt.Columns[i].ColumnName;
                        string cellValue = row.Cell(i + 1).GetString().Trim();

                        if (string.IsNullOrEmpty(cellValue) || cellValue == "NULL" || cellValue.Contains("(blank"))
                        {
                            dataRow[i] = DBNull.Value;
                        }
                        else if (header.Equals("strokes", StringComparison.OrdinalIgnoreCase))
                        {
                            if (decimal.TryParse(cellValue, out decimal value))
                                dataRow[i] = value.ToString("F1");
                            else
                                dataRow[i] = DBNull.Value;
                        }
                        else
                        {
                            dataRow[i] = cellValue;
                        }
                    }

                    dt.Rows.Add(dataRow);
                }
            }

            return dt;
        }


        //Create Table And Truncate
        private void SaveDataTableToSql(DataTable dt, string tableName)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString))
            {
                conn.Open();
                var columnDefs = new List<string>();

                foreach (DataColumn col in dt.Columns)
                {
                    string sqlType = InferSqlType(col, dt);
                    columnDefs.Add($"[{col.ColumnName}] {sqlType}");
                }

                // เพิ่ม flag column แบบ default เป็น varchar 
                columnDefs.Add("[flag] VARCHAR(1) DEFAULT 0");

                string dropAndCreate = $@"
            IF OBJECT_ID('{tableName}', 'U') IS NOT NULL DROP TABLE {tableName};

            CREATE TABLE {tableName} (
                {string.Join(",", columnDefs)}
            );";
                using (SqlCommand cmd = new SqlCommand(dropAndCreate, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                if (!dt.Columns.Contains("flag"))
                {
                    dt.Columns.Add("flag", typeof(int));
                    foreach (DataRow row in dt.Rows)
                    {
                        row["flag"] = 0;
                    }
                }

                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    bulk.DestinationTableName = tableName;
                    bulk.WriteToServer(dt);
                }
            }
        }

        //Column Type SQL
        private string InferSqlType(DataColumn column, DataTable dt)
        {

            if (column.DataType == typeof(int)) return "INT";
            if (column.DataType == typeof(double) || column.DataType == typeof(float) || column.DataType == typeof(decimal))
                return "DECIMAL(18,2)";
            if (column.DataType == typeof(DateTime)) return "DATETIME";


            if (column.DataType == typeof(string))
            {
                int maxLen = dt.AsEnumerable()
                               .Where(r => !r.IsNull(column))
                               .Select(r => r[column].ToString().Length)
                               .DefaultIfEmpty(1)
                               .Max();

                if (maxLen < 50) return $"NVARCHAR(100)";
                if (maxLen < 255) return $"NVARCHAR(255)";
                if (maxLen < 2000) return $"NVARCHAR(2000)";
                return "NVARCHAR(MAX)";
            }


            return "NVARCHAR(MAX)";
        }

        private async Task<Tuple<List<VehicleInfo>, string>> ImportStoreRun()
        {
            List<VehicleInfo> list = new List<VehicleInfo>();
            string result = string.Empty;

            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(conString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("P_VIOInsertUpdateExcel", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter returnValue = new SqlParameter("@outResult", SqlDbType.NVarChar, 100);
                    returnValue.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(returnValue);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new VehicleInfo
                            {
                                KType = reader["KType"]?.ToString() ?? string.Empty,
                                MarketSegment = reader["MarketSegment"]?.ToString() ?? string.Empty,
                                VehicleSegment = reader["VehicleSegment"]?.ToString() ?? string.Empty,
                                Maker = reader["Maker"]?.ToString() ?? string.Empty,
                                ModelRange = reader["ModelRange"]?.ToString() ?? string.Empty,
                                Model = reader["Model"]?.ToString() ?? string.Empty,
                                Body = reader["Body"]?.ToString() ?? string.Empty,
                                BodyType = reader["BodyType"]?.ToString() ?? string.Empty,
                                DriveType = reader["DriveType"]?.ToString() ?? string.Empty,
                                EngineType = reader["EngineType"]?.ToString() ?? string.Empty,
                                Strokes = reader["Strokes"]?.ToString() ?? string.Empty,
                                FuelType = reader["FuelType"]?.ToString() ?? string.Empty,
                                YearFrom = reader["YearFrom"]?.ToString() ?? string.Empty,
                                YearTo = reader["YearTo"]?.ToString() ?? string.Empty,
                                ThailandVIO = reader["ThailandVIO"]?.ToString() ?? string.Empty,
                                Flag = reader["Flag"]?.ToString() ?? string.Empty
                            });
                        }
                    }

                    result = returnValue.Value?.ToString() ?? string.Empty;
                }
            }

            return Tuple.Create(list, result);
        }


        //End Excel

        //start seletor
        public JsonResult GetVIO_VehicleSeg()
        {
            string message = string.Empty;
            List<VIO_VehicleSegment> listVehicelSeg = new List<VIO_VehicleSegment>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
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
                    SqlCommand cmd = new SqlCommand("P_SearchVIO_Selector", conn);
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
                return Json(new { respone = true, message = true, result = list }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new { respone = false, message = true, result = list }, JsonRequestBehavior.AllowGet);

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
                    SqlCommand cmd = new SqlCommand("P_SearchVIO_Selector", conn);
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
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO_Selector", conn))
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
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO_Selector", conn))
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
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO_Selector", conn))
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
                    using (SqlCommand cmd = new SqlCommand("P_SearchVIO_Selector", conn))
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


        //end selector

        //start table
        public JsonResult GetVIOData(string marketID, string vehicleID, string maker, string rangID, string modelID, string bodyID, string engineID)
        {
            string message = string.Empty;
            List<VIO_DATA> list = new List<VIO_DATA>();
            string conString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("P_Search_VIO_view", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@inmarketseID", marketID);
                        cmd.Parameters.AddWithValue("@invehicleseID", vehicleID);
                        cmd.Parameters.AddWithValue("@inmakerID", maker);
                        cmd.Parameters.AddWithValue("@inmodelrangeID", rangID);
                        cmd.Parameters.AddWithValue("@inmodelID", modelID);
                        cmd.Parameters.AddWithValue("@inBodyID", bodyID);
                        cmd.Parameters.AddWithValue("@inEngineID", engineID);

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(new VIO_DATA
                            {
                                KType = reader["KType"] != DBNull.Value ? reader["KType"].ToString() : string.Empty,
                                MarketSegment = reader["MarketSegment"] != DBNull.Value ? reader["MarketSegment"].ToString() : string.Empty,
                                VehicleSegment = reader["VehicleSegment"] != DBNull.Value ? reader["VehicleSegment"].ToString() : string.Empty,
                                Maker = reader["Maker"] != DBNull.Value ? reader["Maker"].ToString() : string.Empty,
                                ModelRange = reader["ModelRange"] != DBNull.Value ? reader["ModelRange"].ToString() : string.Empty,
                                Model = reader["Model"] != DBNull.Value ? reader["Model"].ToString() : string.Empty,
                                Body = reader["Body"] != DBNull.Value ? reader["Body"].ToString() : string.Empty,
                                BodyType = reader["BodyType"] != DBNull.Value ? reader["BodyType"].ToString() : string.Empty,
                                DriveType = reader["DriveType"] != DBNull.Value ? reader["DriveType"].ToString() : string.Empty,
                                EngineType = reader["EngineType"] != DBNull.Value ? reader["EngineType"].ToString() : string.Empty,
                                Strokes = reader["Strokes"] != DBNull.Value ? reader["Strokes"].ToString() : string.Empty,
                                FuelType = reader["FuelType"] != DBNull.Value ? reader["FuelType"].ToString() : string.Empty,
                                YearFrom = reader["YearFrom"] != DBNull.Value ? reader["YearFrom"].ToString() : string.Empty,
                                YearTo = reader["YearTo"] != DBNull.Value ? reader["YearTo"].ToString() : string.Empty,
                                ThailandVIO = reader["ThaiVIO"] != DBNull.Value ? reader["ThaiVIO"].ToString() : string.Empty,
                                TruType = reader["TruType"] != DBNull.Value ? reader["TruType"].ToString() : string.Empty,

                            });
                        }
                    }
                }
                return Json(new { respone = true, message = message, result = list }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Json(new { respone = false, message = message, result = list }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}