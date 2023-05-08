using MVCProject.Filters;
using MVCProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVCProject.Areas.Calendar.Controllers
{
    [WebAuthorize(Page = (int)PageAccess.Calendar)]
    public class CalendarController : Controller
    {
        //
        // GET: /Calendar/Calendar/

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
