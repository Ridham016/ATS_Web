using MVCProject.Filters;
using MVCProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVCProject.Areas.ScheduleManagement.Controllers
{
    [WebAuthorize(Page = (int)PageAccess.Schedules)]
    public class ScheduleController : Controller
    {
        //
        // GET: /ScheduleManagement/Schedule/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details()
        {
            return View();
        }

    }
}
