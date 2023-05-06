using System;
using System.Web;
using System.Web.Mvc;
using TalentsProfile.DTO;
using TalentsProfile.BAL;
using System.Collections.Generic;

namespace Talentsprofile.WebUI.Areas.Search.Controllers
{
    public class JobsController : Controller
    {
        const string _provider = "Talentsprofile.com";
        UserTicket _ticket = null;

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Search job" + _provider;
            return View();
        }


        [HttpPost]
        public JsonResult SearchJob(string keyword, string location, string jobType, int? minExpr, int? maxExpr, string postedBy, int? postedOn)
        {
            maxExpr = (maxExpr == 0) ? null : maxExpr;
            jobType = (jobType.Equals("0")) ? string.Empty : jobType;
            postedBy = (string.IsNullOrEmpty(postedBy)) ? "all" : postedBy;
            string salary = string.Empty;
            bool status = false;
            List<JobSearchResult> jobs = null;

            try
            {
                if (!string.IsNullOrEmpty(keyword))
                    keyword = keyword.Replace('/', ',');

                if (location != null)
                    location = location.Replace('/', ',');

                keyword = (keyword.Length > 1) ? keyword.Substring(0, keyword.Length - 1) : "";
                location = (location.Length > 1) ? location.Substring(0, location.Length - 1) : "";

                IDataAccess accessor = new DataAccess(_ticket);
                jobs = accessor.JobSearched(Session.SessionID, string.Empty, keyword, location, jobType, minExpr, maxExpr, salary, postedBy, postedOn);
                status = true;
            }
            catch (Exception ex) { Common.LogError(ex); }
            return Json(new { Jobs = jobs, Status = status });
        }


        [HttpGet]
        public ActionResult Desc(string id, string job)
        {
            ViewBag.Title = string.Format("{0} - {1}", job, _provider);
            return View();
        }


        [HttpPost]
        public JsonResult LoadJobDetail()
        {
            string redirect = string.Empty;
            bool status = false;
            _ticket = new UserTicket();
            JobSearchResult jobDtl = null;

            try
            {
                _ticket.Id = HttpContext.Request.UrlReferrer.Segments[4];
                jobDtl = new JobSearchResult();

                IDataAccess accessor = new DataAccess(_ticket);
                jobDtl = accessor.JobDetail(null);
                status = true;

            }
            catch (Exception ex)
            {
                // Return to search page, if user tries to access page directly without arguments.
                if (ex.InnerException == null)
                    redirect = "/search/jobs/";
                else
                {
                    Common.LogError(ex);
                }
            }

            return Json(new { Jobs = jobDtl, Status = status, Redirect = redirect });
        }


        [HttpPost]
        public JsonResult UserJobSearchHistory(int id)
        {
            string _status = UpdateUserJobSearchHistory(id);
            return Json(new { Status = _status });
        }

        private string UpdateUserJobSearchHistory(int jobId)
        {
            string status = string.Empty;

            try
            {
                HttpCookie ck = HttpContext.Request.Cookies["talentsprofileUser"];
                if (ck == null)
                    return string.Empty;

                byte[] user = Convert.FromBase64String(ck.Value);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(user);
                UserTicket ticket = (UserTicket)formater.Deserialize(ms);

                IDataAccess accessor = new DataAccess(_ticket);
                accessor.JobSearchHistory(long.Parse(ticket.Id), jobId, false, UserAction.Get.ToString().ToUpper());
            }
            catch (Exception ex) { Common.LogError(ex); }
            return status;
        }

    }
}