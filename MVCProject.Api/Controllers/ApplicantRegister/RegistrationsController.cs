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
        public HttpResponseMessage GetAllApplicants()
        {
            var applcantlist = entities.ApplicantRegisters.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, applcantlist);
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
            if (this.entities.ApplicantRegisters.Any(x => x.ApplicantId != data.ApplicantId && x.Name.Trim() == data.Name.Trim()))
            {
                return this.Response(Utilities.MessageTypes.Warning, string.Format(Resource.AlreadyExists, Resource.ApplicantRegister));
            }
            else
            {
                ApplicantRegister applicantData = this.entities.ApplicantRegisters.FirstOrDefault(x => x.ApplicantId == data.ApplicantId);
                if (applicantData == null)
                {
                    entities.ApplicantRegisters.AddObject(data);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.ApplicantRegister));
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.ApplicantRegister));
                }
                else
                {
                    applicantData.Name = data.Name;
                    applicantData.Email = data.Email;
                    applicantData.Phone = data.Phone;
                    applicantData.Address = data.Address;
                    applicantData.UpdateDate= data.UpdateDate;
                    this.entities.ApplicantRegisters.ApplyCurrentValues(applicantData);
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.ApplicantRegister);
                    }

                    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.ApplicantRegister));
                }
            }
        }
    }
}
