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
            //var applcantlist = this.entities.ApplicantRegisters.Select(g => new
            //{
            //    ApplicantId = g.ApplicantId,
            //    FirstName = g.FirstName,
            //    MiddleName = g.MiddleName,
            //    LastName = g.LastName,
            //    Email = g.Email,
            //    Phone = g.Phone,
            //    Address = g.Address,
            //    DateOfBirth = g.DateOfBirth,
            //    CurrentCompany = g.CurrentCompany,
            //    CurrentDesignation = g.CurrentDesignation,
            //    ApplicantDate = g.ApplicantDate,
            //    TotalExperience = g.TotalExperience,
            //    DetailedExperience = g.DetailedExperience,  
            //    CurrentCTC = g.CurrentCTC,
            //    ExpectedCTC = g.ExpectedCTC,
            //    NoticePeriod = g.NoticePeriod,
            //    CurrentLocation = g.CurrentLocation,
            //    PreferedLocation = g.PreferedLocation,
            //    ReasonForChange = g.ReasonForChange,
            //    IsActive = g.IsActive
            //}).ToList();
            if (string.IsNullOrWhiteSpace(applicantDetailParams.Search))
            {
                applicantDetailParams.Search = string.Empty;
            }

            var applcantlist = (from g in this.entities.ApplicantRegisters.AsEnumerable()
                                   let TotalRecords = this.entities.ApplicantRegisters.AsEnumerable().Count()
                                   select new
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
                                       IsActive = g.IsActive,
                                       TotalRecords
                                   }).AsQueryable().Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, applcantlist);
        }

        [HttpPost]
        public ApiResponse GetApplicantList(PagingParams applicantDetailParams)
        {
            var applcantlist = (from g in this.entities.ApplicantRegisters.Where(x => (x.IsActive.Value)).AsEnumerable()
                                let TotalRecords = this.entities.ApplicantRegisters.Where(x => (x.IsActive.Value)).AsEnumerable().Count()
                                select new
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
                                    IsActive = g.IsActive,
                                    TotalRecords
                                }).AsQueryable().Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, applcantlist);
            //var applcantlist = this.entities.ApplicantRegisters.Where(x => (isGetAll || x.IsActive.Value)).Select(g => new
            //{
            //    ApplicantId = g.ApplicantId,
            //    FirstName = g.FirstName,
            //    MiddleName = g.MiddleName,
            //    LastName = g.LastName,
            //    Email = g.Email,
            //    Phone = g.Phone,
            //    Address = g.Address,
            //    DateOfBirth = g.DateOfBirth,
            //    CurrentCompany = g.CurrentCompany,
            //    CurrentDesignation = g.CurrentDesignation,
            //    ApplicantDate = g.ApplicantDate,
            //    TotalExperience = g.TotalExperience,
            //    DetailedExperience = g.DetailedExperience,
            //    CurrentCTC = g.CurrentCTC,
            //    ExpectedCTC = g.ExpectedCTC,
            //    NoticePeriod = g.NoticePeriod,
            //    CurrentLocation = g.CurrentLocation,
            //    PreferedLocation = g.PreferedLocation,
            //    ReasonForChange = g.ReasonForChange,
            //    IsActive = g.IsActive
            //}).ToList();
            //return this.Response(MessageTypes.Success, string.Empty, applcantlist);
        }

        [HttpGet]
        public ApiResponse GetApplicantById(int ApplicantId)
        {
            var applicantDetail = this.entities.ApplicantRegisters.Where(x => x.ApplicantId== ApplicantId)
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
                    IsActive = g.IsActive
                }).SingleOrDefault();
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
        public ApiResponse Register([FromBody] ApplicantRegister data)
        {
            var applicantData = this.entities.ApplicantRegisters.FirstOrDefault(x => x.ApplicantId == data.ApplicantId);
            if (applicantData == null)
            {
                data.EntryDate = DateTime.Now;
                data.ApplicantDate = DateTime.Now;
                data.DateOfBirth.Value.AddDays(1);
                entities.ApplicantRegisters.AddObject(data);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Applicant));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Applicant));
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
                this.entities.ApplicantRegisters.ApplyCurrentValues(applicantData);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.Applicant);
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Applicant));
            }
        }

        [HttpPost]
        public ApiResponse ListSearchFilter([FromBody]ApplicantRegister data)
        {
            var applcantlist = this.entities.ApplicantRegisters.Where(x => x.PreferedLocation == data.PreferedLocation || x.CurrentLocation == data.CurrentLocation || 
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
        public ApiResponse FileUpload([FromBody]FileUpload data)
        {
            //this.entities.FileUpload.AddObject(new FileUpload()
            //{
            //    FileName = data.FileName,
            //    FilePath= data.FilePath
            //});
            this.entities.FileUpload.AddObject(data);
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Applicant));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Applicant));
        }

        [HttpGet]
        public ApiResponse GetFileUpload()
        {
            var entity = this.entities.FileUpload.ToList();
            return this.Response(Utilities.MessageTypes.Success, string.Empty, entity);
        }
    }
}
