using System;
using System.Collections.Generic;
using TalentsProfile.DTO.Employers;

namespace TalentsProfile.BAL.Employer
{
    public interface IEmployers
    {
        bool ManageJobs(string jobId, string email, string tocken, string jobRefCode, string jobTitle, string designation, decimal minSalary, decimal maxSalary,
                            string currencyType, decimal minExprReq, decimal maxExprReq, string reqSkills, string jobLocation, int jobCountry, string jobDesc,
                            string contactPerson, string contactEmail, string contactPhone, string webUrl, short status, string effectFrom, string effectUpto,
                            string education, string salaryType, string postedDate, string jobType, bool walkIn, int orgId);

        List<JobDescription> EmpGetJobDetail(Nullable<Int64> jobId, string tocken = "", string email = "", bool isWalkin = false);
    }


    public interface ITalents
    {
        Dashboard EmpDashboard(string session = "", string token = "");

        List<TalentBasicProfile> ProfileSearched(string session, string token, string keyword, string location, int? minExpr, int? maxExpr, string edu, int? lastUpdatedIn);
    }
}
