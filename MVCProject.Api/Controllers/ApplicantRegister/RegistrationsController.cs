// -----------------------------------------------------------------------
// <copyright file="DesignationsController.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 

namespace MVCProject.Api.Controllers.ApplicantRegister
{
    using MVCProject.Api.Models;
    using MVCProject.Api.Utilities;
    using MVCProject.Common.Resources;
    using NPOI.HSSF.Record;
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    #endregion
    public class RegistrationsController : BaseController
    {
        private MVCProjectEntities entities;

        public RegistrationsController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpGet]
        public ApiResponse GetAllApplicants()
        {
            var applcantlist = this.entities.ApplicantRegisters.Select(g => new
            {
                ApplicantId = g.ApplicantId,
                Name = g.Name,
                Email = g.Email,
                Phone = g.Phone,
                Address = g.Address,
                DateOfBirth = g.DateOfBirth,
                CurrentCompany = g.CurrentCompany,
                CurrentDesignation = g.CurrentDesignation,
                ApplicantDate = g.ApplicantDate,
                IsActive = g.IsActive
            }).ToList();
            return this.Response(MessageTypes.Success, string.Empty, applcantlist);
        }

        [HttpGet]
        public ApiResponse GetApplicantList(bool isGetAll = false)
        {
            var applcantlist = this.entities.ApplicantRegisters.Where(x => (isGetAll || x.IsActive.Value)).Select(g => new
            {
                ApplicantId = g.ApplicantId,
                Name = g.Name,
                Email = g.Email,
                Phone = g.Phone,
                Address = g.Address,
                DateOfBirth = g.DateOfBirth,
                CurrentCompany = g.CurrentCompany,
                CurrentDesignation = g.CurrentDesignation,
                ApplicantDate = g.ApplicantDate,
                IsActive = g.IsActive
            }).ToList();
            return this.Response(MessageTypes.Success, string.Empty, applcantlist);
        }

        [HttpGet]
        public ApiResponse GetApplicantById(int ApplicantId)
        {
            var applicantDetail = this.entities.ApplicantRegisters.Where(x => x.ApplicantId== ApplicantId)
                .Select(g => new
                {
                    ApplicantId = g.ApplicantId,
                    Name = g.Name,
                    Email = g.Email,
                    Phone = g.Phone,
                    Address = g.Address,
                    DateOfBirth = g.DateOfBirth,
                    CurrentCompany = g.CurrentCompany,
                    CurrentDesignation = g.CurrentDesignation,
                    ApplicantDate = g.ApplicantDate,
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
                entities.ApplicantRegisters.AddObject(data);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Applicant));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Applicant));
            }
            else
            {
                applicantData.Name = data.Name;
                applicantData.Email = data.Email;
                applicantData.Phone = data.Phone;
                applicantData.Address = data.Address;
                applicantData.DateOfBirth = data.DateOfBirth;
                applicantData.CurrentCompany = data.CurrentCompany;
                applicantData.CurrentDesignation = data.CurrentDesignation;
                applicantData.ApplicantDate = data.ApplicantDate;
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
    }
}
