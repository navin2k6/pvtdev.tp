using System;
using System.Web;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using TalentsProfile.DTO.Employers;
using Arcmatics.TalentsProfile.DAL;
using Arcmatics.Authentication;

namespace TalentsProfile.BAL.Employer
{
    public class Jobs : IEmployers
    {
        DataProvider objProvider;
        string _email = string.Empty;
        readonly string _tocken;

        public Jobs(HttpCookie cookie)
        {
            GetUSId usID = new GetUSId();
            _tocken = usID.GetUserTocken();
            string[] user = cookie.Value.Split(',');
            _email = user[1];
            objProvider = new DataProvider();
        }

        /// <summary>
        /// Add/Update job.
        /// </summary>
        /// <param name="jobDtl"></param>
        /// <returns></returns>
        public bool ManageJobs(string jobId, string email, string tocken, string jobRefCode, string jobTitle, string designation, decimal minSalary, decimal maxSalary,
                                      string currencyType, decimal minExprReq, decimal maxExprReq, string reqSkills, string jobLocation, int jobCountry, string jobDesc,
                                      string contactPerson, string contactEmail, string contactPhone, string webUrl, short status, string effectFrom, string effectUpto,
                                      string education, string salaryType, string postedDate, string jobType, bool walkIn, int orgId)
        {
            try
            {
                objProvider = new DataProvider();
                objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                                new object[] { jobId, _email, _tocken, jobRefCode, jobTitle, designation, minSalary, maxSalary,
                                               currencyType, minExprReq, maxExprReq, reqSkills, jobLocation, jobCountry, jobDesc, contactPerson, contactEmail, contactPhone,
                                               webUrl, status, effectFrom, effectUpto, education, salaryType, postedDate, jobType, walkIn, orgId}, ""), string.Empty);
                return true;
            }
            catch (Exception ex) { return false; }
        }



        /// <summary>
        /// Get the selected job detail, and list of jobs in case job ref. id not supplied.
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public List<JobDescription> EmpGetJobDetail(Nullable<Int64> jobId, string tocken = "", string email = "", bool isWalkin = false)
        {
            //if (tickect == null && tickect.Tocken != HttpContext.Current.Session.SessionID)
            //    throw new ArgumentException("INVALID_REQ");

            List<JobDescription> listJobs;
            objProvider = new DataProvider();
            DataTable _tblJobs = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                new object[] { jobId, _tocken, _email, isWalkin }, ""), string.Empty);

            // For selected job, in edit case:
            if (jobId != null)
            {
                listJobs = new List<JobDescription>(from jobs in _tblJobs.AsEnumerable()
                                                    select new JobDescription
                                                    {
                                                        JobId = Convert.ToString(jobs["JobId"]),
                                                        JobRefCode = Convert.ToString(jobs["JobRefCode"]),
                                                        JobTitle = Convert.ToString(jobs["JobTitle"]),
                                                        DesignationFor = Convert.ToString(jobs["Designation"]),
                                                        MinSalary = Convert.ToDecimal(jobs["MinSalary"]),
                                                        MaxSalary = Convert.ToDecimal(jobs["MaxSalary"]),
                                                        CurrencyType = Convert.ToString(jobs["CurrencyType"]),
                                                        MinExperience = Convert.ToInt16(jobs["MinExprReq"]),
                                                        MaxExperience = Convert.ToInt16(jobs["MaxExprReq"]),
                                                        SkillsSet = Convert.ToString(jobs["ReqSkills"]),
                                                        JobLocation = Convert.ToString(jobs["JobLocation"]),
                                                        JobCountry = Convert.ToInt32(jobs["JobCountry"]),
                                                        Description = Convert.ToString(jobs["JobDesc"]),
                                                        Responsiblity = Convert.ToString(jobs["Responsiblity"]),
                                                        ContactPerson = Convert.ToString(jobs["ContactPerson"]),
                                                        ContactEmail = Convert.ToString(jobs["ContactEmail"]),
                                                        ContactPhone = Convert.ToString(jobs["ContactPhone"]),
                                                        WebUrl = Convert.ToString(jobs["WebUrl"]),
                                                        Status = Convert.ToInt16(jobs["Status"]),
                                                        Education = Convert.ToString(jobs["Education"]),
                                                        PublishOn = Convert.ToString(jobs["PostedDate"]),
                                                        EffectFrom = Convert.ToString(jobs["EffectFrom"]),
                                                        ExpireOn = Convert.ToString(jobs["EffectUpto"]),
                                                        UpdatedOn = Convert.ToString(jobs["UpdatedOn"]),
                                                        JobType = Convert.ToString(jobs["JobType"]),
                                                    });
            }

            // For list of posted jobs:
            else
            {
                listJobs = new List<JobDescription>(from jobs in _tblJobs.AsEnumerable()
                                                    select new JobDescription
                                                    {
                                                        JobId = Convert.ToString(jobs["JobId"]),
                                                        JobRefCode = Convert.ToString(jobs["JobRefCode"]),
                                                        JobTitle = Convert.ToString(jobs["JobTitle"]),
                                                        DesignationFor = Convert.ToString(jobs["Designation"]),
                                                        Experience = Convert.ToString(jobs["Experience"]),
                                                        SkillsSet = Convert.ToString(jobs["ReqSkills"]),
                                                        JobLocation = Convert.ToString(jobs["JobLocation"]),
                                                        JobStatus = Convert.ToString(jobs["Status"]),
                                                        PublishOn = Convert.ToString(jobs["PostedDate"]),
                                                        EffectFrom = Convert.ToString(jobs["EffectFrom"]),
                                                        ExpireOn = Convert.ToString(jobs["EffectUpto"]),
                                                        UpdatedOn = Convert.ToString(jobs["UpdatedOn"]),
                                                    });
            }

            return listJobs;
        }

    }



    public class Talents : ITalents
    {
        TalentsProfile.DTO.UserTicket tickect;
        DataProvider objProvider;

        public Talents()
        {
            tickect = (TalentsProfile.DTO.UserTicket)HttpContext.Current.Session["ticket"];
            objProvider = new DataProvider();
        }


        /// <summary>
        /// Get all data to load employer's dashboard.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Dashboard EmpDashboard(string session, string token)
        {
            if (tickect == null || (tickect != null && tickect.Tocken != HttpContext.Current.Session.SessionID))
                throw new ArgumentException("INVALID_REQ");

            Dashboard db = new Dashboard();
            objProvider = new DataProvider();
            DataSet _tbls = objProvider.GetDataSet(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                new object[] { HttpContext.Current.Session.SessionID, tickect.Email }, ""), string.Empty);

            if (_tbls.Tables.Count > 0 && _tbls.Tables[0].Rows.Count > 0)
            {
                db.PostedJobs = _tbls.Tables[0].Rows[0]["TotPost"].ToString();
                db.JobsPaused = _tbls.Tables[0].Rows[0]["Paused"].ToString();
                db.JobsRunning = _tbls.Tables[0].Rows[0]["Running"].ToString();
            }

            if (_tbls.Tables.Count > 1 && _tbls.Tables[1].Rows.Count > 0)
            {
                db.Walkins = _tbls.Tables[1].Rows[0]["TotPost"].ToString();
                db.WalkinThisWeek = _tbls.Tables[1].Rows[0]["ThisWeek"].ToString();
                db.WalkinsUpcomming = _tbls.Tables[1].Rows[0]["Upcomming"].ToString();
            }

            return db;
        }


        /// <summary>
        /// Search talents profile by employers w.r.t. filter options.
        /// </summary>
        /// <returns>List of candidates with basic information.</returns>
        public List<TalentBasicProfile> ProfileSearched(string session, string token, string keyword, string location, int? minExpr, int? maxExpr, string edu, int? lastUpdatedIn)
        {
            if (tickect != null && session != tickect.Tocken)
                throw new ArgumentException("INVALID_REQ");

            DataTable jobsTbl = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                new object[] { tickect.Tocken, tickect.Email, keyword, location, minExpr, maxExpr, edu, lastUpdatedIn }, ""), string.Empty);

            if (jobsTbl == null)
                return null;

            List<TalentBasicProfile> profiles = new List<TalentBasicProfile>(from p in jobsTbl.AsEnumerable()
                                                                             select new TalentBasicProfile
                                                                             {
                                                                                 ProfileId = p["ProfileId"].ToString(),
                                                                                 Name = Convert.ToString(p["Name"]),
                                                                                 City = Convert.ToString(p["City"]),
                                                                                 Photo = Convert.ToString(p["Photo"]),
                                                                                 TotExpr = Convert.ToString(p["TotExpr"]),
                                                                                 PrimarySkills = Convert.ToString(p["PrimarySkills"]),
                                                                                 SecondarySkills = Convert.ToString(p["SecondarySkills"]),
                                                                                 NoticePeriod = Convert.ToString(p["NoticePeriod"]),
                                                                                 LastUpdatedOn = Convert.ToString(p["UpdatedOn"]),
                                                                                 CTC = Convert.ToString(p["CTC"]),
                                                                                 ECTC = Convert.ToString(p["ECTC"]),
                                                                                 Education = Convert.ToString(p["Degree"])
                                                                             });

            return profiles;
        }
    }
}