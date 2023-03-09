using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.ScheduleManagement.Controllers
{
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
