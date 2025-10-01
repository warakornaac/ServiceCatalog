using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using ServiceCatalog.Models;
using ServiceCatalog.Data;
using ClosedXML.Excel;

namespace ServiceCatalog.Controllers
{
    public class ItemProductController : Controller
    {
        // GET: ItemProduct
        public ActionResult Index()
        {
            return View("IndexItemProduct", new
            {
                // @ViewBag.listBrand
            });
        }
        public ActionResult GetListItemProduct(string Stkcode, string BrandId, string RowNumber, string ApiStatus, string Calldate)
        {
            var SearchItemProduct = new List<StoredSearchItemProductsModel>();

            SearchItemProduct = new SearchItemProduct().SearchItem(Stkcode, BrandId, RowNumber, ApiStatus, Calldate);
            @ViewBag.listSearchItemProduct = SearchItemProduct;
            return PartialView("_ListItemProduct", new
            {
                @ViewBag.listSearchItemProduct
            });
        }
        public ActionResult getItemProductDeatil(string Stkcode)
        {
            DateTime? startDate = null;
            DateTime? endDate = null;

            var ListProductDescription = new List<ProductDescriptionModel>();
            var ListProductSpec = new List<ProductSpecModel>();

            string message = string.Empty;
            try
            {

                ListProductDescription = new GetProductDescriptionList().Get(Stkcode);
                ListProductSpec = new GetProductSpecList().Get(Stkcode);
               
                ViewBag.ListProductDescription = ListProductDescription;
                ViewBag.ListProductSpec = ListProductSpec;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ViewBag.ListProductDescription = new List<object>();
                ViewBag.ListProductSpec = new List<object>();

            }
            ViewBag.Message = message;
            return PartialView("_DeatilItemProduct", new
            {
                @ViewBag.Message,
                @ViewBag.ListProductDescription,
                @ViewBag.ListProductSpec
            });
        }
        //Get stored
        private DataTable GetDataFromStore(string storeName, string inSheet)
        {
            var dt = new DataTable();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString))
            using (var cmd = new SqlCommand(storeName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // เพิ่ม parameter @inSheet
                cmd.Parameters.AddWithValue("@inSheet", inSheet ?? string.Empty);

                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }
        public ActionResult ExportProductItem()
        {
            var listProductApi = GetDataFromStore("P_Export_Product_Api", "Product_Api");
            var listProductDescription = GetDataFromStore("P_Export_Product_Api", "Product_Description");
            var listProductSpec = GetDataFromStore("P_Export_Product_Api", "Product_Spec");
            var listProductCompetitor = GetDataFromStore("P_Export_Product_Api", "Product_Competitor");
            var listProductOem = GetDataFromStore("P_Export_Product_Api", "Product_Oem");
            var listProductImage = GetDataFromStore("P_Export_Product_Api", "Product_Image");
            var listProductLinkage = GetDataFromStore("P_Export_Product_Api", "Product_Linkage");

            using (var workbook = new XLWorkbook())
            {
                var worksheet1 = workbook.Worksheets.Add("Product_Api");
                var worksheet2 = workbook.Worksheets.Add("Product_Description");
                var worksheet3 = workbook.Worksheets.Add("Product_Spec");
                var worksheet4 = workbook.Worksheets.Add("Product_Competitor");
                var worksheet5 = workbook.Worksheets.Add("Product_Oem");
                var worksheet6 = workbook.Worksheets.Add("Product_Image");
                var worksheet7 = workbook.Worksheets.Add("Product_Linkage");
                // ========= Sheet 1: Product_Api =========
                for (int i = 0; i < listProductApi.Columns.Count; i++)
                {
                    worksheet1.Cell(1, i + 1).Value = listProductApi.Columns[i].ColumnName;
                }
                for (int i = 0; i < listProductApi.Rows.Count; i++)
                {
                    for (int j = 0; j < listProductApi.Columns.Count; j++)
                    {
                        worksheet1.Cell(i + 2, j + 1).Value = listProductApi.Rows[i][j];
                    }
                }
                // ========= Sheet 2: Product_Description =========
                for (int i = 0; i < listProductDescription.Columns.Count; i++)
                {
                    worksheet2.Cell(1, i + 1).Value = listProductDescription.Columns[i].ColumnName;
                }
                for (int i = 0; i < listProductDescription.Rows.Count; i++)
                {
                    for (int j = 0; j < listProductDescription.Columns.Count; j++)
                    {
                        worksheet2.Cell(i + 2, j + 1).Value = listProductDescription.Rows[i][j];
                    }
                }
                // ========= Sheet 3: Product_Spec =========
                for (int i = 0; i < listProductSpec.Columns.Count; i++)
                {
                    worksheet3.Cell(1, i + 1).Value = listProductSpec.Columns[i].ColumnName;
                }
                for (int i = 0; i < listProductSpec.Rows.Count; i++)
                {
                    for (int j = 0; j < listProductSpec.Columns.Count; j++)
                    {
                        worksheet3.Cell(i + 2, j + 1).Value = listProductSpec.Rows[i][j];
                    }
                }
                // ========= Sheet 4: Product_Competitor =========
                for (int i = 0; i < listProductCompetitor.Columns.Count; i++)
                {
                    worksheet4.Cell(1, i + 1).Value = listProductCompetitor.Columns[i].ColumnName;
                }
                for (int i = 0; i < listProductCompetitor.Rows.Count; i++)
                {
                    for (int j = 0; j < listProductCompetitor.Columns.Count; j++)
                    {
                        worksheet4.Cell(i + 2, j + 1).Value = listProductCompetitor.Rows[i][j];
                    }
                }      
                // ========= Sheet 5: Product_Oem =========
                for (int i = 0; i < listProductOem.Columns.Count; i++)
                {
                    worksheet5.Cell(1, i + 1).Value = listProductOem.Columns[i].ColumnName;
                }
                for (int i = 0; i < listProductOem.Rows.Count; i++)
                {
                    for (int j = 0; j < listProductOem.Columns.Count; j++)
                    {
                        worksheet5.Cell(i + 2, j + 1).Value = listProductOem.Rows[i][j];
                    }
                }
                // ========= Sheet 6: Product_Image =========
                for (int i = 0; i < listProductImage.Columns.Count; i++)
                {
                    worksheet6.Cell(1, i + 1).Value = listProductImage.Columns[i].ColumnName;
                }
                for (int i = 0; i < listProductImage.Rows.Count; i++)
                {
                    for (int j = 0; j < listProductImage.Columns.Count; j++)
                    {
                        worksheet6.Cell(i + 2, j + 1).Value = listProductImage.Rows[i][j];
                    }
                }
                // ========= Sheet 7: Product_Image =========
                for (int i = 0; i < listProductLinkage.Columns.Count; i++)
                {
                    worksheet7.Cell(1, i + 1).Value = listProductLinkage.Columns[i].ColumnName;
                }
                for (int i = 0; i < listProductLinkage.Rows.Count; i++)
                {
                    for (int j = 0; j < listProductLinkage.Columns.Count; j++)
                    {
                        worksheet7.Cell(i + 2, j + 1).Value = listProductLinkage.Rows[i][j];
                    }
                }

                // Save to stream and return file
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "ProductApiExport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
                }
            }
        }
    }
}