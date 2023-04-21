// -----------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace System.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using MVCProject;
    using MVCProject.ViewModel;
    using System.Web.Routing;
    using MVCProject.Common.Resources;

    /// <summary>
    /// Html Helper Extensions
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Get HTML Menu
        /// </summary>
        /// <param name="helper">Html Helper</param>
        /// <param name="permission">Page Permission</param>
        /// <returns>MVC Html String</returns>
        public static MvcHtmlString Menu(this HtmlHelper helper, UserContext userContext)
        {
            //if (permission != null)
            //{
                StringBuilder menuString = new StringBuilder();
                menuString.Append("<div id='sidebar-menu' class='main_menu_side hidden-print main_menu'>");
                menuString.Append("<div class='menu_section'>");
                menuString.Append("<ul class='nav side-menu'>");


            //        menuString.Append("<li>");
            //        menuString.AppendFormat(GetParentMenu(Resource.Configuration, "fa fa-gears"));
            //        menuString.Append("<ul class='nav child_menu'>");

            //        menuString.Append(GetSubMenu(Resource.Designation, "ti-id-badge", "/Configuration/Designation"));
            //menuString.Append("</ul></li>");


            //menuString.Append("<li>");
            //menuString.AppendFormat(GetParentMenu(Resource.ScheduleManagement, "fa fa-calendar"));
            //menuString.Append("<ul class='nav child_menu'>");
            //menuString.Append(GetSubMenu(Resource.ScheduleManagement, "fa fa-calendar", "/ScheduleManagement/Schedule"));
            //menuString.Append("</ul></li>");
            if(userContext.RoleId == 3)
            {
                menuString.Append(GetMainMenu(Resource.Dashboard, "fa fa-home", "/Dashboard/Dashboard"));
                menuString.Append(GetMainMenu(Resource.ScheduleManagement, "fa fa-calendar", "/ScheduleManagement/Schedule"));
            }
            else if(userContext.RoleId == 1 || userContext.RoleId == 2)
            {
                menuString.Append(GetMainMenu(Resource.Dashboard, "fa fa-home", "/Dashboard/Dashboard"));
                menuString.Append(GetMainMenu(Resource.JobOpenings, "fa fa-list-ul", "/JobOpenings/JobOpenings"));
                menuString.Append(GetMainMenu(Resource.ScheduleManagement, "fa fa-calendar", "/ScheduleManagement/Schedule"));
                menuString.Append(GetMainMenu(Resource.AdvancedSearch, "fa fa-search", "/AdvancedSearch/Search"));
                menuString.Append("<li>");
                menuString.AppendFormat(GetParentMenu(Resource.Register, "fa fa-gears"));
                menuString.Append("<ul class='nav child_menu'>");
                menuString.Append(GetSubMenu(Resource.ApplicantRegister, "fa fa-user-plus", "/ApplicantRegister/Registration"));
                menuString.Append(GetSubMenu(Resource.Interviewer, "fa fa-user-plus", "/InterviewerRegister/Interviewer"));
                menuString.Append(GetSubMenu(Resource.JobPostingRegister, "fa fa-user-plus", "/JobPostingMaster/JobPosting"));
                menuString.Append(GetSubMenu(Resource.PositionRegister, "fa fa-user-plus", "/PositionMaster/Position"));
                menuString.Append(GetSubMenu(Resource.CompanyRegister, "fa fa-user-plus", "/CompanyMaster/Company"));
                menuString.Append("</ul></li>");
            }
            menuString.Append("</ul></div></div>");

                string menu = menuString.ToString();
                return new MvcHtmlString(menu);
            
        }

        /// <summary>
        /// Get Widget Menu
        /// </summary>
        /// <param name="helper">Html Helper</param>
        /// <param name="permission">Page Permission</param>
        /// <returns>Widget Menu String</returns>
        public static MvcHtmlString WidgetMenu(this HtmlHelper helper, List<UserContext.PagePermission> permission)
        {
            StringBuilder menuString = new StringBuilder();

           

            string menu = menuString.ToString();
            return new MvcHtmlString(menu);
        }

        /// <summary>
        /// Get Role Array
        /// </summary>
        /// <param name="helper">Html Helper</param>
        /// <param name="roles">Role List</param>
        /// <returns>MVC Html String</returns>
        public static MvcHtmlString RoleJson(this HtmlHelper helper, List<UserContext.Role> roles)
        {
            string roleString = JsonConvert.SerializeObject(roles);
            return new MvcHtmlString(roleString);
        }

        /// <summary>
        /// Get Main Menu
        /// </summary>
        /// <param name="title">Menu title</param>
        /// <param name="icon">Menu icon</param>
        /// <param name="url">Menu URL</param>
        /// <returns>Menu string</returns>
        private static string GetMainMenu(string title, string icon, string url)
        {
            return string.Format("<li><a href='{2}' title='{0}'><i class='{1}'></i><span class='nav-label'>{0}</span></a></li>", title, icon, url);
        }

        /// <summary>
        /// Get Parent Menu
        /// </summary>
        /// <param name="title">Menu title</param>
        /// <param name="icon">Menu icon</param>
        /// <returns>Menu string</returns>
        private static string GetParentMenu(string title, string icon)
        {
            return string.Format("<a title='{0}'><i class='{1}'></i><span class='nav-label'>{0}</span><span class='fa fa-chevron-right'></span></a>", title, icon);
        }

        /// <summary>
        /// Get Sub Menu
        /// </summary>
        /// <param name="title">Menu title</param>
        /// <param name="icon">Menu icon</param>
        /// <param name="url">Menu URL</param>
        /// <returns>Menu string</returns>
        private static string GetSubMenu(string title, string icon, string url)
        {
            return string.Format("<li><a href='{2}'><i class='{1}'></i><span>{0}</span><div class='c'></div></a><div class='c'></div></li>", title, icon, url);
        }

        /// <summary>
        /// Get Widget Menu
        /// </summary>
        /// <param name="title">Widget Menu title</param>
        /// <param name="id">Widget id</param>
        /// <returns>Widget Menu string</returns>
        private static string GetWidgetMenu(string title, int id)
        {
            return string.Format("<li class='dropdown-header'><a href='#' ng-click='addWidget({1})'>{0}</a></li>", title, id);
        }
    }
}