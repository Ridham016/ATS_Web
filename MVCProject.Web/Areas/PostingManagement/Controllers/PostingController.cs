using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Areas.PostingManagement.Controllers
{
    public class PostingController : Controller
    {
        //
        // GET: /PostingManagement/Posting/

        public ActionResult Index()
        {
            return View();
        }

    }
}
