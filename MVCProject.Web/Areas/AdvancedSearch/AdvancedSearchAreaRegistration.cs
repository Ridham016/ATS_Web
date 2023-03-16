// -----------------------------------------------------------------------
// <copyright file="ApplicantAreaRegistration.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Web.Areas.AdvancedSearch
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// ApplicantRegister Area Registration
    /// </summary>
    public class AdvancedSearchAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "AdvancedSearch";
            }
        }

        /// <summary>
        /// Register ApplicantRegister Area
        /// </summary>
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
               "AdvancedSearch_default",
               "AdvancedSearch/{controller}/{action}/{id}",
               new { action = "Search", id = UrlParameter.Optional }
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

            bundles.Add(new ScriptBundle("~/bundles/AdvancedSearch/AdvancedSearch")
               .Include("~/Areas/AdvancedSearch/Scripts/angularjs/services/AdvancedSearchService.js")
               .Include("~/Areas/AdvancedSearch/Scripts/angularjs/controllers/AdvancedSearchCtrl.js"));

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
