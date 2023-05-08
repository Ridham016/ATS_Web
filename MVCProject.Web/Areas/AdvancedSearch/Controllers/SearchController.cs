using MVCProject.Filters;
using MVCProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVCProject.Areas.AdvancedSearch.Controllers
{
    [WebAuthorize(Page = (int)PageAccess.AdvancedSearch)]
    public class SearchController : Controller
    {
        //
        // GET: /AdvancedSearch/Search/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Download(string filename)
        {
            byte[] filedata = System.IO.File.ReadAllBytes(filename);
            string ext = System.IO.Path.GetExtension(filename);
            string contentType = "application/" + ext;

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = "SearchDetailSheet.xlsx",
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }
    }
}
