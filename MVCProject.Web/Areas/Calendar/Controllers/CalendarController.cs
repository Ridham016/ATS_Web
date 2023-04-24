using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.Calendar.Controllers
{
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
