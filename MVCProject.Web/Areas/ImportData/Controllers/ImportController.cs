using MVCProject.Filters;
using MVCProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVCProject.Web.Areas.ImportData.Controllers
{
    [WebAuthorize(Page = (int)PageAccess.ImportData)]
    public class ImportController : Controller
    {
        //
        // GET: /ImportData/Import/

        public ActionResult Index()
        {
            return View();
        }

    }
}
