using System;
using System.Web.Mvc;
using System.Collections.Generic;
using @dto = TalentsProfile.DTO;
using @empDto = TalentsProfile.DTO.Employers;
using TalentsProfile.BAL;
using TalentsProfile.BAL.Employer;
using Talentsprofile.WebUI.Models;

namespace Talentsprofile.WebUI.Areas.Admin.Controllers
{
    public class JobsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(empDto.JobDescription jobDtl)
        {
            ManageJobOpenings jobs = new ManageJobOpenings();
            empDto.JobDescription job = jobs.ManageJobs(jobDtl);
            return View(job);
        }

        [HttpPost]
        public ActionResult LoadPostedJob(bool iswalkins)
        {
            List<empDto.JobDescription> jobs = GetPostedJob(iswalkins);
            return Json(new { Data = jobs, Status = 200, Error = "" });
        }


        [HttpPost]
        public ActionResult PostJob(dto.JobDetail jobDtl)
        {
            empDto.JobDescription job = ManageJobs(jobDtl);
            return View(job);
        }


        #region PVT_METHODS
        private List<empDto.JobDescription> GetPostedJob(bool iswalkins, string editId = null)
        {
            //empDto.JobDescription job = new empDto.JobDescription();
            List<empDto.JobDescription> jobs = null;
            try
            {
                long? jobId = null;
                if (!string.IsNullOrEmpty(editId))
                {
                    jobId = long.Parse(editId);
                }

                IEmployers employer = new Jobs(HttpContext.Request.Cookies["talentsprofile_user"]);
                jobs = employer.EmpGetJobDetail(jobId, string.Empty, string.Empty, false);
                //job = jobs[0];
            }
            catch (Exception ex) { Common.LogError(ex); }
            return jobs;
        }


        private empDto.JobDescription ManageJobs(dto.JobDetail jobDtl)
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

                //UserTicket tickect = (UserTicket)Session["ticket"];
                //IEmployers employer = new Jobs();

                //employer.EmpManagePostJobs(jobDtl.JobId, tickect.Email, tickect.Tocken, jobDtl.JobRefCode, jobDtl.JobTitle, jobDtl.DesignationFor, jobDtl.MinSalary,
                //    jobDtl.MaxSalary, jobDtl.CurrencyType, jobDtl.MinExperience, jobDtl.MaxExperience, jobDtl.SkillsSet, jobDtl.JobLocation, jobDtl.JobCountry,
                //    jobDtl.JobDescription, jobDtl.ContactPerson, jobDtl.ContactEmail, jobDtl.ContactPhone, jobDtl.WebUrl, jobDtl.Status, jobDtl.PublishOn,
                //    jobDtl.ExpireOn, jobDtl.Education, jobDtl.SalaryType, jobDtl.PublishOn, jobDtl.JobType, jobDtl.IsWalkIn);
            }
            catch (Exception ex) { Common.LogError(ex); }
            //empDto.JobDescription job = GetPostedJob(string.Empty);
            //return job;
            return null;
        }


        #endregion PVT_METHODS
    }
}