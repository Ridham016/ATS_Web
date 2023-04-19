using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.JobOpenings.Controllers
{
    public class JobOpeningsController : Controller
    {
        //
        // GET: /JobListing/JobListing/

        public ActionResult Index()
        {
            return View();
        }

    }
}
