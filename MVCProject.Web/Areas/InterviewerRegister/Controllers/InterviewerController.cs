using MVCProject.Filters;
using MVCProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MVCProject.Areas.InterviewerRegister.Controllers
{
    [WebAuthorize(Page = (int)PageAccess.InterviewerRegister)]
    public class InterviewerController : Controller
    {
        //
        // GET: /InterviewerRegister/Interviewer/

        public ActionResult Index()
        {
            return View();
        }

    }
}
