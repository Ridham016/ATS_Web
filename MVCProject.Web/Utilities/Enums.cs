// -----------------------------------------------------------------------
// <copyright file="Enums.cs" company="ASK E-Sqaure">
// All copy rights reserved @ASK E-Sqaure.
// </copyright>
// ----------------------------------------------------------------------- 
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName", Justification = "Reviewed.")]

namespace MVCProject.Utilities
{
    /// <summary>
    /// Permission Role level
    /// </summary>
    public enum PermissionLevel : int
    {
        /// <summary>
        /// Admin level
        /// </summary>
        Admin = 1,
       
    }

    /// <summary>
    /// Page Access
    /// </summary>
    public enum PageAccess : int
    {
        /// <summary>
        /// Advanced Search  Page
        /// </summary>
        AdvancedSearch = 1,

        /// <summary>
        /// Applicant Register Page
        /// </summary>
        ApplicantRegister = 2,

        /// <summary>
        /// Calendar Page
        /// </summary>
        Calendar = 3,

        /// <summary>
        /// Company Page
        /// </summary>
        Company = 4,

        /// <summary>
        /// Dashboard Page
        /// </summary>
        Dashboard = 5,

        /// <summary>
        /// ImportData Page
        /// </summary>
        ImportData = 6,

        /// <summary>
        /// InterviewerRegister Page
        /// </summary>
        InterviewerRegister = 7,

        /// <summary>
        /// JobOpening Page
        /// </summary>
        JobOpening = 8,

        /// <summary>
        /// JobPosting Page
        /// </summary>
        JobPosting = 9,

        /// <summary>
        /// Position Page
        /// </summary>
        Position = 10,

        /// <summary>
        /// Schedules Page
        /// </summary>
        Schedules = 11,
        
    }


}