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
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using System.IO;
    using System.Web.UI;
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
                var pageAccess = this.entities.USP_ATS_PageAccessByRoleId(user.RoleId).ToList();
                List<UserContext.PagePermission> pagePermissionList = new List<UserContext.PagePermission>();
                foreach(var page in pageAccess)
                {
                    bool canRead = page.CanRead;
                    bool canWrite = page.CanWrite;
                    var pagePermission = new UserContext.PagePermission
                    {
                        PageId = (int)page.PageId,
                        CanRead = canRead,
                        CanWrite = canWrite
                    };
                    pagePermissionList.Add(pagePermission);
                }
                UserContext userContext = new UserContext();
                userContext.UserId = user.UserId;
                userContext.UserName = user.Email;
                userContext.User = user.UserName;
                userContext.RoleId = user.RoleId;
                userContext.Ticks = DateTime.Now.Ticks;
                userContext.PageAccess = pagePermissionList;
                userContext.Token = SecurityUtility.GetToken(userContext);
                userContext.TimeZoneMinutes = 330;
                return this.Response(MessageTypes.Success,string.Empty, userContext);
            }
        }

        [HttpGet]
        public ApiResponse GetUserRoles(int UserId)
        {
            var role = this.entities.USP_ATS_GetUserRoles(UserId).Select(x => new
            {
                x.RoleId,
                x.RoleName
            }).ToList();
            if (role == null)
            {
                return this.Response(MessageTypes.Error, Resource.Error);
            }
            return this.Response(MessageTypes.Success,string.Empty,role);
        }

        [HttpPost]
        public ApiResponse GenerateCode([FromBody] ATS_Users user)
        {
            var emailCheck = this.entities.ATS_Users.Where(x => x.Email == user.Email).SingleOrDefault();
            if (emailCheck == null)
            {
                return this.Response(MessageTypes.Error, Resource.UserNotExists);
            }
            string code = SecurityUtility.GenerateCode();
            if (code == null)
            {
                return this.Response(MessageTypes.Error, Resource.Error);
            }
            Task.Run(() => {
                string file = "Templates/EmailforOtp.html";
                string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
                string htmlBody = File.ReadAllText(filepath);
                htmlBody = htmlBody.Replace("[User Name]", emailCheck.UserName).Replace("[OTP]", code).Replace(" [Email Address]", user.Email);
                string[] emailIdTo = { user.Email };
                EmailParams emailParams = new EmailParams();
                string[] emails = emailIdTo;
                var UserDetails = entities.ATS_EmailConfiguration.Where(x => x.Id == 1).FirstOrDefault();
                UserDetails.Email = SecurityUtility.Decrypt(UserDetails.Email);
                UserDetails.Password = SecurityUtility.Decrypt(UserDetails.Password);
                emailParams.emailIdTO = emails;
                emailParams.subject = "One Time Password (OTP) for Forgot Password recovery";
                emailParams.body = htmlBody;
                emailParams.emailIdFrom = UserDetails.Email;
                emailParams.emailPassword = UserDetails.Password;
                emailParams.Host = UserDetails.Host;
                emailParams.Port = UserDetails.Port;
                emailParams.EnableSSL = (bool)UserDetails.EnableSSL;
                ApiHttpUtility.SendMail(emailParams);
            });
            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.OtpSentSuccessfully),emailCheck.UserId);
        }

        [HttpGet]
        public ApiResponse IsCodeValid(string code)
        {
            bool IsValid = SecurityUtility.IsCodeValid(code);
            if (!IsValid)
            {
                return this.Response(MessageTypes.Error, Resource.InvalidOtp);
            }
            return this.Response(MessageTypes.Success, Resource.ValidOtp);
        }

        [HttpPost]
        public ApiResponse ResetPassword([FromBody]ATS_Users user)
        {
            var userPassword = this.entities.ATS_Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
            if (userPassword != null)
            {
                userPassword.Password = user.Password;
                entities.ATS_Users.ApplyCurrentValues(userPassword);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Password));
                }

                return this.Response(Utilities.MessageTypes.Success, Resource.PasswordResetSuccess);
            }
            return this.Response(MessageTypes.Error, Resource.UserNotExists);
        }


    }
}
