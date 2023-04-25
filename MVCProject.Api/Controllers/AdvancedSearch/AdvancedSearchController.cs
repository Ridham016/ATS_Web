using iTextSharp.text.log;
using MVCProject.Api.Handlers;
using MVCProject.Api.Models;
using MVCProject.Api.Models.FilterCriterias;
using MVCProject.Api.Utilities;
using MVCProject.Api.ViewModel;
using MVCProject.Common.Resources;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace MVCProject.Api.Controllers.AdvancedSearch
{
    public class AdvancedSearchController : BaseController
    {
        private MVCProjectEntities entities;

        public AdvancedSearchController()
        {
            this.entities = new MVCProjectEntities();
        }

        [HttpPost]
        public ApiResponse AdvancedActionSearch([FromBody] PagingParams searchDetailParams, [FromUri] SearchParams searchParams)
        {
            if (string.IsNullOrWhiteSpace(searchDetailParams.Search))
            {
                searchDetailParams.Search = string.Empty;
            }
            var advancedsearch = (from g in this.entities.USP_ATS_ActionApplicantSearch(searchParams.StatusId, searchParams.StartDate, searchParams.EndDate).AsEnumerable().Where(x => x.FirstName.Trim().ToLower().Contains(searchDetailParams.Search.Trim().ToLower()))
                                  let TotalRecords = this.entities.USP_ATS_ActionApplicantSearch(searchParams.StatusId, searchParams.StartDate, searchParams.EndDate).AsEnumerable().Where(x => x.FirstName.Trim().ToLower().Contains(searchDetailParams.Search.Trim().ToLower())).Count()
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
                                      StatusId = g.StatusId,
                                      Level = g.Level,
                                      SkillDescription = g.SkillDescription,
                                      PortfolioLink = g.PortfolioLink,
                                      LinkedinLink = g.LinkedinLink,
                                      OtherLink = g.OtherLink,
                                      ExpectedJoiningDate = g.ExpectedJoiningDate,
                                      StatusName = g.StatusName,
                                      Comment = g.Comment,
                                      Reason = g.Reason,
                                      EntryDate = g.EntryDate,
                                      TotalRecords
                                  }).AsEnumerable()
            .Skip((searchDetailParams.CurrentPageNumber - 1) * searchDetailParams.PageSize).Take(searchDetailParams.PageSize);

            return this.Response(MessageTypes.Success, string.Empty, advancedsearch);
        }

        [HttpGet]
        public ApiResponse ApplicantTimeline(int ApplicantId)
        {
            var advancedsearch = this.entities.USP_ATS_ApplicantTimeLine(ApplicantId).Select(g => new
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
                StatusId = g.StatusId,
                StatusName = g.StatusName,
                Level = g.Level,
                Comment = g.Comment,
                SkillDescription = g.SkillDescription,
                PortfolioLink = g.PortfolioLink,
                LinkedinLink = g.LinkedinLink,
                OtherLink = g.OtherLink,
                ExpectedJoiningDate = g.ExpectedJoiningDate,
                PostingId = g.PostingId,
                Reason = g.Reason,
                EntryDate = g.EntryDate,
            }).AsEnumerable();
            return this.Response(MessageTypes.Success, string.Empty, advancedsearch);
        }

        [HttpGet]
        public ApiResponse ApplicantTimeline_APP(int ApplicantId)
        {
            var advancedsearch = this.entities.USP_ATS_ApplicantTimeLine_APP(ApplicantId).Select(g => new
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
                StatusId = g.StatusId,
                StatusName = g.StatusName,
                Level = g.Level,
                SkillDescription = g.SkillDescription,
                PortfolioLink = g.PortfolioLink,
                LinkedinLink = g.LinkedinLink,
                OtherLink = g.OtherLink,
                ExpectedJoiningDate = g.ExpectedJoiningDate,
                PostingId = g.PostingId,
                Comment = g.Comment,
                Reason = g.Reason,
                EntryDate = g.EntryDate,
            }).AsEnumerable();
            return this.Response(MessageTypes.Success, string.Empty, advancedsearch);
        }

        [HttpGet]
        public ApiResponse GetStatus()
        {
            var status = this.entities.USP_ATS_GetStatus().Select(g => new
            {
                g.StatusId,
                g.StatusName
            });
            return this.Response(MessageTypes.Success, string.Empty, status);
        }
        [HttpPost]
        public ApiResponse ExportToXl([FromUri] string[] headers, [FromUri] SearchParams searchParams)
        {
            var result = entities.USP_ATS_ActionApplicantSearch(searchParams.StatusId, searchParams.StartDate, searchParams.EndDate).ToList();
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
                StatusId = g.StatusId,
                Level = g.Level,
                SkillDescription = g.SkillDescription,
                PortfolioLink = g.PortfolioLink,
                LinkedinLink = g.LinkedinLink,
                OtherLink = g.OtherLink,
                ExpectedJoiningDate = g.ExpectedJoiningDate,
                StatusName = g.StatusName,
                Comment = g.Comment,
                Reason = g.Reason,
                EntryDate = g.EntryDate,
                TotalRecords
            }).ToList();
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            IRow headerRow = sheet.CreateRow(0);
            string[] h = headers[0].Split(',');
            for (int i = 0; i < h.Length; i++)
            {
                headerRow.CreateCell(i).SetCellValue(h[i]);
            }
            //AddHeaderRow(sheet);
            int rowNum = 1;

            foreach (var applicant in applicantlist)
            {
                IRow row = sheet.CreateRow(rowNum++);
                row.CreateCell(0).SetCellValue(applicant.ApplicantId);
                row.CreateCell(1).SetCellValue(applicant.FirstName);
                row.CreateCell(2).SetCellValue(applicant.MiddleName);
                row.CreateCell(3).SetCellValue(applicant.LastName);
                row.CreateCell(4).SetCellValue(applicant.Email);
                row.CreateCell(5).SetCellValue(applicant.Phone);
                row.CreateCell(6).SetCellValue(applicant.Address);
                row.CreateCell(7).SetCellValue((DateTime)applicant.DateOfBirth);
                row.CreateCell(8).SetCellValue(applicant.CurrentCompany);
                row.CreateCell(9).SetCellValue(applicant.CurrentDesignation);
                row.CreateCell(10).SetCellValue((DateTime)applicant.ApplicantDate);
                if (applicant.TotalExperience != null)
                {
                    row.CreateCell(11).SetCellValue(applicant.TotalExperience);
                }
                if (applicant.DetailedExperience != null)
                {
                    row.CreateCell(12).SetCellValue(applicant.DetailedExperience);
                }
                if (applicant.CurrentCTC != null)
                {
                    row.CreateCell(13).SetCellValue(applicant.CurrentCTC);
                }
                //row.CreateCell(13).SetCellValue()=;
                if (applicant.ExpectedCTC != null)
                {
                    row.CreateCell(14).SetCellValue(applicant.ExpectedCTC);
                }
                if (applicant.NoticePeriod != null)
                {
                    row.CreateCell(15).SetCellValue(applicant.NoticePeriod);
                }

                row.CreateCell(16).SetCellValue(applicant.ReasonForChange);
                if (applicant.CurrentLocation != null)
                {
                    row.CreateCell(17).SetCellValue(applicant.CurrentLocation);
                }
                row.CreateCell(18).SetCellValue(applicant.PreferedLocation);

                row.CreateCell(19).SetCellValue(applicant.SkillDescription);
                row.CreateCell(20).SetCellValue(applicant.PortfolioLink);
                row.CreateCell(21).SetCellValue(applicant.LinkedinLink);
                row.CreateCell(22).SetCellValue(applicant.OtherLink);
                if (applicant.ExpectedJoiningDate != null)
                {
                    row.CreateCell(23).SetCellValue((DateTime)applicant.ExpectedJoiningDate);
                }
                row.CreateCell(24).SetCellValue(applicant.StatusName);
                row.CreateCell(25).SetCellValue(applicant.Comment);
                row.CreateCell(26).SetCellValue(applicant.Reason);
                row.CreateCell(27).SetCellValue((DateTime)applicant.EntryDate);

            }

            string filePath = HttpContext.Current.Server.MapPath("~/Attachments/Temp/ApplicantSheet.xlsx");
            string fileName = Path.GetFileName("ApplicantSheet.xlsx");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            workbook.Write(fileStream);
            var memorystream = new MemoryStream();
            var byteArray = memorystream.ToArray();
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new ByteArrayContent(byteArray);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachmet")
            {
                FileName = "ApplicantDetaisSheet.xlsx"
            };
            //response.Content.Headers.ContentDisposition.FileName = "ApplicantDetaisSheet.xlsx";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            response.Content.Headers.ContentLength = byteArray.Length;
            Console.WriteLine(response);
            return this.Response(MessageTypes.Success, string.Empty, filePath);
            //return this.Response(MessageTypes.Success, string.Empty, response);
        }
    }
}

