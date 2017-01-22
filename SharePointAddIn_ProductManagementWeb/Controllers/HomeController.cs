using Microsoft.SharePoint.Client;
using ProductManagement.Logic;
using ProductManagement.Models.View;
using SharePointAddIn_ProductManagementWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SharePointAddIn_ProductManagementWeb.Controllers
{
    public class HomeController : Controller
    {
        private ProductFactory _productFactory = new ProductFactory();
        private CountryFactory _countryFactory = new CountryFactory();

        [SharePointContextFilter]
        public ActionResult Index()
        {
            ViewBag.SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri;
            ViewBag.UserName = SharePointHelper.GetUserDisplayName(HttpContext);

            IEnumerable<ProductViewModel> models = _productFactory.GetNonRetiredProducts();

            return View(new ProductsViewModel()
            {
                Products = models
            });
        }

        [SharePointContextFilter]
        public ActionResult New()
        {
            ViewBag.SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri;

            ProductViewModel model = new ProductViewModel();
            model.Countries = _countryFactory.GetCountries();

            return View(model);
        }

        [SharePointContextFilter]
        [HttpPost]
        public ActionResult New(ProductViewModel model)
        {
            ViewBag.SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri;

            model.Countries = _countryFactory.GetCountries();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_productFactory.AddProduct(model))
            {
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            else
            {
                return View(model);
            }
        }

        [SharePointContextFilter]
        public ActionResult Edit(int id)
        {
            ViewBag.SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri;
            ViewBag.Message = "Edit Product: " + id;

            ProductViewModel model = _productFactory.GetProductByID(id);

            return View(model);
        }

        [SharePointContextFilter]
        [HttpPost]
        public ActionResult Edit(ProductViewModel model)
        {
            ViewBag.SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri;
            model.Countries = _countryFactory.GetCountries();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_productFactory.UpdateProduct(model))
            {
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            else
            {
                return View(model);
            }
        }

        [SharePointContextFilter]
        public ActionResult Delete(int id)
        {
            ViewBag.SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri;
            ProductViewModel model = _productFactory.GetProductByID(id);

            if (model != null)
            {
                _productFactory.DeleteProduct(model);
            }

            return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }
    }
}
