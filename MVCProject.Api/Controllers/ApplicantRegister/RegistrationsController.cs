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
                CurrentCTC = g.CurrentCTC,
                ExpectedCTC = g.ExpectedCTC,
                NoticePeriod = g.NoticePeriod,
                CurrentLocation = g.CurrentLocation,
                PreferedLocation = g.PreferedLocation,
                ReasonForChange = g.ReasonForChange,
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
            var result = entities.USP_ATS_AllApplicants().Where(x => x.IsActive == true).ToList();
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
                data.EntryBy = "1";
                entities.ATS_ApplicantRegister.AddObject(data);
                this.entities.ATS_ActionHistory.AddObject(new ATS_ActionHistory()
                {
                    ApplicantId = data.ApplicantId,
                    StatusId = 1,
                    Level = 0,
                    IsActive = true,
                    EntryBy= "1",
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
                applicantData.IsActive = data.IsActive;
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
        public ApiResponse FileUpload([FromBody]ATS_Attachment data, int ApplicantId, string databaseName, string directoryPathEnumName = "Attachment_Temp")
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
            int days = 10; // number of days to keep files

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

            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.File));
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
        public ApiResponse GetFileOfApplicant([FromUri]int ApplicantId)
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
    }
}
