// -----------------------------------------------------------------------
// <copyright file="ApplicantAreaRegistration.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Areas.ScheduleManagement
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// ApplicantRegister Area Registration
    /// </summary>
    public class ScheduleManagementAreaRegister : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "ScheduleManagement";
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
                "ScheduleManagement_default",
                "ScheduleManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

        /// <summary>
        /// Bundle Registration Collection
        /// </summary>
        /// <param name="bundles">Bundle Collection</param>
        private void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/ScheduleManagement/Schedule")
                .Include("~/Areas/ScheduleManagement/Scripts/angular/services/ScheduleService.js")
                .Include("~/Areas/ScheduleManagement/Scripts/angular/controllers/ScheduleCtrl.js"));

        }
    }
}
