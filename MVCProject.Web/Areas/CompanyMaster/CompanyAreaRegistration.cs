// -----------------------------------------------------------------------
// <copyright file="JobPostingAreaRegistration.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Areas.CompanyMaster
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Position Area Registration
    /// </summary>
    public class CompanyAreaRegister : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "CompanyMaster";
            }
        }

        /// <summary>
        /// Register Company Area
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            this.RegisterRoutes(context);
            this.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register Company Routes
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        private void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CompanyMaster_default",
                "CompanyMaster/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

        /// <summary>
        /// Bundle Registration Collection
        /// </summary>
        /// <param name="bundles">Bundle Collection</param>
        private void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Company/Company")
                .Include("~/Areas/CompanyMaster/Scripts/angular/services/CompanyService.js")
                .Include("~/Areas/CompanyMaster/Scripts/angular/controllers/CompanyCtrl.js"));

        }
    }
}

