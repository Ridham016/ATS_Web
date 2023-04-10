using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCProject.Api.Models.Common
{
    public class HeaderRow
    {

        //public string[] ColumnNames { get; set; }
        //public string[] ColumnValues { get; set;}
        public void AddHeaderRow(ISheet sheet)
        {
            IRow headerRow = sheet.CreateRow(0);

            // Define the column headers
            string[] headers = new string[] { "ApplicantId", "FirstName", "MiddleName", "LastName", "Email", "Phone", "Address", "DateOfBirth", "ApplicantDate", "CurrentCompany", "CurrentDesignation", "TotalExperience", "DetailedExperience", "CurrentCTC", "ExpectedCTC", "NoticePeriod", "ReasonForChange", "CurrentLocation", "PreferedLocation", "IsActive", "EntryBy", "EntryDate", "UpdatedBy", "UpdateDate", "SkillDescription", "PortfolioLink", "LinkedinLink", "OtherLink", "Comment" };

            // Add each header to the row
            for (int i = 0; i < headers.Length; i++)
            {
                headerRow.CreateCell(i).SetCellValue(headers[i]);
            }
        }

        public static void AddDataRow(ISheet sheet, int rowIndex, SqlDataReader reader)
        {
            IRow dataRow = sheet.CreateRow(rowIndex);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                object value = reader.GetValue(i);
                if (value != null)
                {
                    dataRow.CreateCell(i).SetCellValue(value.ToString());
                }
            }
        }
    }

}