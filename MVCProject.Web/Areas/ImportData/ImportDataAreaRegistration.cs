//using System.Web.Mvc;

//namespace MVCProject.Web.Areas.ImportData
//{
//    public class ImportDataAreaRegistration : AreaRegistration
//    {
//        public override string AreaName
//        {
//            get
//            {
//                return "ImportData";
//            }
//        }

//        public override void RegisterArea(AreaRegistrationContext context)
//        {
//            context.MapRoute(
//                "ImportData_default",
//                "ImportData/{controller}/{action}/{id}",
//                new { action = "Index", id = UrlParameter.Optional }
//            );
//        }
//    }
//}

// -----------------------------------------------------------------------
// <copyright file="ApplicantAreaRegistration.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Areas.ImportData
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    public class ImportDataRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "ImportData";
            }
        }
        /// <param name="context">Area Registration Context</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            this.RegisterRoutes(context);
            this.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register ApplicantRegister Routes
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        private void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
               "ImportData_default",
               "ImportData/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional }
           );
        }

        /// <summary>
        /// Bundle Registration Collection
        /// </summary>
        /// <param name="bundles">Bundle Collection</param>
        private void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/ApplicantRegister/Registration")
                .Include("~/Areas/ApplicantRegister/Scripts/angular/services/RegistrationService.js")
                .Include("~/Areas/ApplicantRegister/Scripts/angular/controllers/RegistrationCtrl.js"));

            bundles.Add(new ScriptBundle("~/bundles/ImportData/Import")
               .Include("~/Areas/ImportData/Scripts/angular/services/ImportService.js")
               .Include("~/Areas/ImportData/Scripts/angular/controllers/ImportCtrl.js"));

        }
    }
}




//using System.Web.Mvc;

//namespace MVCProject.Web.Areas.AdvancedSearch
//{
//    public class AdvancedSearchAreaRegistration : AreaRegistration
//    {
//        public override string AreaName
//        {
//            get
//            {
//                return "AdvancedSearch";
//            }
//        }

//        public override void RegisterArea(AreaRegistrationContext context)
//        {
//            context.MapRoute(
//                "AdvancedSearch_default",
//                "AdvancedSearch/{controller}/{action}/{id}",
//                new { action = "Index", id = UrlParameter.Optional }
//            );
//        }
//    }
//}

