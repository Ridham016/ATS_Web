﻿// -----------------------------------------------------------------------
// <copyright file="SchedulesController.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Api.Controllers.ScheduleManagement
{
    using iTextSharp.text.log;
    //using MailKit.Net.Smtp;
    //using MailKit.Security;
    //using MimeKit;
    //using MimeKit.Text;
    //using MVCProject.Api.Controllers.Common;
    using MVCProject.Api.Models;
    using MVCProject.Api.Models.FilterCriterias;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    //using Newtonsoft.Json;
    using NPOI.HSSF.Record;
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    //using System.Text;
    using System.Web.Http;
    #endregion
    public class SchedulesController : BaseController
    {
        private MVCProjectEntities entities;

        public SchedulesController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpPost]
        public ApiResponse GetApplicantList(PagingParams applicantDetailParams)
        {
            var result = entities.USP_ATS_ApplicantsList().ToList();
            var TotalRecords = result.Count();
            var applicantlist = result.Select(g => new
            {
                ApplicantId = g.ApplicantId,
                FirstName = g.FirstName,
                MiddleName = g.MiddleName,
                LastName = g.LastName,
                Email = g.Email,
                Phone = g.Phone,
                Address = g.Address,
                DateOfBirth = g.DateOfBirth,
                CurrentCompany = g.CurrentCompany,
                CurrentDesignation = g.CurrentDesignation,
                ApplicantDate = g.ApplicantDate,
                TotalExperience = g.TotalExperience,
                DetailedExperience = g.DetailedExperience,
                CurrentCTC = g.CurrentCTC,
                ExpectedCTC = g.ExpectedCTC,
                NoticePeriod = g.NoticePeriod,
                CurrentLocation = g.CurrentLocation,
                PreferedLocation = g.PreferedLocation,
                ReasonForChange = g.ReasonForChange,
                Level= g.Level,
                SkillDescription = g.SkillDescription,
                PortfolioLink = g.PortfolioLink,
                LinkedinLink = g.LinkedinLink,
                OtherLink = g.OtherLink,
                ExpectedJoiningDate = g.ExpectedJoiningDate,
                EntryDate = g.EntryDate,
                IsActive = g.IsActive,
                StatusId = g.StatusId,
                StatusName = g.StatusName,
                TotalRecords
            }).AsEnumerable()
                .AsQueryable().OrderByField(applicantDetailParams.OrderByColumn, applicantDetailParams.IsAscending)
                .Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, applicantlist);
        }

        [HttpGet]
        public ApiResponse GetApplicant(int ApplicantId)
        {
            var applicantDetail = this.entities.USP_ATS_SingleApplicant(ApplicantId)
                .Select(g => new
                {
                    ApplicantId = g.ApplicantId,
                    FirstName = g.FirstName,
                    MiddleName = g.MiddleName,
                    LastName = g.LastName,
                    Email = g.Email,
                    Phone = g.Phone,
                    Address = g.Address,
                    DateOfBirth = g.DateOfBirth,
                    CurrentCompany = g.CurrentCompany,
                    CurrentDesignation = g.CurrentDesignation,
                    ApplicantDate = g.ApplicantDate,
                    TotalExperience = g.TotalExperience,
                    DetailedExperience = g.DetailedExperience,
                    CurrentCTC = g.CurrentCTC,
                    ExpectedCTC = g.ExpectedCTC,
                    NoticePeriod = g.NoticePeriod,
                    CurrentLocation = g.CurrentLocation,
                    PreferedLocation = g.PreferedLocation,
                    ReasonForChange = g.ReasonForChange,
                    Level = g.Level,
                    SkillDescription = g.SkillDescription,
                    PortfolioLink = g.PortfolioLink,
                    LinkedinLink = g.LinkedinLink,
                    OtherLink = g.OtherLink,
                    ExpectedJoiningDate = g.ExpectedJoiningDate,
                    StatusId = g.StatusId,
                    StatusName = g.StatusName,
                    ReasonId = g.ReasonId,
                    Reason = g.Reason,
                    Comment = g.Comment,
                    FileName = g.FileName
                }).SingleOrDefault();
            //var StatusID = applicantDetail.Select(x => x.StatusId).SingleOrDefault();
            //var button = this.entities.USP_ATS_GetButton(StatusID).ToList();
            //var applicant = applicantDetail.Concat(button).ToList();
            if (applicantDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, applicantDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        [HttpGet]
        public ApiResponse GetButtons(int StatusId)
        {
            var buttons = this.entities.USP_ATS_GetButton(StatusId).ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, buttons);
        }

        [HttpPost]
        public ApiResponse UpdateStatus(int StatusId, int ApplicantId)
        {
            var level = 0;
            var getlevel = entities.USP_ATS_GetLevel(ApplicantId).SingleOrDefault();
            if (StatusId == 4)
            {
                if (getlevel != null)
                {
                    level = (int)getlevel.Level + 1;
                }
                else
                    level++;
            }
            else if (StatusId == 8)
            {
                level = (int)getlevel.Level - 1;
            }
            else
            {
                level = (int)getlevel.Level;
            }
            var addtoaction = new ATS_ActionHistory()
            {
                ApplicantId = ApplicantId,
                StatusId = StatusId,
                IsActive = true,
                Level = level,
                EntryBy = "1",
                EntryDate = DateTime.Now
            };
            entities.ATS_ActionHistory.AddObject(addtoaction);
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.StatusName));
            }
            int[] Result;
            Result = new int[] { StatusId, addtoaction.ActionId };

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.StatusName),Result);
        }
        [HttpGet]
        public ApiResponse GetInterviewers()
        {
            var interviewerslist = (from d in this.entities.USP_ATS_InterviewerList().AsEnumerable()
                                    select new
                                    {
                                        InterviewerId = d.InterviewerId,
                                        InterviewerName = d.InterviewerName,
                                    });
            return this.Response(MessageTypes.Success, string.Empty, interviewerslist);
        }

        [HttpPost]
        public ApiResponse ScheduleInterview([FromBody]ATS_AdditionalInformation data)
        {
            entities.ATS_AdditionalInformation.AddObject(new ATS_AdditionalInformation
            {
                ScheduleDateTime = data.ScheduleDateTime,
                ScheduleLink = data.ScheduleLink,
                ActionId = data.ActionId,
                Description = data.Description,
                InterviewerId = data.InterviewerId,
                PositionId = data.PositionId,
                CompanyId = data.CompanyId,
                Venue = data.Venue,
                Mode = data.Mode,
                IsActive = true,
                EntryBy= "1",
                EntryDate = DateTime.Now
            });
            //entities.ATS_ActionHistory.AddObject(Action);
            //this.entities.ATS_ActionHistory.AddObject(new ATS_ActionHistory()
            //{
            //    ApplicantId = data.ApplicantId,
            //    StatusId = 1,
            //    Level = 0,
            //    IsActive = true,
            //    EntryDate = DateTime.Now
            //});
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Schedule));
            }
            var applicantemail = entities.USP_ATS_GetEmail(data.ActionId).FirstOrDefault();
            string[] emails = { applicantemail.ApplicantEmail };
            var UserDetails = entities.ATS_EmailConfiguration.Where(x => x.Id == 1).FirstOrDefault();
            UserDetails.Email = SecurityUtility.Decrypt(UserDetails.Email);
            UserDetails.Password = SecurityUtility.Decrypt(UserDetails.Password);
            EmailParams emailParams = new EmailParams();
            emailParams.emailIdTO = emails;
            emailParams.subject = "Test Mail";
            emailParams.body = data.Description;
            emailParams.emailIdFrom = UserDetails.Email;
            emailParams.emailPassword = UserDetails.Password;
            emailParams.Host= UserDetails.Host;
            emailParams.Port = UserDetails.Port;
            emailParams.EnableSSL = (bool)UserDetails.EnableSSL;
            bool isSend = ApiHttpUtility.SendMail(emailParams);

            if (!isSend)
            {
                return this.Response(Utilities.MessageTypes.Error, "Error Sending Mail");
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Schedule));
        }

        [HttpGet]
        public ApiResponse GetReasons()
        {
            var reasons = this.entities.USP_ATS_GetOtherReasons().Select(x => new
            {
                ReasonId = x.ReasonId,
                Reason = x.Reason,
                IsActive = x.IsActive
            }).ToList();
            return this.Response(MessageTypes.Success, string.Empty, reasons);
        }

        [HttpGet]
        public ApiResponse GetCompanyDetails()
        {
            var data = this.entities.USP_ATS_GetCompanyDetails().Select(x => new
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                Venue = x.Venue,
                IsActive = x.IsActive
            }).ToList();
            return this.Response(MessageTypes.Success, string.Empty, data);
        }

        [HttpGet]
        public ApiResponse GetPositionDetails()
        {
            var data = this.entities.USP_ATS_GetPositionDetails().Select(x => new
            {
                Id = x.Id,
                PositionName = x.PositionName,
                IsActive = x.IsActive
            }).ToList();
            return this.Response(MessageTypes.Success, string.Empty, data);
        }

        [HttpPost]
        public ApiResponse UpdateReason([FromBody]ReasonParams Reason, [FromUri]int ActionId)
        {
            if(Reason.Reason != null)
            {
                entities.ATS_AdditionalInformation.AddObject(new ATS_AdditionalInformation
                {
                    ActionId = ActionId,
                    Description = Reason.Reason,
                    EntryDate = DateTime.Now,
                    EntryBy = "1",
                    IsActive = true
                });
            }
            if(Reason.ReasonId != null)
            {
                var Action = this.entities.ATS_ActionHistory.FirstOrDefault(x => x.ActionId == ActionId);
                if (Action != null)
                {
                    Action.ReasonId = Reason.ReasonId;
                    entities.ATS_ActionHistory.ApplyCurrentValues(Action);
                }
                else
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.OtherReason));
            }
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.OtherReason));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.OtherReason));

        }

        [HttpPost]
        public ApiResponse Comment([FromBody] ATS_AdditionalInformation data, [FromUri]int ActionId)
        {
            entities.ATS_AdditionalInformation.AddObject(new ATS_AdditionalInformation
            {
                ActionId = ActionId,
                Description = data.Description,
                EntryDate= DateTime.Now,
                EntryBy="1",
                IsActive= true
            });
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.HoldReason));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.HoldReason));
        }

        [HttpPost]
        public ApiResponse GetApplicantsParam([FromBody]PagingParams applicantDetailParams)
        {
            var result = entities.USP_ATS_GetApplicantWithStatus(applicantDetailParams.StatusId).ToList();
            var TotalRecords = result.Count();
            var applicantlist = result.Select(g => new
            {
                ApplicantId = g.ApplicantId,
                FirstName = g.FirstName,
                MiddleName = g.MiddleName,
                LastName = g.LastName,
                Email = g.Email,
                Phone = g.Phone,
                Address = g.Address,
                DateOfBirth = g.DateOfBirth,
                CurrentCompany = g.CurrentCompany,
                CurrentDesignation = g.CurrentDesignation,
                ApplicantDate = g.ApplicantDate,
                TotalExperience = g.TotalExperience,
                DetailedExperience = g.DetailedExperience,
                CurrentCTC = g.CurrentCTC,
                ExpectedCTC = g.ExpectedCTC,
                NoticePeriod = g.NoticePeriod,
                CurrentLocation = g.CurrentLocation,
                PreferedLocation = g.PreferedLocation,
                ReasonForChange = g.ReasonForChange,
                SkillDescription = g.SkillDescription,
                PortfolioLink = g.PortfolioLink,
                LinkedinLink = g.LinkedinLink,
                OtherLink = g.OtherLink,
                ExpectedJoiningDate = g.ExpectedJoiningDate,
                StatusId = g.StatusId,
                IsActive = g.IsActive,
                StatusName = g.StatusName,

                TotalRecords
            }).AsEnumerable()
                .AsQueryable().OrderByField(applicantDetailParams.OrderByColumn, applicantDetailParams.IsAscending)
                .Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, applicantlist);

        }

        [HttpPost]
        public ApiResponse GetEncrptData([FromBody]string ciphertext)
        {
            var encryptData = SecurityUtility.Encrypt(ciphertext);
            var decryptData = SecurityUtility.Decrypt(encryptData);
            var data = new {
                encryptData,
                decryptData
            };
            return this.Response(Utilities.MessageTypes.Success, string.Empty, data);

        }

        [HttpGet]
        public ApiResponse SendEmail()
        {
            EmailParams emailParams = new EmailParams();
            string[] emails = { "dudhatharsh2701@gmail.com" };
            var UserDetails = entities.ATS_EmailConfiguration.Where(x => x.Id == 1).FirstOrDefault();
            UserDetails.Email = SecurityUtility.Decrypt(UserDetails.Email);
            UserDetails.Password = SecurityUtility.Decrypt(UserDetails.Password);
            emailParams.emailIdTO = emails;
            emailParams.subject = "Test Mail";
            emailParams.body = "This is demo mail";
            emailParams.emailIdFrom = UserDetails.Email;
            emailParams.emailPassword = UserDetails.Password;
            emailParams.Host = UserDetails.Host;
            emailParams.Port = UserDetails.Port;
            emailParams.EnableSSL = (bool)UserDetails.EnableSSL;
            bool isSend = ApiHttpUtility.SendMail(emailParams);

            if (!isSend)
            {
                return this.Response(Utilities.MessageTypes.Error, "Error Sending Mail");
            }
            return this.Response(Utilities.MessageTypes.Success, "Error Sending Mail");
        }


    }
}
