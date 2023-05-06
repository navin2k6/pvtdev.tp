using System;

namespace TalentsProfile.DTO.Employers
{
    /// <summary>
    /// To add/update job/walk-ins by employer.
    /// </summary>
    public class JobDescription
    {
        public string JobId { get; set; }
        public string JobRefCode { get; set; }
        public string JobTitle { get; set; }
        public string DesignationFor { get; set; }
        public string SkillsSet { get; set; }
        public string Education { get; set; }

        public string JobLocation { get; set; }
        public int JobCountry { get; set; }
        public string Description { get; set; }
        public string Responsiblity { get; set; }
        public string SalaryType { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public string CurrencyType { get; set; }
        public int MinExperience { get; set; }
        public int MaxExperience { get; set; }

        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string WebUrl { get; set; }
        public string PublishOn { get; set; }
        public string ExpireOn { get; set; }
        public short Status { get; set; }
        public string StatusDesc { get; set; }

        public string EffectFrom { get; set; }
        public string UpdatedOn { get; set; }
        public string Experience { get; set; }
        public string JobStatus { get; set; }
        public string JobType { get; set; }
        public bool IsWalkIn { get; set; }

        public int OrganizationId { get; set; }
    }

    /// <summary>
    /// Class is used to load employer's dashboard data.
    /// </summary>
    public class Dashboard
    {
        public string PostedJobs { get; set; }
        public string JobsRunning { get; set; }
        public string JobsPaused { get; set; }

        public string Walkins { get; set; }
        public string WalkinThisWeek { get; set; }
        public string WalkinsUpcomming { get; set; }
    }

    /// <summary>
    /// This class used to list out all talent's basic profile searched by employer.
    /// </summary>
    public class TalentBasicProfile
    {
        public string ProfileId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Photo { get; set; }
        public string TotExpr { get; set; }
        public string NoticePeriod { get; set; }
        public string PrimarySkills { get; set; }
        public string SecondarySkills { get; set; }
        public string CTC { get; set; }
        public string ECTC { get; set; }
        public string LastUpdatedOn { get; set; }
        public string Education { get; set; }
    }


    /// <summary>
    /// Organization's profile type 
    /// </summary>
    public class CompanyProfile
    {
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string AuthorizedPerson { get; set; }
        public string AuthorizedPersonPhone { get; set; }
        public string AuthorizedPersonDesignation { get; set; }

        /// <summary>
        /// Domain(s) of company, in which it does provide services.
        /// </summary>
        public string IndustryType { get; set; }

        /// <summary>
        /// Company Uri.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Designated person's office address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Designated person's office city name.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Designated person's office country.
        /// </summary>
        public string CountryName { get; set; }
        public string CountryId { get; set; }

        /// <summary>
        /// Uri of company logo in case of external resource. 
        /// Or company logo path in case of internal DB.
        /// </summary>
        public string CompanyLogo { get; set; }

        /// <summary>
        /// Short summary profile of company.
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Employee strength of company.
        /// </summary>
        public int? Strength { get; set; }
    }
}
