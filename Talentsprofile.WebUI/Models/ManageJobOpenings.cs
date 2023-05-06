using System;
using System.Web;
using TalentsProfile.DTO;
using TalentsProfile.BAL.Employer;
using TalentsProfile.DTO.Employers;

namespace Talentsprofile.WebUI.Models
{
    public class ManageJobOpenings
    {
        public JobDescription ManageJobs(JobDescription jobDtl)
        {
            try
            {
                //if (string.IsNullOrEmpty(jobDtl.PublishOn))
                //{
                //    jobDtl.Status = (int)JobStatus.Running;
                //    jobDtl.PublishOn = DateTime.Now.ToShortDateString();
                //}
                //else
                //    jobDtl.Status = (int)JobStatus.Paused;

                //UserTicket tickect = (UserTicket)HttpContext.Current.Session["ticket"];
                //IEmployers employer = new Jobs(tickect);

                //employer.ManageJobs(jobDtl.JobId, string.Empty, string.Empty, jobDtl.JobRefCode, jobDtl.JobTitle, jobDtl.DesignationFor,
                //    jobDtl.MinSalary, jobDtl.MaxSalary, jobDtl.CurrencyType, jobDtl.MinExperience, jobDtl.MaxExperience,
                //    jobDtl.SkillsSet, jobDtl.JobLocation, jobDtl.JobCountry, jobDtl.Description, jobDtl.ContactPerson,
                //    jobDtl.ContactEmail, jobDtl.ContactPhone, jobDtl.WebUrl, jobDtl.Status, jobDtl.PublishOn,
                //    jobDtl.ExpireOn, jobDtl.Education, jobDtl.SalaryType, jobDtl.PublishOn, jobDtl.JobType, jobDtl.IsWalkIn);
            }
            catch (Exception ex)
            {
                // Common.LogError(ex);
            }

            JobDescription job = GetPostedJob(string.Empty);
            return job;
        }


        private JobDescription GetPostedJob(string edit)
        {
            JobDescription job = new JobDescription();
            try
            {
                UserTicket tickect = (UserTicket)HttpContext.Current.Session["ticket"];
                if (!string.IsNullOrEmpty(edit))
                {
                    //IEmployers employer = new Jobs(tickect);
                    //List<JobDetail> jobs = employer.EmpGetJobDetail(long.Parse(edit), tickect.Email, tickect.Tocken);
                    //job = jobs[0];
                }
            }
            catch (Exception ex)
            {
                //Common.LogError(ex);
            }
            return job;
        }
    }
}