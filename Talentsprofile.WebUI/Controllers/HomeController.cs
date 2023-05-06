using System;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using TalentsProfile; // Includes extension methods.
using TalentsProfile.DTO;
using TalentsProfile.BAL;
using System.Collections;
using System.Configuration;
using Communicator.Notification;
using System.Runtime.Serialization.Formatters.Binary;

namespace Talentsprofile.WebUI.Controllers
{

    public class HomeController : Controller
    {
        UserTicket _ticket = null;

        #region GET_Methods

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "Welcome to Talent's profile";
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Terms()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Services()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Policies()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Faqs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResumeDevelopment()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InterviewTips()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CareerGuide()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Organization(string val)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Feedback()
        {
            return View();
        }

        #endregion GET_Methods


        #region POST_Methods


        [HttpPost]
        public JsonResult JobSearch(string keyword, string location, string jobType, string minExpr, string maxExpr, string salary)
        {
            return Json(keyword + " - " + location);
        }


        [HttpPost]
        public JsonResult PostMessage(FormCollection obj)
        {
            // Assign default result object:
            string _status = HttpStatusCode.NotAcceptable.ToString();
            string _desc = string.Empty;
            int _code = (int)HttpStatusCode.NotAcceptable;
            bool _captcha = false;

            _captcha = CaptchaMvc.HtmlHelpers.CaptchaHelper.IsCaptchaValid(this, "Invalid captcha");

            string name = obj["txtName"];
            string email = obj["txtEmail"];
            string subject = obj["txtSubject"];
            string message = obj["txtMessage"];

            // Validate user inputs:
            if (string.IsNullOrEmpty(name) || (name.Length < 5) || !name.IsAlphabetWithSpace())
            {
                _status = "txtName";
                _desc = "Invalid! Name should be ateleast 5 characters and must not contain special characters except blank space.";
            }

            else if (string.IsNullOrEmpty(email) || !email.IsValidEmail())
            {
                _status = "txtEmail";
                _desc = "Invalid email id.";
            }

            else if (string.IsNullOrEmpty(subject) || (subject.Length < 15))
            {
                _status = "txtSubject";
                _desc = "Invalid! Subject should be ateleast 15 characters.";
            }

            else if (string.IsNullOrEmpty(message) || (message.Length < 25))
            {
                _status = "txtMessage";
                _desc = "Invalid! Message length should be ateleast 25 characters.";
            }


            // Process if inputs are valid
            else
            {
                try
                {
                    using (Message msg = new Message())
                    {
                        msg.SenderName = name;
                        msg.SenderEmail = email;
                        msg.MailSubject = subject;
                        msg.MessageBody = message;
                        msg.SmtpMailType = MailType.OTHER_HTML_MAIL;
                        msg.SendMail();
                        _desc = "Thank you! Your email has been sent successfully. We will get back to you shortly.";
                        _status = HttpStatusCode.OK.ToString();
                        _code = (int)HttpStatusCode.OK;
                    }
                }
                catch (Exception ex)
                {
                    _code = (int)HttpStatusCode.InternalServerError;
                    _status = HttpStatusCode.InternalServerError.ToString();
                    _desc = ex.Message;
                }
            }

            return Json(new { Status = _status, Code = _code, Description = _desc });
        }


        [HttpPost]
        public JsonResult PostFeeback(FormCollection obj)
        {
            // Assign default result object:
            string _status = HttpStatusCode.NotAcceptable.ToString();
            string _desc = string.Empty;
            int _code = (int)HttpStatusCode.NotAcceptable;
            bool _captcha = false;

            _captcha = CaptchaMvc.HtmlHelpers.CaptchaHelper.IsCaptchaValid(this, "Invalid captcha");

            string name = obj["txtName"];
            string subject = obj["feedbackType"];
            string message = obj["txtMessage"];


            // Validate user inputs:
            if (!_captcha)
            {
                _status = "captcha";
                _desc = "Invalid captcha! Please try another captcha and enter valid code then try.";
            }
            else if (string.IsNullOrEmpty(name) || (name.Length < 5) || !name.IsAlphabetWithSpace())
            {
                _status = "txtName";
                _desc = "Invalid! Name should be ateleast 5 characters and must not contain special characters except blank space.";
            }
            else if (string.IsNullOrEmpty(subject) || subject.ToLower().Equals("select"))
            {
                _status = "txtSubject";
                _desc = "Invalid! Please select subject type.";
            }

            else if (string.IsNullOrEmpty(message) || (message.Length < 20))
            {
                _status = "txtMessage";
                _desc = "Invalid! Message length should be ateleast 20 characters.";
            }


            // Process if inputs are valid
            else
            {
                try
                {
                    using (Message msg = new Message())
                    {
                        msg.SenderName = name;
                        msg.MailSubject = "[Feedback]: " + subject;
                        msg.MessageBody = message;
                        msg.SmtpMailType = MailType.OTHER_HTML_MAIL;
                        msg.SendMail();
                        _desc = "Thank you! Your feedback has been sent successfully. </br>We will analyse and take corrective action soon.";
                        _status = HttpStatusCode.OK.ToString();
                        _code = (int)HttpStatusCode.OK;
                    }
                }
                catch (Exception ex)
                {
                    _code = (int)HttpStatusCode.InternalServerError;
                    _status = HttpStatusCode.InternalServerError.ToString();
                    _desc = ex.Message;
                }
            }

            return Json(new { Status = _status, Code = _code, Description = _desc });
        }

        //[HttpPost]
        //public ActionResult Feedback(FormCollection obj)
        //{
        //    string[] name = HttpContext.Request.Form.GetValues("txtName");

        //    if (CaptchaMvc.HtmlHelpers.CaptchaHelper.IsCaptchaValid(this, "Invalid captcha"))
        //    {
        //        ViewBag.Captcha = "Captcha is valid";
        //    }
        //    else
        //        ViewBag.Captcha = "Captcha is invalid";

        //    return View("index");
        //}

        [HttpPost]
        public JsonResult SubscribeUser(string email)
        {
            bool status = false;
            string msg = string.Empty;
            string _code = string.Empty;

            try
            {
                //_ticket = TalentsProfile.Models.HttpUser.ReadUsercookie();
                IDataAccess accessor = new DataAccess(_ticket);
                msg = accessor.ManageSubscription(email, true);

                if (msg.Equals("ALREADY_SUBSCRIBED"))
                {
                    _code = "EXIST";
                    msg = "You have already subscribed with us.";
                }

                else if (msg.Equals("SUBSCRIBED") || msg.Equals("RE_SUBSCRIBED"))
                {
                    _code = "OK";
                    msg = "Thank you! Your subscription added successfully.";
                }
                else if (msg.Equals("DE_SUBSCRIBED"))
                {
                    _code = "UNSUBSCRIBED_OK";
                    msg = "Your subscription removed successfully. You will not receive any further notifications from us. Thanks for choosing us and have a great day!";
                }
                else
                {
                    _code = "ERR";
                    msg = "Seems something went wrong. Please try again!";
                }

                status = true;
            }
            catch (Exception ex)
            {
                Common.LogError(ex);
                _code = "ERR";
                msg = "Sorry! Your subscription could not save. Please try again!";
            }

            return Json(new { Status = status, Message = msg, Code = _code });
        }

        #endregion POST_Methods
    }
}