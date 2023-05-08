using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Filters;
using MVCProject.Utilities;

namespace MVCProject.Areas.PositionMaster.Controllers
{
    [WebAuthorize(Page = (int)PageAccess.Position)]
    public class PositionController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
