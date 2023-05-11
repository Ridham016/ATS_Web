// -----------------------------------------------------------------------
// <copyright file="PostingManagementAreaRegister.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Areas.PostingManagement
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// PostingManagement Area Registration
    /// </summary>
    public class PostingManagementAreaRegister : AreaRegistration
    {
        /// <summary>
        /// Gets Area Name
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "PostingManagement";
            }
        }

        /// <summary>
        /// Register PostingManagement Area
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            this.RegisterRoutes(context);
            this.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Register PostingManagement Routes
        /// </summary>
        /// <param name="context">Area Registration Context</param>
        private void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PostingManagement_default",
                "PostingManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

        /// <summary>
        /// Bundle Registration Collection
        /// </summary>
        /// <param name="bundles">Bundle Collection</param>
        private void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/PostingManagement/Posting")
                .Include("~/Areas/PostingManagement/Scripts/angular/services/PostingService.js")
                .Include("~/Areas/PostingManagement/Scripts/angular/controllers/PostingCtrl.js"));

        }
    }
}
