using MVCProject.Filters;
using MVCProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVCProject.Areas.CompanyMaster.Controllers
{
    [WebAuthorize(Page = (int)PageAccess.Company)]
    public class CompanyController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
