// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Api.Controllers.Account
{
    using MVCProject.Api.Models;
    using MVCProject.Api.Models.FilterCriterias;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;

    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Web.Http;
    using System.Web.ModelBinding;
    using Newtonsoft.Json;
    using System.Text;
    using System.Web;
    #endregion
    public class AccountController : BaseController
    {
        private MVCProjectEntities entities;

        public AccountController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpPost]
        public ApiResponse Login(ATS_Users users)
        {
            var user = entities.USP_ATS_AuthenticateUser(users.Email,users.Password).SingleOrDefault();
            if (user == null)
            {
                return this.Response(Utilities.MessageTypes.Error,string.Format(Resource.IncorrectCredentials));
            }
            else
            {
                UserContext userContext = new UserContext();
                userContext.UserId = user.UserId;
                userContext.UserName = user.Email;
                userContext.User = user.UserName;
                userContext.RoleId = user.RoleId;
                userContext.Ticks = DateTime.Now.Ticks;
                userContext.Token = SecurityUtility.GetToken(userContext);
                userContext.TimeZoneMinutes = 330;
                return this.Response(MessageTypes.Success,string.Empty, userContext);
            }
        }

    }
}
