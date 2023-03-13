// -----------------------------------------------------------------------
// <copyright file="DesignationsController.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Api.Controllers.ApplicantRegister
{
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Api.ViewModel;
    using MVCProject.Common.Resources;
    using NPOI.HSSF.Record;
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    #endregion
    public class RegistrationsController : BaseController
    {
        private MVCProjectEntities entities;

        public RegistrationsController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpPost]
        public ApiResponse GetAllApplicants(PagingParams applicantDetailParams)
        {
            var applicantlist = entities.USP_ATS_AllApplicants().AsEnumerable()
                 .AsQueryable().OrderByField(applicantDetailParams.OrderByColumn, applicantDetailParams.IsAscending)
                 .Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, applicantlist);
        }

        [HttpPost]
        public ApiResponse GetApplicantList(PagingParams applicantDetailParams)
        {
            var applicantlist = entities.USP_ATS_ApplicantsList().AsEnumerable()
                .AsQueryable().OrderByField(applicantDetailParams.OrderByColumn, applicantDetailParams.IsAscending)
                .Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, applicantlist);
        }

        [HttpGet]
        public ApiResponse GetApplicantById(int ApplicantId)
        {
            var applicantDetail = this.entities.USP_ATS_ApplicantById(ApplicantId).SingleOrDefault();
            if (applicantDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, applicantDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        [HttpPost]
        public ApiResponse Register([FromBody] ATS_ApplicantRegister data)
        {
            var applicantData = this.entities.ATS_ApplicantRegister.FirstOrDefault(x => x.ApplicantId == data.ApplicantId);
            if (applicantData == null)
            {
                data.EntryDate = DateTime.Now;
                data.ApplicantDate = DateTime.Now;
                data.DateOfBirth.Value.ToLocalTime();
                entities.ATS_ApplicantRegister.AddObject(data);
                this.entities.ATS_ActionHistory.AddObject(new ATS_ActionHistory()
                {
                    ApplicantId = data.ApplicantId,
                    StatusId = 1,
                    Level = 0,
                    IsActive = true,
                    EntryDate = DateTime.Now
                });
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Applicant));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Applicant),data.ApplicantId);
            }
            else
            {
                applicantData.FirstName = data.FirstName;
                applicantData.MiddleName = data.MiddleName;
                applicantData.LastName = data.LastName;
                applicantData.Email = data.Email;
                applicantData.Phone = data.Phone;
                applicantData.Address = data.Address;
                applicantData.DateOfBirth = data.DateOfBirth.Value.AddDays(1);
                applicantData.CurrentCompany = data.CurrentCompany;
                applicantData.CurrentDesignation = data.CurrentDesignation;
                applicantData.ApplicantDate = data.ApplicantDate;
                applicantData.TotalExperience = data.TotalExperience;
                applicantData.DetailedExperience = data.DetailedExperience;
                applicantData.CurrentCTC = data.CurrentCTC;
                applicantData.ExpectedCTC = data.ExpectedCTC;
                applicantData.NoticePeriod = data.NoticePeriod;
                applicantData.CurrentLocation = data.CurrentLocation;
                applicantData.PreferedLocation = data.PreferedLocation;
                applicantData.ReasonForChange = data.ReasonForChange;
                applicantData.IsActive = data.IsActive;
                applicantData.ApplicantDate = DateTime.Now;
                applicantData.UpdateDate = DateTime.Now;
                this.entities.ATS_ApplicantRegister.ApplyCurrentValues(applicantData);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.Applicant);
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Applicant), data.ApplicantId);
            }
        }

        [HttpPost]
        public ApiResponse ListSearchFilter([FromBody] ATS_ApplicantRegister data)
        {
            var applcantlist = this.entities.ATS_ApplicantRegister.Where(x => x.PreferedLocation == data.PreferedLocation || x.CurrentLocation == data.CurrentLocation || 
            (x.PreferedLocation == data.PreferedLocation && x.CurrentLocation == data.CurrentLocation)).Select(g => new
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
                IsActive = g.IsActive
            }).ToList();
            //return this.Response(MessageTypes.Success, string.Empty, applcantlist);

            if (applcantlist != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, applcantlist);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        [HttpPost]
        public ApiResponse FileUpload([FromBody]ATS_Attachment data, int ApplicantId)
        {
            //this.entities.FileUpload.AddObject(new FileUpload()
            //{
            //    FileName = data.FileName,
            //    FilePath= data.FilePath
            //});
            entities.ATS_Attachment.AddObject(new ATS_Attachment()
            {
                FileName = data.FileName,
                FilePath = data.FilePath,
                FileRelativePath = data.FileRelativePath,
                OriginalFileName = data.OriginalFileName,
                IsDeleted = false,
                EntryDate= DateTime.Now,
                ApplicantId = ApplicantId,
                AttachmentTypeId = 1
            });
            //this.entities.ATS_Attachment.AddObject(data);
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Applicant));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Applicant));
        }

        [HttpGet]
        public ApiResponse GetFileUpload()
        {
            var entity = this.entities.ATS_Attachment.ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, entity);
        }
    }
}
