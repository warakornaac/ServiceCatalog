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
    }
}