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
namespace MVCProject.Api.Controllers.Interviewers
{
    public class InterviewersController : BaseController
    {
        private MVCProjectEntities entities;

        public InterviewersController()
        {
            this.entities = new MVCProjectEntities();
        }
        [HttpPost]
        public ApiResponse GetAllInterviewers(PagingParams interviewerDetailParams)
        {
            var interviewerlist = entities.USP_ATS_InterviewerList().ToList();
            //.AsEnumerable()
            // .AsQueryable().OrderByField(interviewerDetailParams.OrderByColumn, interviewerDetailParams.IsAscending)
            // .Skip((interviewerDetailParams.CurrentPageNumber - 1) * interviewerDetailParams.PageSize).Take(interviewerDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, interviewerlist);
        }
        //[HttpPost]
        //public ApiResponse GetAllInterviewers(PagingParams interviewerDetailParams)
        //{
        //    //var interviewerslist = this.entities.Interviewers.Select(d => new
        //    //{
        //    //    InterviewerId = d.InterviewerId,
        //    //    InterviewerName = d.InterviewerName,
        //    //    InterviewerEmail = d.InterviewerEmail,
        //    //    InterviewerPhone = d.InterviewerPhone
        //    //}).ToList();
        //    //return this.Response(MessageTypes.Success, string.Empty, interviewerslist);
        //    var interviewerslist = (from d in this.entities.ATS_Interviewer.AsEnumerable()
        //                        let TotalRecords = this.entities.ATS_Interviewer.AsEnumerable().Count()
        //                        select new
        //                        {
        //                            InterviewerId = d.InterviewerId,
        //                            InterviewerName = d.InterviewerName,
        //                            InterviewerEmail = d.InterviewerEmail,
        //                            InterviewerPhone = d.InterviewerPhone,
        //                            TotalRecords
        //                        }).AsQueryable().Skip((interviewerDetailParams.CurrentPageNumber - 1) * interviewerDetailParams.PageSize).Take(interviewerDetailParams.PageSize);
        //    return this.Response(MessageTypes.Success, string.Empty, interviewerslist);
        //}

        [HttpGet]
        public ApiResponse GetInterviewerById(int InterviewerId)
        {
            var InterviewerDetail = this.entities.USP_ATS_InterviewerbyId(InterviewerId).Select(d => new
            {
                InterviewerId = d.InterviewerId,
                InterviewerName = d.InterviewerName,
                InterviewerEmail = d.InterviewerEmail,
                InterviewerPhone = d.InterviewerPhone,
                Is_Active = d.Is_Active
            }).SingleOrDefault();
            //.ATS_Interviewer.Where(x => x.InterviewerId == InterviewerId)
            //.Select(d => new
            //{
            //    InterviewerId = d.InterviewerId,
            //    InterviewerName = d.InterviewerName,
            //    InterviewerEmail = d.InterviewerEmail,
            //    InterviewerPhone = d.InterviewerPhone
            //}).SingleOrDefault();
            if (InterviewerDetail != null)
            {
                return this.Response(Utilities.MessageTypes.Success, string.Empty, InterviewerDetail);
            }
            else
            {
                return this.Response(Utilities.MessageTypes.NotFound, string.Empty);
            }
        }

        [HttpPost]
        public ApiResponse Register([FromBody] ATS_Interviewer data)
        {
            var InterviewerData = this.entities.ATS_Interviewer.FirstOrDefault(x => x.InterviewerId == data.InterviewerId);

            if (InterviewerData == null)
            {

                data.EntryDate = DateTime.Now;
                entities.ATS_Interviewer.AddObject(data);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Interviewer));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Interviewer));
            }
            else
            {
                InterviewerData.InterviewerName = data.InterviewerName;
                InterviewerData.InterviewerEmail = data.InterviewerEmail;
                InterviewerData.InterviewerPhone = data.InterviewerPhone;
                InterviewerData.Is_Active = data.Is_Active;
                InterviewerData.UpdateDate = DateTime.Now;

                this.entities.ATS_Interviewer.ApplyCurrentValues(InterviewerData);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.Interviewer);
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Interviewer));
            }
        }
    }
}