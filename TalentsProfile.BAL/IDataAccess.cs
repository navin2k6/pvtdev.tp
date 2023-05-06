using System;
using System.Data;
using TalentsProfile.DTO;
using System.Collections.Generic;
using System.Web;
using TalentsProfile.DTO.Employers;

namespace TalentsProfile.BAL
{
    public interface IDataAccess : IDisposable
    {
        SocialUser SocialUserInfo { get; set; }

        string AddMembership(string email, string password, Int16 uType, string captcha, string name, string lastName, char gender, int country, string designation, string companyName);

        bool ForgotPassword(string email);

        UserTicket Login(string email, string pwd, string tocken);

        string RedirectAction(string Session, string Token);

        void CreateProfile(string title, bool isExperienced, decimal totExpr, string session, string token);

        bool ActivateAccount(string userAcc, string userType);

        bool SetAccountPassword(string userAcc, string password);

        void JSPersonalInfo(string Action, string FirstName, string LastName, char Gender, string DoB, string Phone1, string Phone2, string Address, string City,
                            string PostalCode, int Nationality, string LangKnown, int MaritalStatus, int PassportStatus, int VisaStatus, int visaCountry,
                            string Session, string Uid);

        void UpdateSalaryInfo(string ctc, string ctcCurrency, string ectc, string ectcCurrency, string Session, string Uid);

        void UpdateSkills(string primarySkills, string secondarySkills, string Session, string Uid);

        void UpdateSummaryProfile(string profileSummary, decimal totExpr, string Session, string Uid);

        void UpdateGraduationInfo(string degree, string course, string passedYear, string institute, string university, string Session, string Uid);

        void UpdateUnderGraduation(string courseName, string discipline, string passedYear, string school, string board, string courseName10,
                                          string discipline10, string passedYear10, string school10, string board10, string Session, string Uid);

        EducationalDetail UpdatePostGraduationInfo(string degree, string course, string passedYear, string institute, string university,
                                                   string Session, string Uid, Nullable<Int64> pgId);

        CandidateProfile GetProfile(string Session, string Token);

        PersonalProfile GetJSProfile(string Session, string Token);

        SalaryDetail GetJSSalaryInfo(string Session, string Token);

        SummaryProfile GetSummaryProfile(string Session, string Token);

        void ManageProjects(string title, string roles, string synopsis, string tools, string session, string uid, Nullable<Int64> id, string action);

        List<ProjectDetail> GetProjects(string session, string token, Nullable<Int64> id, string action, Nullable<Int64> profileId);

        void DeleteProjects(string session, string token, Nullable<Int64> id, Nullable<Int64> profileId);

        void DeleteOrganization(string session, string token, Nullable<Int64> id, Nullable<Int64> profileId);

        void DeletePGEducation(string session, string token, Nullable<Int64> id, Nullable<Int64> profileId);

        EducationalDetail GetQualification(string session, string token, string action, Nullable<long> eduVal);

        void ManageCompanyWorked(string session, string token, string compName, Nullable<DateTime> fromDate, Nullable<DateTime> toDate, string city, Nullable<int> country, string countryName,
            string designation, int noticePeriod, string compType, string action, Nullable<Int64> id);

        List<OrganizationalProfile> GetOrganizationalInfo(string session, string token, Nullable<Int64> id, string action, Nullable<Int64> profileId);

        List<JobSearchResult> JobSearched(string session, string email, string keyword, string location, string jobType, int? minExpr, int? maxExpr, string salary, string postedBy, int? postedDate);

        string JobSearchHistory(long userId, long jobId, bool isApplied, string action);

        JobSearchResult JobDetail(long? jobId);

        List<ProfileJobs> GetDesiredJobs(string session, string token);

        List<JobSearchResult> GetDesiredJobsList(string desc, string session, string token);

        string ManageProfilePic(string photo, string session, string token);

        HttpPostedFileBase Photo { get; set; }

        CandidateProfile GetProfileDataForPdf(string id);

        string ManageCoverLetter(string session, string token, string letter, string action);

        string ManageSubscription(string email, bool? isSubscribe);

        void ManageResume(byte[] file, string title, string session, string token, string action);

        void ManageResume(HttpPostedFileBase fileObj, string domain, string email, string phone);

        string PostJobApplication(long jobId, string session, string token);

        CompanyProfile GetCompanyProfile(string session, string token, int? profileId);
    }
}
