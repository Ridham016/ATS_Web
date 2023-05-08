using MVCProject.Filters;
using MVCProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVCProject.Areas.Dashboard.Controllers
{
    [WebAuthorize(Page = (int)PageAccess.Dashboard)]
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/Dashboard/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Calendar()
        {
            return View();
        }

    }
}
