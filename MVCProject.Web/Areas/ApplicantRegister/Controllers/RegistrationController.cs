using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.ApplicantRegister.Controllers
{
    public class RegistrationController : Controller
    {
        //
        // GET: /ApplicantRegister/Registration/

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
                FileName = "ApplicantDetaisSheet.xlsx",
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(filedata, contentType);
        }
    }
}
