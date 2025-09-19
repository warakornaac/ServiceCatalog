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
    public class ApiProductController : Controller
    {
        // GET: ApiProduct
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CallByItemManual()
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
            @ViewBag.listBrand = listBrandMaster;
            return View("IndexCallByItemManual", new
            {
                @ViewBag.listBrand
            });
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
            @ViewBag.listBrand = listBrandMaster;
            return View("IndexCallByItemAutomate", new
            {
                @ViewBag.listBrand
            });
        }
        //check data 
        [HttpPost]
        public async Task<ActionResult> checkApiDataByPartno(string PartNo, int BrandId)
        {
            var url = "https://localhost:44361/Post/Articles";
            string statusCode = string.Empty;
            bool statusCallApi;
            string dataResponse = string.Empty;
            List<string> mfrNames = null;
            var post = new
            {
                Partno = PartNo,
                SupplierId = BrandId,
            };
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var handler = new HttpClientHandler();
                var client = new HttpClient(handler);
                string jsonContent = JsonConvert.SerializeObject(post);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dataResponse = responseContent;
                    statusCode = response.StatusCode.ToString();
                    statusCallApi = true;
                    if (!string.IsNullOrWhiteSpace(dataResponse))
                    {
                        JObject jsonResponse = JObject.Parse(dataResponse);
                        var mfrNamesToken = jsonResponse["MfrNames"];
                        if (mfrNamesToken != null && mfrNamesToken.Type != JTokenType.Null)
                        {
                            mfrNames = mfrNamesToken.ToObject<List<string>>();
                        }
                    }
                }
                else
                {
                    statusCode = response.StatusCode.ToString();
                    statusCallApi = false;
                }
            }
            catch (Exception ex)
            {
                statusCode = ex.Message;
                statusCallApi = false;
            }
            var result = new
            {
                statusCode = statusCode,
                statusCallApi = statusCallApi,
                MfrNames = mfrNames
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //get part detail, description, oem
        public async Task<ActionResult> getArticlesByPartno(string PartNo, int BrandId)
        {
            //var url = "https://mst.aac.co.th/APIService/Post/PushMessage";
            var url = "https://localhost:44361/Post/Articles";
            string status = string.Empty;
            string dataResponse = string.Empty;
            var post = new
            {
                Partno = PartNo,
                SupplierId = BrandId,
            };
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var handler = new HttpClientHandler();
                var client = new HttpClient(handler);
                string jsonContent = JsonConvert.SerializeObject(post);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dataResponse = responseContent;
                    status = response.StatusCode.ToString();
                }
                else
                {
                    status = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                dataResponse = ex.Message;
            }
            JObject jsonResponse = JObject.Parse(dataResponse);
            var articleIdToken = jsonResponse["ArticleId"];
            var articleNumberToken = jsonResponse["ArticleNumber"];
            var additionalDescriptionsToken = jsonResponse["AdditionalDescriptions"];
            var articleStatusDescriptionToken = jsonResponse["ArticleStatusDescription"];
            var quantityPerPackageToken = jsonResponse["QuantityPerPackage"];
            var mfrNamesToken = jsonResponse["MfrNames"];

            var articleId = articleIdToken != null && articleIdToken.Type != JTokenType.Null
                ? articleIdToken.ToObject<List<string>>()
                : null;
            var articleNumber = articleNumberToken != null && articleNumberToken.Type != JTokenType.Null
                ? articleNumberToken.ToObject<List<string>>()
                : null;
            var additionalDescriptions = additionalDescriptionsToken != null && additionalDescriptionsToken.Type != JTokenType.Null
                ? additionalDescriptionsToken.ToObject<List<string>>()
                : null;
            var articleStatusDescription = articleStatusDescriptionToken != null && articleStatusDescriptionToken.Type != JTokenType.Null
                ? articleStatusDescriptionToken.ToObject<List<string>>()
                : null;
            var quantityPerPackage = quantityPerPackageToken != null && quantityPerPackageToken.Type != JTokenType.Null
               ? quantityPerPackageToken.ToObject<List<string>>()
               : null;
            var mfrNames = mfrNamesToken != null && mfrNamesToken.Type != JTokenType.Null
                ? mfrNamesToken.ToObject<List<string>>()
                : null;
            @ViewBag.articleId = articleId;
            @ViewBag.articleNumber = articleNumber;
            @ViewBag.additionalDescriptions = additionalDescriptions;
            @ViewBag.articleStatusDescription = articleStatusDescription;
            @ViewBag.quantityPerPackage = quantityPerPackage; //จำนวน
            @ViewBag.mfrName = mfrNames;
            ArticleDataModels articleResponse = JsonConvert.DeserializeObject<ArticleDataModels>(dataResponse);
            @ViewBag.dataResponse = articleResponse;
            //Competitor - คู่แข่ง
            //Product_Competitor
            if (!string.IsNullOrEmpty(PartNo) && BrandId > 0)
            {
                var dataCompetitor = await getArticlesPartNumberNearby(PartNo, BrandId);
                if (dataCompetitor == null || dataCompetitor == null)
                {
                    ViewBag.dataCompetitor = new List<PartNumberNearbyData>();
                }
                else
                {
                    @ViewBag.dataCompetitor = dataCompetitor.PartNumberNearby;
                }
            }
            //KType
            //Product_Linkage
            if (!string.IsNullOrEmpty(articleId[0]))
            {
                var dataLinkage = await GetLinkageByArticleId(articleId[0]);
                if (dataLinkage == null || dataLinkage.LinkageDetails == null)
                {
                    ViewBag.dataLinkage = new List<LinkageDetails>();
                }
                else
                {
                    @ViewBag.dataLinkage = dataLinkage.LinkageDetails;
                }
            }
            return PartialView("_listCallByItemManual_2", new
            {
                @ViewBag.articleId,
                @ViewBag.articleNumber,
                @ViewBag.additionalDescriptions,
                @ViewBag.articleStatusDescription,
                @ViewBag.mfrName,
                @ViewBag.dataResponse,
                @ViewBag.dataCompetitor,
                @ViewBag.dataLinkage
            });
        }
        //Save To DB
        [HttpPost]
        public async Task<ActionResult> SaveProductApiToDb(string PartNo, int BrandId)
        {
            var url = "https://localhost:44361/Post/Articles";
            string status = string.Empty;
            string dataResponse = string.Empty;
            var post = new
            {
                Partno = PartNo,
                SupplierId = BrandId,
            };
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var handler = new HttpClientHandler();
                var client = new HttpClient(handler);
                string jsonContent = JsonConvert.SerializeObject(post);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dataResponse = responseContent;
                    status = response.StatusCode.ToString();
                }
                else
                {
                    status = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                dataResponse = ex.Message;
            }
            JObject jsonResponse = JObject.Parse(dataResponse);
            var articleIdToken = jsonResponse["ArticleId"];
            var articleNumberToken = jsonResponse["ArticleNumber"];
            var additionalDescriptionsToken = jsonResponse["AdditionalDescriptions"];
            var articleStatusDescriptionToken = jsonResponse["ArticleStatusDescription"];
            var quantityPerPackageToken = jsonResponse["QuantityPerPackage"];
            var mfrNamesToken = jsonResponse["MfrNames"];

            var articleId = articleIdToken != null && articleIdToken.Type != JTokenType.Null
                ? articleIdToken.ToObject<List<string>>()
                : null;
            var articleNumber = articleNumberToken != null && articleNumberToken.Type != JTokenType.Null
                ? articleNumberToken.ToObject<List<string>>()
                : null;
            var additionalDescriptions = additionalDescriptionsToken != null && additionalDescriptionsToken.Type != JTokenType.Null
                ? additionalDescriptionsToken.ToObject<List<string>>()
                : null;
            var articleStatusDescription = articleStatusDescriptionToken != null && articleStatusDescriptionToken.Type != JTokenType.Null
                ? articleStatusDescriptionToken.ToObject<List<string>>()
                : null;
            var quantityPerPackage = quantityPerPackageToken != null && quantityPerPackageToken.Type != JTokenType.Null
               ? quantityPerPackageToken.ToObject<List<string>>()
               : null;
            var mfrNames = mfrNamesToken != null && mfrNamesToken.Type != JTokenType.Null
                ? mfrNamesToken.ToObject<List<string>>()
                : null;
            ArticleDataModels articleResponse = JsonConvert.DeserializeObject<ArticleDataModels>(dataResponse);
            //Set Array Product Details
            //Product_Description
            int countRowDescription = 0;
            int countRowSpec = 0;
            int countRowOem = 0;
            int countRowImage = 0;
            int countRowCompetitor = 0;
            int countRowLinkage = 0;
            var setArrayByTable = new List<ProductDataToArray>();
            //แบรนด์
            if (mfrNames != null && mfrNames.Count > 0 && !string.IsNullOrEmpty(mfrNames[0]))
            {
                SetDataToArray(setArrayByTable, ++countRowDescription, "แบรนด์", mfrNames[0], "", "", "Product_Description");
            }
            //หมายเลขสินค้า
            if (articleNumber != null && articleNumber.Count > 0 && !string.IsNullOrEmpty(articleNumber[0]))
            {
                SetDataToArray(setArrayByTable, ++countRowDescription, "หมายเลขสินค้า", articleNumber[0], "", "", "Product_Description");
            }
            //กลุ่มผลิตภัณฑ์
            if (articleResponse.GenericArticles != null)
            {
                foreach (var rowDataDescription in articleResponse.GenericArticles)
                {
                    SetDataToArray(setArrayByTable, ++countRowDescription, "กลุ่มผลิตภัณฑ์", rowDataDescription.genericArticleDescription, "", "", "Product_Description");
                }
            }
            //หน่วยบรรจุภัณฑ์
            if (quantityPerPackage != null && quantityPerPackage.Count > 0 && !string.IsNullOrEmpty(quantityPerPackage[0]))
            {
                SetDataToArray(setArrayByTable, ++countRowDescription, "หน่วยบรรจุภัณฑ์", quantityPerPackage[0], "", "", "Product_Description");
            }
            //คำอธิบายเพิ่มเติม
            if (additionalDescriptions != null && additionalDescriptions.Count > 0 && !string.IsNullOrEmpty(additionalDescriptions[0]))
            {
                SetDataToArray(setArrayByTable, ++countRowDescription, "คำอธิบายเพิ่มเติม", additionalDescriptions[0], "", "", "Product_Description");
            }
            //หมายเลขการใช้งาน
            if (articleResponse.TradeNumberDetail != null)
            {
                foreach (var rowDataTrade in articleResponse.TradeNumberDetail)
                {
                    SetDataToArray(setArrayByTable, ++countRowDescription, "หมายเลขการใช้งาน", rowDataTrade.tradeNumber, "", "", "Product_Description");
                }
            }
            //Set Array Product Specifications
            //Product_Spec
            countRowDescription = 0;
            if (articleResponse.ArticleCriteria != null)
            {
                foreach (var rowDataDescription in articleResponse.ArticleCriteria)
                {
                    SetDataToArray(setArrayByTable, ++countRowSpec, rowDataDescription.criteriaAbbrDescription, rowDataDescription.formattedValue + " " + rowDataDescription.criteriaUnitDescription, "", "", "Product_Spec");
                };
            }
            //OEM
            //Product_Oem
            countRowDescription = 0;
            if (articleResponse.ArticleCriteria != null)
            {
                foreach (var rowDataOem in articleResponse.OemNumbers)
                {
                    SetDataToArray(setArrayByTable, ++countRowOem, "", rowDataOem.articleNumber, rowDataOem.mfrName, "", "Product_Oem");
                };
            }
            //Image
            //Product_Image
            countRowDescription = 0;
            if (articleResponse.Images != null)
            {
                foreach (var rowDataImage in articleResponse.Images)
                {
                    ++countRowDescription;
                    SetDataToArray(setArrayByTable, ++countRowImage, rowDataImage.fileName, rowDataImage.imageURL800, "", "", "Product_Image");
                };
            }
            //Competitor - คู่แข่ง
            //Product_Competitor
            if (!string.IsNullOrEmpty(PartNo) && BrandId > 0)
            {
                var dataCompetitor = await getArticlesPartNumberNearby(PartNo, BrandId);
                if (dataCompetitor != null || dataCompetitor != null)
                {
                    //@ViewBag.dataCompetitor = dataCompetitor.PartNumberNearby;
                    foreach (var rowComp in dataCompetitor.PartNumberNearby)
                    {
                        SetDataToArray(setArrayByTable, ++countRowCompetitor, "", rowComp.articleNo.ToString(), rowComp.brandName, "", "Product_Competitor");
                    }
                }
            }
            //KType
            //Product_Linkage
            if (articleId != null && articleId.Count > 0 && !string.IsNullOrEmpty(articleId[0]))
            {
                var dataLinkage = await GetLinkageByArticleId(articleId[0]);
                if (dataLinkage != null && dataLinkage.LinkageDetails != null)
                {
                    foreach (var rowDataLinkage in dataLinkage.LinkageDetails)
                    {
                        SetDataToArray(setArrayByTable, ++countRowLinkage, "", rowDataLinkage.LinkageTargetId.ToString(), rowDataLinkage.MfrName, rowDataLinkage.VehicleModelSeriesName, "Product_Linkage");
                    }
                }
            }
            //Save to DB
            var saveProductRowSuccess = new List<ProductDataToArray>();
            var saveProductRowFail = new List<FailedSaveRecord>();
            if (setArrayByTable.Count == 0) //Call api เจอข้อมูล
            {
                var UpdateProductApiModel = new UpdateProductApi().SaveProductApi(
                          "",
                          PartNo,
                          BrandId,
                          0,
                          "",
                          "",
                          "",
                          "",
                          "SystemAdmin"
                      );
            }
            else //Call api ไม่พบข้อมูล
            {
                foreach (var rowData in setArrayByTable)
                {
                    try
                    {
                        var UpdateProductApiModel = new UpdateProductApi().SaveProductApi(
                            rowData.Table,
                            PartNo,
                            BrandId,
                            rowData.Seq,
                            rowData.Title,
                            rowData.Description,
                            rowData.Maker,
                            rowData.Model,
                            "SystemAdmin"
                        );
                        if (UpdateProductApiModel != null) // บันทึกสำเร็จ
                        {
                            saveProductRowSuccess.Add(rowData);
                        }
                        else // บันทึกไม่สำเร็จ
                        {
                            saveProductRowFail.Add(new FailedSaveRecord
                            {
                                Table = rowData.Table,
                                Seq = rowData.Seq,
                                Title = rowData.Title,
                                Description = rowData.Description,
                                Maker = rowData.Maker,
                                Model = rowData.Model,
                                ErrorMessage = "SaveProductApi Fail"
                            });
                        }
                    }
                    catch (Exception ex) //Error
                    {
                        saveProductRowFail.Add(new FailedSaveRecord
                        {
                            Table = rowData.Table,
                            Seq = rowData.Seq,
                            Title = rowData.Title,
                            Description = rowData.Description,
                            Maker = rowData.Maker,
                            Model = rowData.Model,
                            ErrorMessage = ex.Message
                        });
                    }
                }
            }
            var resultProductSuccess = saveProductRowSuccess
                .GroupBy(item => item.Table)
                .ToDictionary(g => g.Key, g => g.Count());
            var resultProductFail = saveProductRowFail
                .GroupBy(item => item.Table)
                .ToDictionary(g => g.Key, g => g.Count());

            return Json(new
            {
                status = resultProductFail.Any() ? "fail" : "success",
                message = resultProductFail.Any() ? "บางรายการบันทึไม่สำเร็จ" : "บันทึกสำเร็จทั้งหมด",
                countProductSuccess = resultProductSuccess.Count,
                countProductFail = resultProductFail.Count,
                resultProductSuccess = resultProductSuccess,
                resultProductFail = resultProductFail,
            });
        }
        [HttpPost]
        public async Task<ActionResult> RunTaskSaveProductApiToDb(string PartNo, int BrandId)
        {
            var url = "https://localhost:44361/Post/Articles";
            string status = string.Empty;
            string dataResponse = string.Empty;
            var post = new
            {
                Partno = PartNo,
                SupplierId = BrandId,
            };
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var handler = new HttpClientHandler();
                var client = new HttpClient(handler);
                string jsonContent = JsonConvert.SerializeObject(post);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dataResponse = responseContent;
                    status = response.StatusCode.ToString();
                }
                else
                {
                    status = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                dataResponse = ex.Message;
            }
            JObject jsonResponse = JObject.Parse(dataResponse);
            var articleIdToken = jsonResponse["ArticleId"];
            var articleNumberToken = jsonResponse["ArticleNumber"];
            var additionalDescriptionsToken = jsonResponse["AdditionalDescriptions"];
            var articleStatusDescriptionToken = jsonResponse["ArticleStatusDescription"];
            var quantityPerPackageToken = jsonResponse["QuantityPerPackage"];
            var mfrNamesToken = jsonResponse["MfrNames"];

            var articleId = articleIdToken != null && articleIdToken.Type != JTokenType.Null
                ? articleIdToken.ToObject<List<string>>()
                : null;
            var articleNumber = articleNumberToken != null && articleNumberToken.Type != JTokenType.Null
                ? articleNumberToken.ToObject<List<string>>()
                : null;
            var additionalDescriptions = additionalDescriptionsToken != null && additionalDescriptionsToken.Type != JTokenType.Null
                ? additionalDescriptionsToken.ToObject<List<string>>()
                : null;
            var articleStatusDescription = articleStatusDescriptionToken != null && articleStatusDescriptionToken.Type != JTokenType.Null
                ? articleStatusDescriptionToken.ToObject<List<string>>()
                : null;
            var quantityPerPackage = quantityPerPackageToken != null && quantityPerPackageToken.Type != JTokenType.Null
               ? quantityPerPackageToken.ToObject<List<string>>()
               : null;
            var mfrNames = mfrNamesToken != null && mfrNamesToken.Type != JTokenType.Null
                ? mfrNamesToken.ToObject<List<string>>()
                : null;
            ArticleDataModels articleResponse = JsonConvert.DeserializeObject<ArticleDataModels>(dataResponse);
            //Set Array Product Details
            //Product_Description
            int countRowDescription = 0;
            int countRowSpec = 0;
            int countRowOem = 0;
            int countRowImage = 0;
            int countRowCompetitor = 0;
            int countRowLinkage = 0;
            var setArrayByTable = new List<ProductDataToArray>();
            //แบรนด์
            if (mfrNames != null && mfrNames.Count > 0 && !string.IsNullOrEmpty(mfrNames[0]))
            {
                SetDataToArray(setArrayByTable, ++countRowDescription, "แบรนด์", mfrNames[0], "", "", "Product_Description");
            }
            //หมายเลขสินค้า
            if (articleNumber != null && articleNumber.Count > 0 && !string.IsNullOrEmpty(articleNumber[0]))
            {
                SetDataToArray(setArrayByTable, ++countRowDescription, "หมายเลขสินค้า", articleNumber[0], "", "", "Product_Description");
            }
            //กลุ่มผลิตภัณฑ์
            if (articleResponse.GenericArticles != null)
            {
                foreach (var rowDataDescription in articleResponse.GenericArticles)
                {
                    SetDataToArray(setArrayByTable, ++countRowDescription, "กลุ่มผลิตภัณฑ์", rowDataDescription.genericArticleDescription, "", "", "Product_Description");
                }
            }
            //หน่วยบรรจุภัณฑ์
            if (quantityPerPackage != null && quantityPerPackage.Count > 0 && !string.IsNullOrEmpty(quantityPerPackage[0]))
            {
                SetDataToArray(setArrayByTable, ++countRowDescription, "หน่วยบรรจุภัณฑ์", quantityPerPackage[0], "", "", "Product_Description");
            }
            //คำอธิบายเพิ่มเติม
            if (additionalDescriptions != null && additionalDescriptions.Count > 0 && !string.IsNullOrEmpty(additionalDescriptions[0]))
            {
                SetDataToArray(setArrayByTable, ++countRowDescription, "คำอธิบายเพิ่มเติม", additionalDescriptions[0], "", "", "Product_Description");
            }
            //หมายเลขการใช้งาน
            if (articleResponse.TradeNumberDetail != null)
            {
                foreach (var rowDataTrade in articleResponse.TradeNumberDetail)
                {
                    SetDataToArray(setArrayByTable, ++countRowDescription, "หมายเลขการใช้งาน", rowDataTrade.tradeNumber, "", "", "Product_Description");
                }
            }
            //Set Array Product Specifications
            //Product_Spec
            countRowDescription = 0;
            if (articleResponse.ArticleCriteria != null)
            {
                foreach (var rowDataDescription in articleResponse.ArticleCriteria)
                {
                    SetDataToArray(setArrayByTable, ++countRowSpec, rowDataDescription.criteriaAbbrDescription, rowDataDescription.formattedValue + " " + rowDataDescription.criteriaUnitDescription, "", "", "Product_Spec");
                };
            }
            //OEM
            //Product_Oem
            countRowDescription = 0;
            if (articleResponse.ArticleCriteria != null)
            {
                foreach (var rowDataOem in articleResponse.OemNumbers)
                {
                    SetDataToArray(setArrayByTable, ++countRowOem, "", rowDataOem.articleNumber, rowDataOem.mfrName, "", "Product_Oem");
                };
            }
            //Image
            //Product_Image
            countRowDescription = 0;
            if (articleResponse.Images != null)
            {
                foreach (var rowDataImage in articleResponse.Images)
                {
                    ++countRowDescription;
                    SetDataToArray(setArrayByTable, ++countRowImage, rowDataImage.fileName, rowDataImage.imageURL800, "", "", "Product_Image");
                };
            }
            //Competitor - คู่แข่ง
            //Product_Competitor
            if (!string.IsNullOrEmpty(PartNo) && BrandId > 0)
            {
                var dataCompetitor = await getArticlesPartNumberNearby(PartNo, BrandId);
                if (dataCompetitor != null || dataCompetitor != null)
                {
                    //@ViewBag.dataCompetitor = dataCompetitor.PartNumberNearby;
                    foreach (var rowComp in dataCompetitor.PartNumberNearby)
                    {
                        SetDataToArray(setArrayByTable, ++countRowCompetitor, "", rowComp.articleNo.ToString(), rowComp.brandName, "", "Product_Competitor");
                    }
                }
            }
            //KType
            //Product_Linkage
            if (articleId != null && articleId.Count > 0 && !string.IsNullOrEmpty(articleId[0]))
            {
                var dataLinkage = await GetLinkageByArticleId(articleId[0]);
                if (dataLinkage != null && dataLinkage.LinkageDetails != null)
                {
                    foreach (var rowDataLinkage in dataLinkage.LinkageDetails)
                    {
                        SetDataToArray(setArrayByTable, ++countRowLinkage, "", rowDataLinkage.LinkageTargetId.ToString(), rowDataLinkage.MfrName, rowDataLinkage.VehicleModelSeriesName, "Product_Linkage");
                    }
                }
            }
            //Save to DB
            var saveProductRowSuccess = new List<ProductDataToArray>();
            var saveProductRowFail = new List<FailedSaveRecord>();
            if (setArrayByTable.Count == 0) //Call api ไม่พบข้อมูล
            {
                var UpdateProductApiModel = new UpdateProductApi().SaveProductApi(
                          "",
                          PartNo,
                          BrandId,
                          0,
                          "",
                          "",
                          "",
                          "",
                          "SystemAdmin"
                      );
            }
            else //Call api เจอข้อมูล
            {
                foreach (var rowData in setArrayByTable)
                {
                    try
                    {
                        var UpdateProductApiModel = new UpdateProductApi().SaveProductApi(
                            rowData.Table,
                            PartNo,
                            BrandId,
                            rowData.Seq,
                            rowData.Title,
                            rowData.Description,
                            rowData.Maker,
                            rowData.Model,
                            "SystemAdmin"
                        );
                        if (UpdateProductApiModel != null) // บันทึกสำเร็จ
                        {
                            saveProductRowSuccess.Add(rowData);
                        }
                        else // บันทึกไม่สำเร็จ
                        {
                            saveProductRowFail.Add(new FailedSaveRecord
                            {
                                Table = rowData.Table,
                                Seq = rowData.Seq,
                                Title = rowData.Title,
                                Description = rowData.Description,
                                Maker = rowData.Maker,
                                Model = rowData.Model,
                                ErrorMessage = "SaveProductApi Fail"
                            });
                        }
                    }
                    catch (Exception ex) //Error
                    {
                        saveProductRowFail.Add(new FailedSaveRecord
                        {
                            Table = rowData.Table,
                            Seq = rowData.Seq,
                            Title = rowData.Title,
                            Description = rowData.Description,
                            Maker = rowData.Maker,
                            Model = rowData.Model,
                            ErrorMessage = ex.Message
                        });
                    }
                }
            }
            var resultProductSuccess = saveProductRowSuccess
                .GroupBy(item => item.Table)
                .ToDictionary(g => g.Key, g => g.Count());
            var resultProductFail = saveProductRowFail
                .GroupBy(item => item.Table)
                .ToDictionary(g => g.Key, g => g.Count());

            return Json(new
            {
                status = resultProductFail.Any() ? "fail" : "success",
                message = resultProductFail.Any() ? "บางรายการบันทึไม่สำเร็จ" : "บันทึกสำเร็จทั้งหมด",
                countProductSuccess = resultProductSuccess.Count,
                countProductFail = resultProductFail.Count,
                resultProductSuccess = resultProductSuccess,
                resultProductFail = resultProductFail,
                resultPartNo = PartNo,
                resultBrandId = BrandId
            });
        }
        //Set data to array
        private void SetDataToArray(List<ProductDataToArray> arrayByTable, int seq, string title, string description, string maker, string model, string tableName = null)
        {
            arrayByTable.Add(new ProductDataToArray
            {
                Table = tableName,
                Seq = seq,
                Title = title,
                Description = description,
                Maker = maker,
                Model = model,
            });
        }
        //get part Competitor
        public async Task<PartNumberNearbyData> getArticlesPartNumberNearby(string Partno, int BrandId)
        {
            var url = "https://localhost:44361/Post/ArticlesPartNumberNearby";
            string status = string.Empty;
            string dataResponse = string.Empty;
            var postData = new
            {
                Partno = Partno,
                SupplierId = BrandId,
            };
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                      | SecurityProtocolType.Tls11
                                                      | SecurityProtocolType.Tls12;
                using (var client = new HttpClient())
                {
                    string jsonContent = JsonConvert.SerializeObject(postData);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, httpContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<PartNumberNearbyData>(json);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //get KType
        public async Task<LinkageData> GetLinkageByArticleId(string articleId)
        {
            var url = "https://localhost:44361/Post/ArticlesLinkedAll";
            var postData = new
            {
                ArticleId = articleId
            };
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                      | SecurityProtocolType.Tls11
                                                      | SecurityProtocolType.Tls12;
                using (var client = new HttpClient())
                {
                    string jsonContent = JsonConvert.SerializeObject(postData);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, httpContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LinkageData>(json);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //Save To DB
        [HttpPost]
        public ActionResult DeleteProductApiToDb(string PartNo, int BrandId)
        {
            string getResult = "";
            var connectionString = Utils.GetConfig("ServiceCatalogDB");
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                Connection.Open();
                var cmdSearch = new SqlCommand("P_Delete_Product_Api", Connection);

                cmdSearch.CommandType = CommandType.StoredProcedure;
                cmdSearch.Parameters.AddWithValue("@inStkcode", PartNo);
                cmdSearch.Parameters.AddWithValue("@inBrandId", BrandId);
                SqlParameter returnResult = new SqlParameter("@outResult", SqlDbType.NVarChar, 1000);
                returnResult.Direction = ParameterDirection.Output;
                cmdSearch.Parameters.Add(returnResult);
                int Insid = cmdSearch.ExecuteNonQuery();
                getResult = cmdSearch.Parameters["@outResult"].Value.ToString();
                cmdSearch.Dispose();
                Connection.Close();
            }
            return Json(new { status = "success", message = getResult });
        }
        //get list brand
        public JsonResult GetBrandMaster()
        {
            List<SelectListItem> listBrandMaster = new List<SelectListItem>();
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString))
            {
                Connection.Open();
                var command = new SqlCommand("P_Search_Brand", Connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader drList = command.ExecuteReader();
                while (drList.Read())
                {
                    listBrandMaster.Add(new SelectListItem() { Value = drList["BrandId"].ToString(), Text = drList["BrandId"].ToString() + "/" + drList["BrandName"].ToString() });
                }
            }
            return Json(listBrandMaster, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProductApiCount(string Stkcode)
        {
            List<ListProductApiCount> ListProductApiCount = new List<ListProductApiCount>();
            var connectionString = ConfigurationManager.ConnectionStrings["ServiceCatalogDB"].ConnectionString;
            SqlConnection Connection = new SqlConnection(connectionString);
            var command = new SqlCommand("P_Product_Api_Count", Connection);
            command.Parameters.AddWithValue("@inStkcode", Stkcode);
            command.CommandType = CommandType.StoredProcedure;
            Connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                ListProductApiCount.Add(new ListProductApiCount()
                {
                    Stkcode = dr["Stkcode"].ToString(),
                    BrandId = dr["BrandId"].ToString(),
                    countProductDes = dr["countProductDes"].ToString(),
                    countProductSpec = dr["countProductSpec"].ToString(),
                    countProductImage = dr["countProductImage"].ToString(),
                    countProductOem = dr["countProductOem"].ToString(),
                    countProductCom = dr["countProductCom"].ToString(),
                    countProductLinkage = dr["countProductLinkage"].ToString()
                });
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();
            Connection.Close();
            return Json(ListProductApiCount, JsonRequestBehavior.AllowGet);
        }
        public class ListProductApiCount
        {
            public string Stkcode { get; set; }
            public string BrandId { get; set; }
            public string countProductDes { get; set; }
            public string countProductSpec { get; set; }
            public string countProductImage { get; set; }
            public string countProductOem { get; set; }
            public string countProductCom { get; set; }
            public string countProductLinkage { get; set; }
        }
        public class ProductDataToArray
        {
            public string Table { get; set; }
            public int Seq { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Maker { get; set; }
            public string Model { get; set; }
        }
        public class FailedSaveRecord
        {
            public string Table { get; set; }
            public int Seq { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Maker { get; set; }
            public string Model { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}