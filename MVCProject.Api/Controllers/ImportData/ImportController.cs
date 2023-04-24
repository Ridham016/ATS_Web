using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using Elmah;
using iTextSharp.text;
using MVCProject.Api.Models;
using MVCProject.Common.Resources;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MVCProject.Api.Controllers.ImportData
{
    public class ImportController : BaseController
    {
        private MVCProjectEntities entities = new MVCProjectEntities();
        [HttpPost]
        //[Route("importData")]
        public ApiResponse ImportData()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count == 0)
                    return this.Response(Utilities.MessageTypes.Error);

                var files = httpRequest.Files[0];
                if (files.ContentType != "application/vnd.ms-excel" && files.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    return this.Response(Utilities.MessageTypes.Error);

                var stream = files.InputStream;
                var workbook = new XSSFWorkbook(stream);
                var sheet = workbook.GetSheetAt(0);
                var validationErrors = new List<string>();

                var list = new List<ATS_ApplicantRegister>();
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null || row.Cells.Count < 27)
                        continue;
                    var FirstName = row.GetCell(0)?.StringCellValue?.Trim();
                    var MiddleName = row.GetCell(1)?.StringCellValue?.Trim();
                    var LastName = row.GetCell(2)?.StringCellValue?.Trim();
                    var Email = row.GetCell(3)?.StringCellValue?.Trim();
                    var Phone = row.GetCell(4)?.NumericCellValue;
                    var Address = row.GetCell(5)?.StringCellValue?.Trim();
                    var DateOfBirth = row.GetCell(6)?.DateCellValue;
                    var ApplicantDate = row.GetCell(7)?.DateCellValue;
                    var CurrentCompany = row.GetCell(8)?.StringCellValue?.Trim();
                    var CurrentDesignation = row.GetCell(9)?.StringCellValue?.Trim();
                    var TotalExperience = row.GetCell(10)?.StringCellValue?.Trim();
                    var DetailedExperience = row.GetCell(11)?.StringCellValue?.Trim();
                    var CurrentCTC = row.GetCell(12)?.StringCellValue?.Trim();
                    var ExpectedCTC = row.GetCell(13)?.StringCellValue?.Trim();
                    var NoticePeriod = row.GetCell(14)?.StringCellValue?.Trim();
                    var ReasonForChange = row.GetCell(15)?.StringCellValue?.Trim();
                    var CurrentLocation = row.GetCell(16)?.StringCellValue?.Trim();
                    var PreferedLocation = row.GetCell(17)?.StringCellValue?.Trim();
                    var IsActive = row.GetCell(18)?.BooleanCellValue;
                    var EntryBy = row.GetCell(19)?.StringCellValue?.Trim();
                    var EntryDate = row.GetCell(20)?.DateCellValue;
                    var UpdatedBy = row.GetCell(21)?.StringCellValue?.Trim();
                    var UpdateDate = row.GetCell(22)?.DateCellValue;
                    var SkillDescription = row.GetCell(23)?.StringCellValue?.Trim();
                    var PortfolioLink = row.GetCell(24)?.StringCellValue?.Trim();
                    var LinkedinLink = row.GetCell(25)?.StringCellValue?.Trim();
                    var OtherLink = row.GetCell(26)?.StringCellValue?.Trim();
                    var comment = row.GetCell(27).StringCellValue?.Trim();

                    string phone = Convert.ToString(Phone);
                    bool isActive = Convert.ToBoolean(IsActive);
                    var applicantlist = new ATS_ApplicantRegister
                    {
                        //ApplicantId = ApplicantId,
                        FirstName = FirstName,
                        MiddleName = MiddleName,
                        LastName = LastName,
                        Email = Email,
                        Phone = phone,
                        Address = Address,
                        DateOfBirth = (DateTime)DateOfBirth,
                        CurrentCompany = CurrentCompany,
                        CurrentDesignation = CurrentDesignation,
                        ApplicantDate = ApplicantDate,
                        TotalExperience = TotalExperience,
                        DetailedExperience = DetailedExperience,
                        CurrentCTC = CurrentCTC,
                        ExpectedCTC = ExpectedCTC,
                        NoticePeriod = NoticePeriod,
                        EntryBy = EntryBy,
                        EntryDate = (DateTime)EntryDate,
                        UpdatedBy = UpdatedBy,
                        UpdateDate = (DateTime)UpdateDate,
                        CurrentLocation = CurrentLocation,
                        PreferedLocation = PreferedLocation,
                        ReasonForChange = ReasonForChange,
                        SkillDescription = SkillDescription,
                        PortfolioLink = PortfolioLink,
                        LinkedinLink = LinkedinLink,
                        OtherLink = OtherLink,
                        Comment = comment,
                        IsActive = isActive,
                    };
                    var validationResult = ValidateApplicant(applicantlist);
                    if (string.IsNullOrEmpty(validationResult))
                    {
                        list.Add((ATS_ApplicantRegister)applicantlist);
                    }
                    else
                    {
                        list.Add((ATS_ApplicantRegister)applicantlist);
                        validationErrors.Add($"{i}: {validationResult}");
                    }
                    //list.Add((ATS_ApplicantRegister)applicantlist);
                }
                var result = new { Data = list, Errors = validationErrors };
                if (validationErrors.Any())
                {
                    //var result = new { Data = list, Errors = validationErrors };
                    return this.Response(Utilities.MessageTypes.Information, string.Join(",", validationErrors), result);
                    //return (string.Join(",", validationErrors));
                }

                return this.Response(Utilities.MessageTypes.Success, "No Validation Errors in Data", result);
                //var validationErrors = new List<string>();
                //foreach (var applicant in list)
                //{
                //    if (applicant.FirstName != null && applicant.FirstName.Any(char.IsDigit))
                //    {
                //        validationErrors.Add("First Name is required.");
                //        //validationErrors.Add(new KeyValuePair<int, string>(applicant.Id, "Name should not contain digits."));
                //    }
                //    if (applicant.LastName != null && applicant.LastName.Any(char.IsDigit))
                //    {
                //        validationErrors.Add("Last Name is required.");
                //    }
                //    if (!string.IsNullOrEmpty(applicant.Email) && !Regex.IsMatch(applicant.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                //    {
                //        validationErrors.Add("Invalid email address.");
                //    }
                //    if (!string.IsNullOrEmpty(applicant.Phone) && !Regex.IsMatch(applicant.Phone, @"^(?:\+?\d{1,3})?[ -]?\(?\d{3}\)?[ -]?\d{3}[ -]?\d{4}$"))
                //    {
                //        validationErrors.Add("Invalid phone number.");
                //    }
                //    if (applicant.DateOfBirth>DateTime.Now && applicant.DateOfBirth>DateTime.Now.AddYears(-18))
                //    {
                //        validationErrors.Add("Invalid DOB.");
                //    }

                //}
                //if (validationErrors.Any())
                //{
                //    //validationErrors.Add($"{i + 1}: {validationResult}");
                //    //List<object> result = new List<object>();
                //    //result.Add(list);
                //    //result.Add(validationErrors);
                //    return this.Response(Utilities.MessageTypes.Error,string.Empty,list);
                //}
                //using (var context = new MVCProjectEntities())
                //{
                //    foreach (var applicant in list)
                //    {
                //        context.ATS_ApplicantRegister.AddObject(applicant);
                //    }
                //    context.SaveChanges();
                //}

                // return new ApiResponse(HttpStatusCode.OK, list);

            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);

                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                return this.Response(Utilities.MessageTypes.Error, exceptionMessage);
            }
            catch (Exception ex)
            {
                return this.Response(Utilities.MessageTypes.Error, "Error occurred while importing data: " + ex.Message);
            }

        }
        private string ValidateApplicant(ATS_ApplicantRegister applicant)
        {
            var validationErrors = new List<string>();
            //foreach (var applicant in list)
            //{
            if (applicant.FirstName != null && applicant.FirstName.Any(char.IsDigit))
            {
                validationErrors.Add("First Name is required.");
                //validationErrors.Add(new KeyValuePair<int, string>(applicant.Id, "Name should not contain digits."));
            }
            if (applicant.LastName != null && applicant.LastName.Any(char.IsDigit))
            {
                validationErrors.Add("Last Name is required.");
            }
            if (!string.IsNullOrEmpty(applicant.Email) && !Regex.IsMatch(applicant.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                validationErrors.Add("Invalid email address.");
            }
            if (!string.IsNullOrEmpty(applicant.Phone) && !Regex.IsMatch(applicant.Phone, @"^(?:\+?\d{1,3})?[ -]?\(?\d{3}\)?[ -]?\d{3}[ -]?\d{4}$"))
            {
                validationErrors.Add("Invalid phone number.");
            }
            if (applicant.DateOfBirth > DateTime.Now && applicant.DateOfBirth > DateTime.Now.AddYears(-18))
            {
                validationErrors.Add("Invalid DOB.");
            }
            return string.Join(",", validationErrors);

            //}

        }

        //[HttpPost]
        //public ApiResponse AddApplicants(string applicantsJson)
        //{
        //    List<ATS_ApplicantRegister> data = JsonConvert.DeserializeObject<List<ATS_ApplicantRegister>>(applicantsJson);
        //    foreach (var applicant in data)
        //    {
        //        applicant.EntryDate = DateTime.Now;
        //        applicant.ApplicantDate = DateTime.Now;
        //        applicant.EntryBy = "1";
        //        entities.ATS_ApplicantRegister.AddObject(new ATS_ApplicantRegister
        //        {
        //            FirstName = applicant.FirstName,
        //            MiddleName = applicant.MiddleName,
        //            LastName = applicant.LastName,
        //            Email = applicant.Email,
        //            Phone = applicant.Phone,
        //            Address = applicant.Address,
        //            DateOfBirth = applicant.DateOfBirth,
        //            CurrentCompany = applicant.CurrentCompany,
        //            CurrentDesignation = applicant.CurrentDesignation,
        //            ApplicantDate = applicant.ApplicantDate,
        //            TotalExperience = applicant.TotalExperience,
        //            DetailedExperience = applicant.DetailedExperience,
        //            CurrentCTC = applicant.CurrentCTC,
        //            ExpectedCTC = applicant.ExpectedCTC,
        //            NoticePeriod = applicant.NoticePeriod,
        //            CurrentLocation = applicant.CurrentLocation,
        //            PreferedLocation = applicant.PreferedLocation,
        //            ReasonForChange = applicant.ReasonForChange,
        //            SkillDescription = applicant.SkillDescription,
        //            PortfolioLink = applicant.PortfolioLink,
        //            LinkedinLink = applicant.LinkedinLink,
        //            OtherLink = applicant.OtherLink,
        //            IsActive = applicant.IsActive,
        //            Comment = applicant.Comment,
        //            EntryBy = "1",
        //            EntryDate = DateTime.Now,
        //            UpdatedBy = "1"
        //        });
        //        this.entities.ATS_ActionHistory.AddObject(new ATS_ActionHistory()
        //        {
        //            ApplicantId = applicant.ApplicantId,
        //            StatusId = 1,
        //            Level = 0,
        //            IsActive = true,
        //            EntryBy = "1",
        //            EntryDate = DateTime.Now
        //        });
        //        if (!(this.entities.SaveChanges() > 0))
        //        {
        //            return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Applicant));
        //        }

        //        return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Applicant), applicant.ApplicantId);

        //    }
        //    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.Success, Resource.Applicant));
        //}

        [HttpGet]
        public HttpResponseMessage GetSample()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var fileStream = new MemoryStream();

            try
            {
                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Sample");

                var properties = typeof(ATS_ApplicantRegister).GetProperties().Where(p => p.Name != "ApplicantId" && p.Name != "ATS_ActionHistory" && p.Name != "ATS_Attachment" && p.Name != "EntityState" && p.Name != "EntityKey");
                var row = sheet.CreateRow(0);
                for (int i = 0; i < properties.Count(); i++)
                {
                    var property = properties.ElementAt(i);
                    row.CreateCell(i).SetCellValue(property.Name);
                }

                // Sample data
                var dataRow = sheet.CreateRow(1);
                for (int i = 0; i < properties.Count(); i++)
                {
                    var property = properties.ElementAt(i);
                    var value = "";
                    if (property.PropertyType == typeof(string))
                    {
                        value = "John";
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        value = "30";
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        value = "True";
                    }
                    dataRow.CreateCell(i).SetCellValue(value);
                }

                workbook.Write(fileStream);
                response.Content = new ByteArrayContent(fileStream.ToArray());
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Sample.xlsx"
                };
            }
            catch (Exception ex)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                fileStream.Close();
            }

            return response;
        }

        //}
        [HttpPost]
        public ApiResponse AddApplicants(List<ATS_ApplicantRegister> data)
        {
            foreach (var applicant in data)
            {
                var applicantData = this.entities.ATS_ApplicantRegister.FirstOrDefault(x => x.ApplicantId == applicant.ApplicantId);
                if (applicantData == null)
                {
                    applicant.EntryDate = DateTime.Now;
                    applicant.ApplicantDate = DateTime.Now;
                    applicant.EntryBy = "1";
                    entities.ATS_ApplicantRegister.AddObject(applicant);
                    this.entities.ATS_ActionHistory.AddObject(new ATS_ActionHistory()
                    {
                        ApplicantId = applicant.ApplicantId,
                        StatusId = 1,
                        Level = 0,
                        IsActive = true,
                        EntryBy = "1",
                        EntryDate = DateTime.Now
                    });
                    if (!(this.entities.SaveChanges() > 0))
                    {
                        return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Applicant));
                    }

                    //return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Applicant), applicant.ApplicantId);
                }

            }
            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.Success, Resource.Applicant));
        }
        //[HttpPost]
        //public ApiResponse AddApplicant(string applicantsJson)
        //{
        //    List<ATS_ApplicantRegister> data = JsonConvert.DeserializeObject<List<ATS_ApplicantRegister>>(applicantsJson);
        //    foreach (var applicant in data)
        //    {
        //        var applicantData = this.entities.ATS_ApplicantRegister.FirstOrDefault(x => x.ApplicantId == applicant.ApplicantId);
        //        if (applicantData == null)
        //        {
        //            applicant.EntryDate = DateTime.Now;
        //            applicant.ApplicantDate = DateTime.Now;
        //            applicant.EntryBy = "1";
        //            entities.ATS_ApplicantRegister.AddObject(applicant);
        //            this.entities.ATS_ActionHistory.AddObject(new ATS_ActionHistory()
        //            {
        //                ApplicantId = applicant.ApplicantId,
        //                StatusId = 1,
        //                Level = 0,
        //                IsActive = true,
        //                EntryBy = "1",
        //                EntryDate = DateTime.Now
        //            });
        //            if (!(this.entities.SaveChanges() > 0))
        //            {
        //                return this.Response(Utilities.MessageTypes.Error, string.Format(Resource.SaveError, Resource.Applicant));
        //            }

        //            return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.CreatedSuccessfully, Resource.Applicant), applicant.ApplicantId);
        //        }

        //    }
        //    return this.Response(Utilities.MessageTypes.Success, string.Format(Resource.Success, Resource.Applicant));
        //}
    }

}

