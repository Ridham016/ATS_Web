using MVCProject.Api.Models;
using MVCProject.Api.Utilities;
using MVCProject.Api.ViewModel;
using MVCProject.Common.Resources;
using NPOI.HSSF.Record;
#region namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
#endregion

namespace MVCProject.Api.Controllers.CompanyMaster
{
    public class CompanyController : BaseController
    {
        private MVCProjectEntities entities;

        public CompanyController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpPost]
        public ApiResponse Register([FromBody] ATS_CompanyMaster data)
        {
            var companyData = this.entities.ATS_CompanyMaster.FirstOrDefault(x => x.Id == data.Id);
            if (companyData == null)
            {
                data.EntryDate = DateTime.Now;
                data.EntryBy = "1";
                entities.ATS_CompanyMaster.AddObject(data);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Company));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Company));
            }
            else
            {
                companyData.Id = data.Id;
                companyData.CompanyName = data.CompanyName;
                companyData.Venue = data.Venue;
                companyData.ContactPersonName = data.ContactPersonName;
                companyData.IsActive = data.IsActive;
                companyData.EntryDate = DateTime.Now;
                companyData.UpdateDate = DateTime.Now;
                this.entities.ATS_CompanyMaster.ApplyCurrentValues(companyData);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.Company);
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Company));
            }
        }

        [HttpPost]
        public ApiResponse GetAllCompany(PagingParams companyDetailParams)
        {
            if (string.IsNullOrWhiteSpace(companyDetailParams.Search))
            {
                companyDetailParams.Search = string.Empty;
            }
            var result = entities.USP_ATS_AllCompany().Where(x => x.CompanyName.Trim().ToLower().Contains(companyDetailParams.Search.Trim().ToLower())).ToList();
            var TotalRecords = result.Count();
            if (TotalRecords > 0)
            {
                var CompanyDetail = result.Select(g => new
                {
                    Id = g.Id,
                    CompanyName = g.CompanyName,
                    Venue = g.Venue,
                    ContactPersonName = g.ContactPersonName,
                    EntryDate = g.EntryDate,
                    IsActive = g.IsActive,
                    TotalRecords
                }).AsEnumerable()
                .AsQueryable().OrderByField(companyDetailParams.OrderByColumn, companyDetailParams.IsAscending)
                .Skip((companyDetailParams.CurrentPageNumber - 1) * companyDetailParams.PageSize).Take(companyDetailParams.PageSize);
                return this.Response(MessageTypes.Success, string.Empty, CompanyDetail);
            }
            return this.Response(MessageTypes.Error, string.Empty);
        }

        [HttpGet]
        public ApiResponse GetCompanyById(int Id)
        {
            var CompanyDetail = this.entities.USP_ATS_CompanyById(Id).SingleOrDefault();
            if (CompanyDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, CompanyDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

    }
}
