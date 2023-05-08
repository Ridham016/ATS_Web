using MVCProject.Filters;
using MVCProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVCProject.Areas.JobOpenings.Controllers
{
    [WebAuthorize(Page = (int)PageAccess.JobOpening)]
    public class JobOpeningsController : Controller
    {
        //
        // GET: /JobListing/JobListing/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Description()
        {
            return View();
        }

    }
}
