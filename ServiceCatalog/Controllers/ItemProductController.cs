using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ServiceCatalog.Models;
using ServiceCatalog.Data;

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
        public ActionResult GetListItemProduct(string Stkcode, string BrandId, string RowNumber, string ApiStatus)
        {
            var SearchItemProduct = new List<StoredSearchItemProductsModel>();

            SearchItemProduct = new SearchItemProduct().SearchItem(Stkcode, BrandId, RowNumber, ApiStatus);
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
    }
}