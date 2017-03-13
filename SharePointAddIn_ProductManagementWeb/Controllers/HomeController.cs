using Microsoft.SharePoint.Client;
using ProductManagement.Logic;
using ProductManagement.Models.View;
using SharePointAddIn_ProductManagementWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

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

        [SharePointContextFilter]
        [HttpGet]
        public ActionResult UploadFiles()
        {
            ViewBag.SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri;
            ViewBag.UserName = SharePointHelper.GetUserDisplayName(HttpContext);

            return View("UploadFiles");
        }

        [SharePointContextFilter]
        [HttpPost]
        public async Task<ContentResult> UploadFileContent(string code, bool useSet)
        {
            if (!string.IsNullOrEmpty(code))
            {
                Trace.TraceInformation("Product Code: " + code);
            }

            // Callback name is passed if upload happens via iframe, not AJAX (FileAPI).
            string callback = Request.Form["fd-callback"];
            string name;
            byte[] data;
            string _libraryName = "Products Manuals";
            string setURL = "";

            if (useSet)
            {
                setURL = SharePointHelper.CreateDocumentSet(HttpContext, _libraryName, code);
            }

            // Upload data can be POST'ed as raw form data or uploaded via <iframe> and <form>
            // using regular multipart/form-data enctype (which is handled by ASP.NET Request.Files).
            HttpPostedFileBase fdFile = Request.Files["fd-file"];
            if (fdFile != null)
            {
                // Regular multipart/form-data upload.
                name = fdFile.FileName;
                data = new byte[fdFile.ContentLength];
                fdFile.InputStream.Read(data, 0, fdFile.ContentLength);
            }
            else
            {
                // Raw POST data.
                name = HttpUtility.UrlDecode(Request.Headers["X-File-Name"]);
                data = new byte[Request.InputStream.Length];
                Request.InputStream.Read(data, 0, (int)Request.InputStream.Length); //up to 2GB
            }

            // get a stream
            MemoryStream stream = new MemoryStream(data);
            // and optionally write the file to disk
            //var fileName = Path.GetFileName(name);
            //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            //using (var fileStream = System.IO.File.Create(path))
            //{
            //    stream.CopyTo(fileStream);
            //}

            // Upload to SharePoint
            Microsoft.SharePoint.Client.File uploadedFile = SharePointHelper.UploadFileStream(HttpContext, _libraryName, setURL + name, stream);

            SharePointHelper.UpdateFileInformation(HttpContext, uploadedFile, code);

            // Output message for this demo upload. In your real app this would be something
            // meaningful for the calling script (that uses FileDrop.js).
            byte[] md5Hash;
            using (MD5 md5 = MD5.Create())
            {
                md5Hash = md5.ComputeHash(data);
            }

            var output = string.Format("{0}; received {1} bytes, MD5 = {2}", name, data.Length, BytesArrayToHexString(md5Hash));

            // In FileDrop sample this demonstrates the passing of custom ?query variables along
            // with an AJAX/iframe upload.
            string opt = Request["upload_option"];
            if (!string.IsNullOrEmpty(opt))
            {
                output += "\nReceived upload_option with value " + opt;
                Trace.TraceInformation("File upload options: " + opt);
            }

            if (!string.IsNullOrEmpty(callback))
            {
                // Callback function given - the caller loads response into a hidden <iframe> so
                // it expects it to be a valid HTML calling this callback function.
                //Response.Headers["Content-Type"] = "text/html; charset=utf-8";
                output = HttpUtility.JavaScriptStringEncode(output);

                //Response.Write(
                //    "<!DOCTYPE html><html><head></head><body><script type=\"text/javascript\">" +
                //   "try{window.top." + callback + "(\"" + output + "\")}catch(e){}</script></body></html>");

                string result = "<!DOCTYPE html><html><head></head><body><script type=\"text/javascript\">" +
                   "try{window.top." + callback + "(\"" + output + "\")}catch(e){}</script></body></html>";

                return Content(result, "text/html; charset=utf-8");
            }
            else
            {
                //Response.Headers["Content-Type"] = "text/plain; charset=utf-8";
                //Response.Write(output);

                return Content(output, "text/plain; charset=utf-8");
            }
        }

        private string BytesArrayToHexString(byte[] hash)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
