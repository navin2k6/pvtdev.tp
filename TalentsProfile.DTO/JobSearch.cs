using System;

namespace TalentsProfile.DTO
{
    /// <summary>
    /// Search job criterias.
    /// </summary>
    public class JobSearch
    {
        public String Skills { get; set; }

        // List of locations:
        public String Location { get; set; }
        public Boolean IsAdvanceSearch { get; set; }
        public Nullable<int> MinExpr { get; set; }
        public Nullable<int> MaxExpr { get; set; }
        public Nullable<Double> MinSalary { get; set; }
        public Nullable<Double> MaxSalary { get; set; }

        // List of designations:
        public String Designations { get; set; }

        // Organization types: cconsultant/company
        public String OrganizationTypes { get; set; }
        public Nullable<int> DaysOld { get; set; }
    }


    /// <summary>
    /// Result of job search.
    /// </summary>
    public class JobSearchResult
    {
        public String JobId { get; set; }
        public String JobTitle { get; set; }
        public String JobCode { get; set; }
        public String Designation { get; set; }
        public String Experience { get; set; }
        public String SkillsSet { get; set; }
        public String Organization { get; set; }
        public String Location { get; set; }
        public String Country { get; set; }
        public String Description { get; set; }
        public String PostedDate { get; set; }
        public String CompanyLogo { get; set; }
        public String JobType { get; set; }
        public String CompanyProfile { get; set; }
        public String CompUri { get; set; }
    }

    /// <summary>
    /// Detail of a particular job
    /// </summary>
    public class JobDetail : JobSearchResult
    {
        public String CloseDate { get; set; }
        //public String JobType { get; set; }
        public String Salary { get; set; }
        public String Url { get; set; }
        public String Education { get; set; }
        public String Responsibity { get; set; }
        public bool IsWalkin { get; set; }
        public String ContactPerson { get; set; }
        public String ContactEmail { get; set; }
        public String ContactPhone { get; set; }
    }
}
