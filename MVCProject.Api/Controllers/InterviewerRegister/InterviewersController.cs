using MVCProject.Api.Models;
using MVCProject.Api.Utilities;
using MVCProject.Common.Resources;
using NPOI.HSSF.Record;
#region namespaces
using System;
using System.Collections.Generic;
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

        [HttpGet]
        public ApiResponse GetAllInterviewers()
        {
            var interviewerslist = this.entities.Interviewer.Select(d => new
            {
                InterviewerId = d.InterviewerId,
                InterviewerName = d.InterviewerName,
                InterviewerEmail = d.InterviewerEmail,
                InterviewerPhone = d.InterviewerPhone
            }).ToList();
            return this.Response(MessageTypes.Success, string.Empty, interviewerslist);
        }

        [HttpGet]
        public ApiResponse GetInterviewerById(int InterviewerId)
        {
            var InterviewerDetail = this.entities.Interviewer.Where(x => x.InterviewerId == InterviewerId)
                .Select(d => new
                {
                    InterviewerId = d.InterviewerId,
                    InterviewerName = d.InterviewerName,
                    InterviewerEmail = d.InterviewerEmail,
                    InterviewerPhone = d.InterviewerPhone
                }).SingleOrDefault();
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
        public ApiResponse Register([FromBody] Interviewer data)
        {
            var InterviewerData = this.entities.Interviewer.FirstOrDefault(x => x.InterviewerId == data.InterviewerId);
            if (InterviewerData == null)
            {
                entities.Interviewer.AddObject(data);
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

                this.entities.Interviewer.ApplyCurrentValues(InterviewerData);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.Interviewer);
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Interviewer));
            }
        }
    }
}