using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using TalentsProfile.DTO.Employers;
using Talentsprofile.Admin.Models;

namespace Talentsprofile.Admin.Controllers
{
    public class EmployerController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadPostedJobs(bool jobType)
        {
            bool status = false;
            List<JobDescription> jobs = null;
            string error = string.Empty;

            // Logout, if session is null
            if (Session["ticket"] == null)
            {
                Session.Abandon();
                Session.Clear();
                //return RedirectToAction("login", "home");
            }
            else
            {
                try
                {
                    //UserTicket tickect = (UserTicket)Session["ticket"];
                    //IEmployers employer = new Jobs();
                    //jobs = employer.EmpGetJobDetail(null, tickect.Email, tickect.Tocken, jobType);
                    status = true;
                }
                catch (Exception ex) { error = ex.Message + " Source: " + ex.Source; }
            }

            return Json(new { Jobs = jobs, Status = status, Error = error });
        }


        [HttpPost]
        public ActionResult Index(JobDescription jobDtl)
        {
            ManageJobOpenings jobs = new ManageJobOpenings();
            JobDescription job = jobs.ManageJobs(jobDtl);
            return View(job);
        }


    }
}