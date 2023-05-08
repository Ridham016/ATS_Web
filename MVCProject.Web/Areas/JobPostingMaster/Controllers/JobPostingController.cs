using MVCProject.Filters;
using MVCProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVCProject.Areas.JobPostingMaster.Controllers
{
    [WebAuthorize(Page = (int)PageAccess.JobPosting)]
    public class JobPostingController : Controller
    {
        //
        // GET: /JobPosting/JobPosting/

        public ActionResult Index()
        {
            return View();
        }

    }
}
