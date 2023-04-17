using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.JobListing.Controllers
{
    public class JobListingController : Controller
    {
        //
        // GET: /JobListing/JobListing/

        public ActionResult Index()
        {
            return View();
        }

    }
}
