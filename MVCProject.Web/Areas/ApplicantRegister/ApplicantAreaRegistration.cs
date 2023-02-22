// -----------------------------------------------------------------------
// <copyright file="ApplicantAreaRegistration.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Areas.ApplicantRegister
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// ApplicantRegister Area Registration
    /// </summary>
    public class ApplicantAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "ApplicantRegister";
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
                "ApplicantRegister_default",
                "ApplicantRegister/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }

        /// <summary>
        /// Bundle Registration Collection
        /// </summary>
        /// <param name="bundles">Bundle Collection</param>
        private void RegisterBundles(BundleCollection bundles)
        {

            // Application
            bundles.Add(new ScriptBundle("~/bundles/Configuration/Application")
                .Include("~/Scripts/angular/services/SiteMasterService.js")
                .Include("~/Areas/Configuration/Scripts/angular/services/ApplicationService.js")
                .Include("~/Areas/Configuration/Scripts/angular/controllers/ApplicationCtrl.js"));

            // Reset To Login
            bundles.Add(new ScriptBundle("~/bundles/Configuration/ResetToLogin")
                .Include("~/Areas/Configuration/Scripts/angular/services/ResetToLoginService.js")
                .Include("~/Areas/Configuration/Scripts/angular/controllers/ResetToLoginCtrl.js"));

            // Users 
            bundles.Add(new ScriptBundle("~/bundles/Configuration/Users")
                .Include("~/Scripts/angular/services/SiteMasterService.js")
                .Include("~/Scripts/angular/services/DepartmentMasterService.js")
               .Include("~/Areas/Configuration/Scripts/angular/services/UsersService.js")
               .Include("~/Areas/Configuration/Scripts/angular/controllers/UsersCtrl.js"));

            // Role Level
            bundles.Add(new ScriptBundle("~/bundles/Configuration/RoleLevel")
                .Include("~/Areas/Configuration/Scripts/angular/services/RoleService.js")
                .Include("~/Areas/Configuration/Scripts/angular/controllers/RoleLevelCtrl.js"));

            // Role Master
            bundles.Add(new ScriptBundle("~/bundles/Configuration/RoleMaster")
                .Include("~/Areas/Configuration/Scripts/angular/services/RoleService.js")
                .Include("~/Areas/Configuration/Scripts/angular/controllers/RoleMasterCtrl.js"));

            // Designation 
            bundles.Add(new ScriptBundle("~/bundles/ApplicantRegister/Registration")
                .Include("~/Areas/ApplicantRegister/Scripts/angular/services/RegistrationService.js")
                .Include("~/Areas/ApplicantRegister/Scripts/angular/controllers/RegistrationCtrl.js"));

        }
    }
}
