using System;
using System.Web;
using System.Web.Mvc;
using TalentsProfile.DTO;
using TalentsProfile.BAL.Admin;
using System.Collections.Generic;
using Talentsprofile.WebUI.Areas.Admin.Models;

namespace Talentsprofile.WebUI.Areas.Admin.Controllers
{
    //[AuthenticateException]
    //[HandleError(ExceptionType = typeof(InvalidOperationException), View = "/adm/login")]
    public class DashboardController : Controller
    {
        string _title = "Talent's Profile";

        [HttpGet]

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("talentsprofile_user");
            HttpCookie ck = new HttpCookie("talentsprofile_user", string.Empty);
            HttpContext.Response.Cookies.Add(ck);
            HttpContext.Request.Cookies.Clear();
            HttpContext.Session.Clear();
            return new RedirectResult("/adm/login");
        }


        [HttpGet]
        [CheckUserSession]
        public ActionResult Index()
        {
            ViewBag.Title = "Welcome recruiter: " + _title;
            return View();
        }


        [HttpPost]
        public JsonResult LoadDashboard()
        {
            //Dashboard db = null;
            //bool status = false;
            //string error = string.Empty;

            //// Logout, if session is null
            //if (Session["ticket"] == null)
            //{
            //    Session.Abandon();
            //    Session.Clear();
            //}
            //else
            //{
            //    try
            //    {
            //        UserTicket tickect = (UserTicket)Session["ticket"];
            //        ITalents employer = new Talents();
            //        db = employer.EmpDashboard(tickect.Email, tickect.Tocken);
            //        status = true;
            //    }
            //    catch (Exception ex) { error = ex.Message + " Source: " + ex.Source; }
            //}

            //return Json(new { Data = db, Status = status, Error = error });
            return Json(new { });
        }


        //[HttpGet]
        //public ActionResult Walkins(string edit)
        //{
        //    // Logout, if session is null
        //    if (Session["ticket"] == null)
        //    {
        //        Session.Abandon();
        //        Session.Clear();
        //        return RedirectToAction("login", "home");
        //    }
        //    else
        //    {
        //        JobDetail job = GetPostedJob(edit);
        //        return View(job);
        //    }
        //}


        //[HttpPost]
        //public ActionResult Walkins(JobDetail jobDtl)
        //{
        //    JobDetail job = ManageJobs(jobDtl);
        //    return View(job);
        //}


        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        public JsonResult LoadPostedJobs(bool jobType)
        {
            bool status = false;
            List<JobDetail> jobs = null;
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
        public JsonResult SearchProfiles(string keyword, string location, Nullable<int> minExpr, Nullable<int> maxExpr, string edu, Nullable<int> lastUpdated)
        {
            //bool status = false;
            //string error = string.Empty;
            //List<TalentBasicProfile> profiles = new List<TalentBasicProfile>();

            //try
            //{
            //    UserTicket tickect = (UserTicket)Session["ticket"];
            //    ITalents employer = new Talents();
            //    profiles = employer.ProfileSearched(HttpContext.Session.SessionID, string.Empty, keyword, location, minExpr, maxExpr, edu, lastUpdated);
            //    status = true;
            //}
            //catch (Exception ex) { Common.LogError(ex); error = "Sorry! Error occured while searching profiles"; }
            //return Json(new { Profiles = profiles, Status = status, Error = error });
            return Json(new { });
        }


        [HttpGet]
        public ActionResult ViewProfile()
        {
            ViewBag.Title = "View profile " + _title;
            return View();
        }


        [HttpPost]
        public JsonResult AddOrganization(string name, string address, string city, string country, string uri, string logoUri,
                                          string functionalArea, string category, string sector, string profile)
        {
            // Validate inputs:
            string err = "Invalid inputs";

            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(city) ||
                    string.IsNullOrEmpty(country) || string.IsNullOrEmpty(functionalArea) ||
                    string.IsNullOrEmpty(category) || string.IsNullOrEmpty(sector) || string.IsNullOrEmpty(profile))
                {
                    return Json(new { Code = 406, Message = err });
                }

                int countryId = 0;
                int functionalAreaId = 0;

                if (!int.TryParse(country, out countryId) || countryId == 0)
                {
                    err = "Country is not in a valid format";
                    return Json(new { Code = 406, Message = err });
                }

                if (!int.TryParse(functionalArea, out functionalAreaId) || functionalAreaId == 0)
                {
                    err = "Functional area is not in a valid format";
                    return Json(new { Code = 406, Message = err });
                }

                // Save data:
                IOrganization org = new Organization(null);
                if (org.AddOrganization("arcmatics@gmail.com", "", name, address, city, countryId, uri, logoUri, functionalAreaId, category, sector, profile))
                    return Json(new { Code = 200, Message = "Organization is saved successfully." });
                else
                    return Json(new { Code = 500, Message = "Seems something went wrong while saving information. Please try again!" });
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return Json(new { Code = 500, Error = err });
            }
        }



        [HttpGet]
        public ActionResult GetPartial()
        {
            return PartialView("components/ViewOrganizations");
        }


        [HttpPost]
        public JsonResult GetOrganization(Int32? id)
        {
            try
            {
                IOrganization org = new Organization(null);
                List<OrganizationDetail> listOrg = org.GetOrganization(id);
                return Json(new { Data = listOrg, Message = "", Code = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { Data = "", Message = ex.Message, Code = 500 });
            }
        }

    }
}
