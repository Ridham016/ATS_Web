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
using System.Web.UI;
#endregion

namespace MVCProject.Api.Controllers.PositionMaster
{
    public class PositionController : BaseController
    {
        private MVCProjectEntities entities;

        public PositionController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpPost]
        public ApiResponse Register([FromBody] ATS_PositionMaster data)
        {
            var positionData = this.entities.ATS_PositionMaster.FirstOrDefault(x => x.Id == data.Id);
            if (positionData == null)
            {
                data.EntryDate = DateTime.Now;
                data.EntryBy = UserContext.UserId;
                entities.ATS_PositionMaster.AddObject(data);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Position));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Position));
            }
            else
            {
                positionData.Id = data.Id;
                positionData.PositionName = data.PositionName;
                positionData.IsActive = data.IsActive;
                positionData.EntryDate = DateTime.Now;
                positionData.UpdateDate = DateTime.Now;
                positionData.UpdateBy = UserContext.UserId;
                this.entities.ATS_PositionMaster.ApplyCurrentValues(positionData);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.Position);
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Position));
            }
        }

        [HttpPost]
        public ApiResponse GetAllPositions(PagingParams positionDetailParams)
        {
            if (string.IsNullOrWhiteSpace(positionDetailParams.Search))
            {
                positionDetailParams.Search = string.Empty;
            }
            var result = entities.USP_ATS_AllPositions().Where(x => x.PositionName.Trim().ToLower().Contains(positionDetailParams.Search.Trim().ToLower())).ToList();
            var TotalRecords = result.Count();
            if (TotalRecords > 0)
            {
                var PositionDetail = result.Select(g => new
                {
                    Id = g.Id,
                    PositionName = g.PositionName,
                    EntryDate = g.EntryDate,
                    IsActive = g.IsActive,
                    TotalRecords
                }).AsEnumerable()
                .AsQueryable().OrderByField(positionDetailParams.OrderByColumn, positionDetailParams.IsAscending)
                .Skip((positionDetailParams.CurrentPageNumber - 1) * positionDetailParams.PageSize).Take(positionDetailParams.PageSize);
                return this.Response(MessageTypes.Success, string.Empty, PositionDetail);
            }
            return this.Response(MessageTypes.Error, string.Empty);
        }

        [HttpGet]
        public ApiResponse GetPositionById(int Id)
        {
            var PositionDetail = this.entities.USP_ATS_PositionById(Id).SingleOrDefault();
            if (PositionDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, PositionDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

    }
}
