// -----------------------------------------------------------------------
// <copyright file="ApplicantAreaRegistration.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Areas.Dashboard
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// ApplicantRegister Area Registration
    /// </summary>
    public class DashboardAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "Dashboard";
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
               "Dashboard_default",
               "Dashboard/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional }
           );
        }

        /// <summary>
        /// Bundle Registration Collection
        /// </summary>
        /// <param name="bundles">Bundle Collection</param>
        private void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Dashboard/Dashboard")
                .Include("~/Areas/Dashboard/Scripts/angular/controllers/DashboardCtrl.js")
                .Include("~/Areas/Dashboard/Scripts/angular/services/DashboardService.js"));

        }
    }
}

