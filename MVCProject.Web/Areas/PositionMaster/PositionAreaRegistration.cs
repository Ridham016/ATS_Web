// -----------------------------------------------------------------------
// <copyright file="JobPostingAreaRegistration.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Areas.PositionMaster
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Position Area Registration
    /// </summary>
    public class PositionAreaRegister : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "PositionMaster";
            }
        }

        /// <summary>
        /// Register Position Area
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            this.RegisterRoutes(context);
            this.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register Position Routes
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        private void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PositionMaster_default",
                "PositionMaster/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

        /// <summary>
        /// Bundle Registration Collection
        /// </summary>
        /// <param name="bundles">Bundle Collection</param>
        private void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Position/Position")
                .Include("~/Areas/PositionMaster/Scripts/angular/services/PositionService.js")
                .Include("~/Areas/PositionMaster/Scripts/angular/controllers/PositionCtrl.js"));

        }
    }
}

