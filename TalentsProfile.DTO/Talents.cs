using System;
using System.Collections.Generic;

namespace TalentsProfile.DTO
{
    /// <summary>
    /// Candidat's personal detail : TO REMOVE
    /// </summary>
    public class PersonalProfile
    {
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Phone { get; set; }
        public String AltNum { get; set; }
        public String Email { get; set; }
        public String BirthDate { get; set; }
        public String Gender { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String PostalCode { get; set; }
        public String Country { get; set; }
        public String LanguagesKnown { get; set; }
        public String MaritalStatus { get; set; }
        public String PassportStatus { get; set; }
        public String VisaStatus { get; set; }
        public String VisaCountry { get; set; }
        public String VisaExpireDate { get; set; }
        public String Photo { get; set; }
        public String CTC { get; set; }
        public String CTCCurrencyType { get; set; }
        public String ECTC { get; set; }
        public String ECTCCurrencyType { get; set; }
        public String NoticePeriod { get; set; }
        public String PrimarySkills { get; set; }
        public String SecondarySkills { get; set; }
        public String SummaryProfile { get; set; }
        public String ResumeTitle { get; set; }
        public String Designation { get; set; }
        public String Education { get; set; }
        public String UpdatedOn { get; set; }
        public String ProfileCompleted { get; set; }
        public String ResumeAttached { get; set; }
    }

    /// <summary>
    /// Jobseeker's personal detail.
    /// </summary>
    public class JobseekerPersonalProfile
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Phone { get; set; }
        public String AltNum { get; set; }
        public String BirthDate { get; set; }
        public String Gender { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String PostalCode { get; set; }
        public String Country { get; set; }
        public String LanguagesKnown { get; set; }
        public String MaritalStatus { get; set; }
        public String PassportStatus { get; set; }
        public String VisaStatus { get; set; }
        public String VisaCountry { get; set; }
        public String VisaExpireDate { get; set; }
    }


    /// <summary>
    /// Jobseeker's summary profile.
    /// </summary>
    public class SummaryProfile
    {
        public string PrimarySkills { get; set; }
        public string SecondarySkills { get; set; }
        public string TotalExperienceYear { get; set; }
        public string TotalExperienceMonth { get; set; }
        public string ProfileSummary { get; set; }
        public string NoticePeriod { get; set; }
    }

    /// <summary>
    /// Jobseeker's salary detail.
    /// </summary>
    public class SalaryDetail
    {
        public String CTC { get; set; }
        public String CTCCurrencyType { get; set; }
        public String ECTC { get; set; }
        public String ECTCCurrencyType { get; set; }
    }

    /// <summary>
    /// Candidate's woked organizational detail.
    /// </summary>
    public class OrganizationalProfile
    {
        public Int64 SrNo { get; set; }
        public String OrganizationName { get; set; }
        //public String MonthFrom { get; set; }
        //public String YearFrom { get; set; }
        //public String MonthTo { get; set; }
        //public String YearTo { get; set; }
        public String DateFrom { get; set; }
        public String ToDate { get; set; }
        public String City { get; set; }
        //public String Location { get; set; }
        public Int32 Country { get; set; }
        public String CountryName { get; set; }
        public String Designation { get; set; }
        //public String Role { get; set; }
        public bool CompanyType { get; set; }
        public String NoticePeriod { get; set; }
    }

    /// <summary>
    /// Candidate's project detail.
    /// </summary>
    public class ProjectDetail
    {
        public Int64 ProjId { get; set; }
        public String Title { get; set; }
        public String Synopsis { get; set; }
        public String Duration { get; set; }
        public String ToolsUsed { get; set; }
        public String RolePlayed { get; set; }
    }


    /// <summary>
    /// Candidate's educational detail.
    /// </summary>
    public class EducationalProfile
    {
        public Int64 SrNo { get; set; }
        public String Degree { get; set; }
        public String Course { get; set; }
        public String PassedYear { get; set; }
        public String InstituteName { get; set; }
        public String University { get; set; }
    }


    /// <summary>
    /// Candidates educational detail.
    /// </summary>
    public class EducationalDetail
    {
        public List<EducationalProfile> UnderGraduation { get; set; }
        public EducationalProfile Graduation { get; set; }
        public List<EducationalProfile> PostGraduation { get; set; }
    }

    /// <summary>
    /// Wrapper class to view candidate's profile.
    /// </summary>
    public class CandidateProfile
    {
        public PersonalProfile PersonalDetail { get; set; }
        public List<OrganizationalProfile> OrganizationalDetail { get; set; }
        public List<ProjectDetail> ProjectsDetail { get; set; }
        public List<EducationalProfile> UnderGraduation { get; set; }
        public EducationalProfile Graduation { get; set; }
        public List<EducationalProfile> PostGraduation { get; set; }
    }

    /// <summary>
    /// Get the profile related job counts.
    /// </summary>
    public class ProfileJobs
    {
        public string Desc { get; set; }
        public string JobsCount { get; set; }
        public string Keywords { get; set; }
    }


    /// <summary>
    /// Class to define talent's profile settings.
    /// </summary>
    [Serializable]
    public class ProfileSettings
    {
        public string UId { get; set; }
        public bool Newsletters { get; set; }
        public bool JobAlert { get; set; }
        public bool JobAlertFromFollowedComp { get; set; }
        public bool SiteChangeAlert { get; set; }
        public bool ArticleNotify { get; set; }
        public bool EventSubscription { get; set; }
        public bool Walkinubscription { get; set; }
    }
}
