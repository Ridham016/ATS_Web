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
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Data.Objects;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.WebPages;
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
            if (string.IsNullOrWhiteSpace(applicantDetailParams.Search))
            {
                applicantDetailParams.Search = string.Empty;
            }
            var result = entities.USP_ATS_AllApplicants().Where(x => x.FirstName.Trim().ToLower().Contains(applicantDetailParams.Search.Trim().ToLower())).ToList();
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
                SkillDescription = g.SkillDescription,
                PortfolioLink = g.PortfolioLink,
                LinkedinLink = g.LinkedinLink,
                OtherLink = g.OtherLink,
                ReasonForChange = g.ReasonForChange,
                ExpectedJoiningDate = g.ExpectedJoiningDate,
                PostingId = g.PostingId,
                FileName = g.FileName,
                FilePath = g.FilePath,
                FileRelativePath = g.FileRelativePath,
                IsActive = g.IsActive,
                TotalRecords
            }).AsEnumerable()
                .AsQueryable().OrderByField(applicantDetailParams.OrderByColumn, applicantDetailParams.IsAscending)
                .Skip((applicantDetailParams.CurrentPageNumber - 1) * applicantDetailParams.PageSize).Take(applicantDetailParams.PageSize);
            return this.Response(MessageTypes.Success, string.Empty, applicantlist);
        }

        [HttpPost]
        public ApiResponse GetApplicantList(PagingParams applicantDetailParams)
        {
            if (string.IsNullOrWhiteSpace(applicantDetailParams.Search))
            {
                applicantDetailParams.Search = string.Empty;
            }
            var result = entities.USP_ATS_AllApplicants().Where(x => x.IsActive == true && x.FirstName.Trim().ToLower().Contains(applicantDetailParams.Search.Trim().ToLower())).ToList();
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
                FileName = g.FileName,
                FilePath = g.FilePath,
                FileRelativePath = g.FileRelativePath,
                IsActive = g.IsActive,
                TotalRecords
            }).AsEnumerable()
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
                data.EntryBy = UserContext.UserId;
                entities.ATS_ApplicantRegister.AddObject(data);
                this.entities.ATS_ActionHistory.AddObject(new ATS_ActionHistory()
                {
                    ApplicantId = data.ApplicantId,
                    StatusId = 1,
                    Level = 0,
                    IsActive = true,
                    EntryBy = UserContext.UserId,
                    EntryDate = DateTime.Now
                });
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Applicant));
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Applicant), data.ApplicantId);
            }
            else
            {
                applicantData.FirstName = data.FirstName;
                applicantData.MiddleName = data.MiddleName;
                applicantData.LastName = data.LastName;
                applicantData.Email = data.Email;
                applicantData.Phone = data.Phone;
                applicantData.Address = data.Address;
                applicantData.DateOfBirth = data.DateOfBirth;
                applicantData.CurrentCompany = data.CurrentCompany;
                applicantData.CurrentDesignation = data.CurrentDesignation;
                applicantData.ApplicantDate = DateTime.Now;
                applicantData.TotalExperience = data.TotalExperience;
                applicantData.DetailedExperience = data.DetailedExperience;
                applicantData.CurrentCTC = data.CurrentCTC;
                applicantData.ExpectedCTC = data.ExpectedCTC;
                applicantData.NoticePeriod = data.NoticePeriod;
                applicantData.CurrentLocation = data.CurrentLocation;
                applicantData.PreferedLocation = data.PreferedLocation;
                applicantData.ReasonForChange = data.ReasonForChange;
                applicantData.SkillDescription = data.SkillDescription;
                applicantData.PortfolioLink = data.PortfolioLink;
                applicantData.LinkedinLink = data.LinkedinLink;
                applicantData.OtherLink = data.OtherLink;
                applicantData.IsActive = data.IsActive;
                applicantData.PostingId = data.PostingId;
                applicantData.ExpectedJoiningDate = data.ExpectedJoiningDate;
                applicantData.UpdatedBy = UserContext.UserId;
                applicantData.UpdateDate = DateTime.Now;
                this.entities.ATS_ApplicantRegister.ApplyCurrentValues(applicantData);
                if (!(this.entities.SaveChanges() > 0))
                {
                    return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError), Resource.Applicant);
                }

                return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.Applicant), data.ApplicantId);
            }
        }

        [HttpGet]
        public ApiResponse GetJobPostingList()
        {
            var data = this.entities.USP_ATS_JobListing(null, null).Select(g => new
            {
                PostingId = g.PostingId,
                CompanyId = g.CompanyId,
                PositionId = g.PositionId,
                CompanyName = g.CompanyName,
                CompanyVenue = g.CompanyVenue,
                PositionName = g.PositionName,
                Experience = g.Experience,
                Salary = g.Salary,
                EntryDate = g.EntryDate,
                Posted = g.Posted,
                IsActive = g.IsActive,
            }).ToList();
            return this.Response(MessageTypes.Success, string.Empty, data);
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
                SkillDescription = g.SkillDescription,
                PortfolioLink = g.PortfolioLink,
                LinkedinLink = g.LinkedinLink,
                OtherLink = g.OtherLink,
                ExpectedJoiningDate = g.ExpectedJoiningDate,
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
        public ApiResponse FileUpload([FromBody] ATS_Attachment data, int ApplicantId, string databaseName, string directoryPathEnumName = "Attachment_Temp")
        {
            string FileURL = string.Empty;
            string directoryPath = string.Empty;
            DirectoryPath enumDirectoryPath = new DirectoryPath();
            if (Enum.IsDefined(typeof(DirectoryPath), directoryPathEnumName))
            {
                Enum.TryParse(directoryPathEnumName, out enumDirectoryPath);
                directoryPath = AppUtility.GetDirectoryPath(enumDirectoryPath, databaseName, false, FileURL);
            }
            File.Copy(Path.Combine(directoryPath, data.FileName), Path.Combine(AppUtility.GetDirectoryPath(DirectoryPath.Attachment, databaseName, false, FileURL), data.FileName), true);
            int days = 10; 

            DateTime thresholdDate = DateTime.Now.AddDays(-days);
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.LastWriteTime < thresholdDate)
                {
                    File.Delete(file);
                }
            }

            string filePath = Path.Combine(AppUtility.GetDirectoryPath(DirectoryPath.Attachment, databaseName, false, FileURL), data.FileName);
            string fileRelativePath = string.Format("{0}{1}", AppUtility.GetDirectoryPath(DirectoryPath.Attachment, databaseName, true, FileURL), data.FileName);
            entities.ATS_Attachment.AddObject(new ATS_Attachment()
            {
                FileName = data.FileName,
                FilePath = filePath,
                FileRelativePath = fileRelativePath,
                OriginalFileName = data.OriginalFileName,
                IsDeleted = false,
                EntryDate = DateTime.UtcNow,
                ApplicantId = ApplicantId,
                AttachmentTypeId = 1
            });
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.File));
            }

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.File),data);
            //var fileData = this.entities.ATS_Attachment.FirstOrDefault(x => x.ApplicantId == ApplicantId);
            //if(fileData == null)
            //{
            //    entities.ATS_Attachment.AddObject(new ATS_Attachment()
            //    {
            //        FileName = data.FileName,
            //        FilePath = filePath,
            //        FileRelativePath = data.FileRelativePath,
            //        OriginalFileName = data.OriginalFileName,
            //        IsDeleted = false,
            //        EntryDate = DateTime.Now,
            //        ApplicantId = ApplicantId,
            //        AttachmentTypeId = 1
            //    });
            //    if (!(this.entities.SaveChanges() > 0))
            //    {
            //        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.File));
            //    }

            //    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.File));
            //}
            //else
            //{
            //    fileData.FileName = data.FileName;
            //    fileData.FilePath = data.FilePath;
            //    fileData.FileRelativePath = data.FileRelativePath;
            //    fileData.OriginalFileName = data.OriginalFileName;
            //    fileData.IsDeleted = false;
            //    fileData.EntryDate = DateTime.Now;
            //    fileData.ApplicantId = ApplicantId;
            //    fileData.AttachmentTypeId = 1;
            //    entities.ATS_Attachment.ApplyCurrentValues(fileData);
            //    if (!(this.entities.SaveChanges() > 0))
            //    {
            //        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.File));
            //    }

            //    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.UpdatedSuccessfully, Resource.File));
            //}
        }

        [HttpGet]
        public ApiResponse GetFileOfApplicant([FromUri] int ApplicantId)
        {
            var applcantlist = entities.USP_ATS_FilesOfApplicant(ApplicantId).Select(g => new
            {
                Id = g.Id,
                FileName = g.FileName,
                FilePath = g.FilePath,
                FileRelativePath = g.FileRelativePath,
                OriginalFileName = g.OriginalFileName,
                IsDeleted = g.IsDeleted,
                EntryDate = g.EntryDate,
                ApplicantId = g.ApplicantId,
                AttachmentTypeId = g.AttachmentTypeId
            });
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
        public ApiResponse DeleteFile([FromUri] int FileId)
        {
            var file = entities.ATS_Attachment.Where(x => x.Id == FileId).FirstOrDefault();
            if (file != null)
            {
                file.IsDeleted = true;
            }
            if (!(this.entities.SaveChanges() > 0))
            {
                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.Delete, Resource.File));
            }
            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.DeletedSuccessfully, Resource.File));
        }

        [HttpGet]
        public ApiResponse ExportToXl([FromUri] string[] headers)
        {
            var result = entities.USP_ATS_AllApplicants().ToList();
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
                CurrentCTC = g.CurrentCTC ?? null,
                ExpectedCTC = g.ExpectedCTC,
                NoticePeriod = g.NoticePeriod,
                CurrentLocation = g.CurrentLocation,
                PreferedLocation = g.PreferedLocation,
                ReasonForChange = g.ReasonForChange,
                FileName = g.FileName,
                SkillDescription = g.SkillDescription,
                PortfolioLink = g.PortfolioLink,
                LinkedinLink = g.LinkedinLink,
                OtherLink = g.OtherLink,
                FilePath = g.FilePath,
                FileRelativePath = g.FileRelativePath,
                IsActive = g.IsActive,
                TotalRecords
            }).ToList();
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");
            IRow headerRow = sheet.CreateRow(0);
            string[] h = headers[0].Split(',');
            ICellStyle headerStyle = workbook.CreateCellStyle();
            headerStyle.FillForegroundColor = IndexedColors.LightOrange.Index;
            headerStyle.FillPattern = FillPattern.SolidForeground;
            headerStyle.BorderBottom = BorderStyle.Medium;
            headerStyle.BottomBorderColor = IndexedColors.Black.Index;
            headerStyle.BorderLeft = BorderStyle.Medium;
            headerStyle.LeftBorderColor = IndexedColors.Black.Index;
            headerStyle.BorderRight = BorderStyle.Medium;
            headerStyle.RightBorderColor = IndexedColors.Black.Index;
            headerStyle.BorderTop = BorderStyle.Medium;
            headerStyle.TopBorderColor = IndexedColors.Black.Index;
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.WrapText = true;
            for (int i = 0; i < h.Length; i++)
            {
                ICell cell = headerRow.CreateCell(i);
                cell.SetCellValue(h[i]);
                cell.CellStyle = headerStyle;
            }
            //AddHeaderRow(sheet);
            int rowNum = 1;

            foreach (var applicant in applicantlist)
            {
                var dob = applicant.DateOfBirth ?? DateTime.MinValue;
                var appldate = applicant.ApplicantDate ?? DateTime.MinValue;
                IRow row = sheet.CreateRow(rowNum++);
                row.CreateCell(0).SetCellValue(applicant.ApplicantId);
                row.CreateCell(1).SetCellValue(applicant.FirstName);
                row.CreateCell(2).SetCellValue(applicant.MiddleName);
                row.CreateCell(3).SetCellValue(applicant.LastName);
                row.CreateCell(4).SetCellValue(applicant.Email);
                row.CreateCell(5).SetCellValue(applicant.Phone);
                row.CreateCell(6).SetCellValue(applicant.Address);
                row.CreateCell(7).SetCellValue(dob.ToString("dd/MM/yyyy"));
                row.CreateCell(8).SetCellValue(applicant.CurrentCompany);
                row.CreateCell(9).SetCellValue(applicant.CurrentDesignation);
                row.CreateCell(10).SetCellValue(appldate.ToString("dd/MM/yyyy"));
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
                if (applicant.IsActive != null)
                {
                    row.CreateCell(19).SetCellValue((bool)applicant.IsActive);
                }
                row.CreateCell(20).SetCellValue(applicant.SkillDescription);
                row.CreateCell(21).SetCellValue(applicant.PortfolioLink);
                row.CreateCell(22).SetCellValue(applicant.LinkedinLink);
                row.CreateCell(23).SetCellValue(applicant.OtherLink);
                row.CreateCell(24).SetCellValue(applicant.FileName);
                row.CreateCell(25).SetCellValue(applicant.FilePath);
                row.CreateCell(26).SetCellValue(applicant.FileRelativePath);
            }
            for (int i = 0; i < 27; i++)
            {
                sheet.AutoSizeColumn(i);
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
        }
    }
}
