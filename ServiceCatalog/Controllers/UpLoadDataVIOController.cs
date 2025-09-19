using ServiceCatalog.Models;
using OfficeOpenXml;
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
            string conString = ConfigurationManager.ConnectionStrings["VIO_Connectionstring"].ConnectionString;
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
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.InputStream.CopyToAsync(stream);
                    }

                    DataTable dt = ReadExcelToDataTable(filePath);

                    SaveDataTableToSql(dt, "Test260826");

                    var vehicles = await ImportStoreRun();

                    return Json(new { success = true, message = "อัพโหลดสำเร็จ", data = vehicles });
                }
                return Json(new { success = false, message = "ไม่พบไฟล์" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        //Add Data From Excel To DataTable
        private DataTable ReadExcelToDataTable(string path)
        {
            var dt = new DataTable();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var worksheet = package.Workbook.Worksheets["Sheet1"];
                if (worksheet == null)
                    throw new Exception("ไม่พบชีทชื่อ 'K-Type Data' ในไฟล์ Excel");

                bool hasHeader = true;
                foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                {
                    dt.Columns.Add(hasHeader ? firstRowCell.Text : $"Column {firstRowCell.Start.Column}", typeof(string));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                {
                    var wsRow = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
                    DataRow row = dt.NewRow();

                    foreach (var cell in wsRow)
                    {
                        string header = dt.Columns[cell.Start.Column - 1].ColumnName;

                        if (cell.Text == "NULL" || cell.Text.Contains("(blank"))
                        {
                            row[cell.Start.Column - 1] = null;
                        }
                        else if (header.Equals("strokes", StringComparison.OrdinalIgnoreCase))
                        {
                            if (decimal.TryParse(cell.Text, out decimal value))
                            {

                                row[cell.Start.Column - 1] = value.ToString("F1");
                            }
                            else
                            {
                                row[cell.Start.Column - 1] = null;
                            }
                        }
                        else
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }


        //Create Table And Truncate
        private void SaveDataTableToSql(DataTable dt, string tableName)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["VIO_Connectionstring"].ConnectionString))
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
                        row["flag"] = 0; // หรือค่า default อื่น ๆ
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

        private async Task<List<VehicleInfo>> ImportStoreRun()
        {
            List<VehicleInfo> list = new List<VehicleInfo>();
            string conString = ConfigurationManager.ConnectionStrings["VIO_Connectionstring"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(conString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("P_VIOInsertUpdateExcel", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                }
            }
            return list;
        }

        //End Excel

        //start table
        public JsonResult GetVIOData(string marketID, string vehicleID, string maker, string rangID, string modelID, string bodyID, string engineID)
        {
            string message = string.Empty;
            List<VIO_DATA> list = new List<VIO_DATA>();
            string conString = ConfigurationManager.ConnectionStrings["VIO_Connectionstring"].ConnectionString;
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