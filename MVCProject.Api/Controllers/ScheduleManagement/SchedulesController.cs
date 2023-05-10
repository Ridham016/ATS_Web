// -----------------------------------------------------------------------
// <copyright file="SchedulesController.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Api.Controllers.ScheduleManagement
{
    using MVCProject.Api.Models;
    using MVCProject.Api.Models.FilterCriterias;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    //using Newtonsoft.Json;
    #region Namespaces
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
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
                Level = g.Level,
                SkillDescription = g.SkillDescription,
                PortfolioLink = g.PortfolioLink,
                LinkedinLink = g.LinkedinLink,
                OtherLink = g.OtherLink,
                ExpectedJoiningDate = g.ExpectedJoiningDate,
                PostingId = g.PostingId,
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
                    PostingId = g.PostingId,
                    PositionName = g.PositionName,
                    CompanyName = g.CompanyName,
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
        public ApiResponse UpdateStatus(int StatusId, int ApplicantId, int CurrentStatusId, HttpRequestMessage httpRequest)
        {
            var status = this.entities.USP_ATS_SingleApplicant(ApplicantId)
              .Select(g => new
              {
                  StatusId = g.StatusId
              }).SingleOrDefault();
            if (status.StatusId != CurrentStatusId)
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.StatusName));
            }
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
                EntryBy = UserContext.UserId,
                EntryDate = DateTime.Now
            };
            entities.ATS_ActionHistory.AddObject(addtoaction);
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.StatusName));
            }
            int UserId = UserContext.UserId;
            if(StatusId == 2 || StatusId == 7)
            {
                Task.Run(() =>
                {
                    var emailData = this.entities.USP_ATS_SendEmailData(2, addtoaction.ActionId, UserId, null).FirstOrDefault();
                    string htmlBody = emailData.TemplateContent;
                    string subject = emailData.TemplateSubject;
                    EmailParams emailParams = new EmailParams();
                    string[] emails = { emailData.EmailTO };
                    emailData.Email = SecurityUtility.Decrypt(emailData.Email);
                    emailData.Password = SecurityUtility.Decrypt(emailData.Password);
                    emailParams.emailIdTO = emails;
                    emailParams.subject = subject;
                    emailParams.body = htmlBody;
                    emailParams.emailIdFrom = emailData.Email;
                    emailParams.emailPassword = emailData.Password;
                    emailParams.Host = emailData.Host;
                    emailParams.Port = emailData.Port;
                    emailParams.EnableSSL = (bool)emailData.EnableSSL;
                    ApiHttpUtility.SendMail(emailParams);

                });
            }

            int[] Result;
            Result = new int[] { StatusId, addtoaction.ActionId };

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.StatusName), Result);
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
        public ApiResponse ScheduleInterview([FromBody] ATS_AdditionalInformation data)
        {
            entities.ATS_AdditionalInformation.AddObject(new ATS_AdditionalInformation
            {
                ScheduleDateTime = data.ScheduleDateTime,
                ScheduleLink = data.ScheduleLink,
                ActionId = data.ActionId,
                Description = data.Description,
                InterviewerId = data.InterviewerId,
                Venue = data.Venue,
                Mode = data.Mode,
                IsActive = true,
                EntryBy = UserContext.UserId,
                EntryDate = DateTime.Now
            });
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Schedule));
            }
            int UserId = UserContext.UserId;
            Task.Run(() =>
            {
                var emailData = this.entities.USP_ATS_SendEmailData(0, data.ActionId, UserId, data.Mode).FirstOrDefault();
                string htmlBody = emailData.TemplateContent;
                string subject = emailData.TemplateSubject;
                EmailParams emailParams = new EmailParams();
                string[] emails = { emailData.EmailTO};
                emailData.Email = SecurityUtility.Decrypt(emailData.Email);
                emailData.Password = SecurityUtility.Decrypt(emailData.Password);
                emailParams.emailIdTO = emails;
                emailParams.subject = subject;
                emailParams.body = htmlBody;
                emailParams.emailIdFrom = emailData.Email;
                emailParams.emailPassword = emailData.Password;
                emailParams.Host = emailData.Host;
                emailParams.Port = emailData.Port;
                emailParams.EnableSSL = (bool)emailData.EnableSSL;
                ApiHttpUtility.SendMail(emailParams);

                var emailInterviewer = this.entities.USP_ATS_SendEmailData(1, data.ActionId, UserId, data.Mode).FirstOrDefault();
                string htmlBodyInterviewer = emailInterviewer.TemplateContent;
                string subjectInterviewer = emailInterviewer.TemplateSubject;
                EmailParams emailInterviewerParams = new EmailParams();
                string[] emailsInterviewer = { emailInterviewer.EmailTO };
                emailInterviewer.Email = SecurityUtility.Decrypt(emailInterviewer.Email);
                emailInterviewer.Password = SecurityUtility.Decrypt(emailInterviewer.Password);
                emailInterviewerParams.emailIdTO = emailsInterviewer;
                emailInterviewerParams.subject = subjectInterviewer;
                emailInterviewerParams.body = htmlBodyInterviewer;
                emailInterviewerParams.emailIdFrom = emailInterviewer.Email;
                emailInterviewerParams.emailPassword = emailInterviewer.Password;
                emailInterviewerParams.Host = emailInterviewer.Host;
                emailInterviewerParams.Port = emailInterviewer.Port;
                emailInterviewerParams.EnableSSL = (bool)emailInterviewer.EnableSSL;
                ApiHttpUtility.SendMail(emailInterviewerParams);
            });
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
        public ApiResponse GetCompanyDetails([FromUri] int ApplicantId)
        {
            var data = this.entities.USP_ATS_GetCompanyDetailsFromApplicant(ApplicantId).SingleOrDefault();
            if (data != null)
            {
                return this.Response(MessageTypes.Success, string.Empty, data);
            }
            return this.Response(Utilities.MessageTypes.Error, "No Openings Selected");
        }

        [HttpPost]
        public ApiResponse UpdateReason([FromBody] ReasonParams Reason, [FromUri] int ActionId)
        {
            if (Reason.Reason != null)
            {
                entities.ATS_AdditionalInformation.AddObject(new ATS_AdditionalInformation
                {
                    ActionId = ActionId,
                    Description = Reason.Reason,
                    EntryDate = DateTime.Now,
                    EntryBy = UserContext.UserId,
                    IsActive = true
                });
            }
            if (Reason.ReasonId != null)
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
        public ApiResponse Comment([FromBody] ATS_AdditionalInformation data, [FromUri] int ActionId)
        {
            entities.ATS_AdditionalInformation.AddObject(new ATS_AdditionalInformation
            {
                ActionId = ActionId,
                Description = data.Description,
                EntryDate = DateTime.Now,
                EntryBy = UserContext.UserId,
                IsActive = true
            });
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.HoldReason));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.HoldReason));
        }

        [HttpGet]
        public ApiResponse GetCompanyDetails()
        {
            var data = this.entities.USP_ATS_GetCompanyDetails().Select(x => new
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
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
        public ApiResponse GetApplicantsParam([FromBody] PagingParams applicantDetailParams, [FromUri] SearchParams searchParams)
        {
            if (string.IsNullOrWhiteSpace(applicantDetailParams.Search))
            {
                applicantDetailParams.Search = string.Empty;
            }
            var result = entities.USP_ATS_GetApplicantWithStatus(applicantDetailParams.StatusId, searchParams.CompanyId, searchParams.PositionId)
                .Where(x => x.FirstName.Trim().ToLower().Contains(applicantDetailParams.Search.Trim().ToLower())).ToList();
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
                PostingId = g.PostingId,
                StatusId = g.StatusId,
                IsActive = g.IsActive,
                StatusName = g.StatusName,
                TotalRecords
            }).AsEnumerable()
                .AsQueryable()
                .Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, applicantlist);

        }

        [HttpPost]
        public ApiResponse GetEncrptData([FromBody] string ciphertext)
        {
            var encryptData = SecurityUtility.Encrypt(ciphertext);
            var decryptData = SecurityUtility.Decrypt(encryptData);
            var data = new
            {
                encryptData,
                decryptData
            };
            return this.Response(Utilities.MessageTypes.Success, string.Empty, data);

        }

    }
}
