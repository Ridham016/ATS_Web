// -----------------------------------------------------------------------
// <copyright file="JobPostingAreaRegistration.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Areas.JobOpenings
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// JobPosting Area Registration
    /// </summary>
    public class JobOpeningsAreaRegister : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "JobOpenings";
            }
        }

        /// <summary>
        /// Register JobOpenings Area
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            this.RegisterRoutes(context);
            this.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register JobOpenings Routes
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        private void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "JobOpenings_default",
                "JobOpenings/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

        /// <summary>
        /// Bundle Registration Collection
        /// </summary>
        /// <param name="bundles">Bundle Collection</param>
        private void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/JobListing/JobListing")
                .Include("~/Areas/JobOpenings/Scripts/angular/services/JobListingService.js")
                .Include("~/Areas/JobOpenings/Scripts/angular/controllers/JobListingCtrl.js"));

        }
    }
}

