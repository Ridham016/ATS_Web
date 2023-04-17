// -----------------------------------------------------------------------
// <copyright file="JobPostingAreaRegistration.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Areas.JobPostingMaster
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// JobPosting Area Registration
    /// </summary>
    public class JobPostingAreaRegister : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "JobPostingMaster";
            }
        }

        /// <summary>
        /// Register JobPostingMaster Area
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            this.RegisterRoutes(context);
            this.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register JobPostingMaster Routes
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        private void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "JobPostingMaster_default",
                "JobPostingMaster/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

        /// <summary>
        /// Bundle Registration Collection
        /// </summary>
        /// <param name="bundles">Bundle Collection</param>
        private void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/JobPosting/JobPosting")
                .Include("~/Areas/JobPostingMaster/Scripts/angular/services/JobPostingService.js")
                .Include("~/Areas/JobPostingMaster/Scripts/angular/controllers/JobPostingCtrl.js"));

        }
    }
}

