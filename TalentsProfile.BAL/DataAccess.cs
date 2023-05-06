using System;
using System.Web;
using System.Data;
using TalentsProfile.DTO;
using System.Linq;
using System.Reflection;
using System.Configuration;
using Arcmatics.TalentsProfile.DAL;
using Communicator.Notification;
using System.Collections.Generic;
using TalentsProfile.DTO.Employers;
using Arcmatics.Security;


namespace TalentsProfile.BAL
{
    public class DataAccess : FileManager, IDataAccess
    {
        UserTicket ticket;
        DataProvider objProvider;
        Cryptography crypt;

        private DataSet ViewProfile(string Session, string Token, Int64? profileId)
        {
            //UserTicket ticket = (UserTicket)HttpContext.Current.Session["ticket"];
            DataProvider objProvider = new DataProvider();

            if (Session != ticket.Tocken)
                throw new ArgumentException("INVALID_REQ");

            CandidateProfile profileInfo = new CandidateProfile();
            DataSet profileDtl = objProvider.GetDataSet(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ticket.Tocken, ticket.Email, profileId }, ""), string.Empty);

            return profileDtl;
        }

        public HttpPostedFileBase Photo { get; set; }

        public SocialUser SocialUserInfo { get; set; }

        public DataAccess(UserTicket userTicket)
        {
            ticket = userTicket;
            objProvider = new DataProvider();
            SocialUserInfo = new Talentsprofile();
            crypt = new Cryptography();
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Register new user: employer/job-seeker.
        /// </summary>
        /// <param name="email">Email id</param>
        /// <param name="userPwd">Confirm password</param>
        /// <param name="userType">Empty for new</param>
        /// <returns>User exists/not exists.</returns>
        public string AddMembership(string email, string password, Int16 uType, string captcha, string name, string lastName, char gender, int country, string designation, string companyName)
        {
            email = email.Trim().ToLower();
            DataProvider objProvider = new DataProvider();
            string status = objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { email, password, uType, captcha, name, lastName, gender, country, designation, companyName }, ""), string.Empty);

            //Enum.GetName(typeof(UserType), uType)

            if (status.Equals("USER_EXISTS"))
                throw new ArgumentException("USER_EXISTS");

            try
            {
                Message smtpMsg = new Message(name, email, "TalentsProfile- Verification and account activation link");
                smtpMsg.SmtpMailType = MailType.OTR;
                smtpMsg.Link = status;
                smtpMsg.SendMail();
            }
            catch (Exception ex) { throw new ArgumentException("MAIL_ERR"); }
            return "OK";
        }


        /// <summary>
        /// Add user into DB for first login, using Google or other account.
        /// </summary>
        /// <param name="email">Google email account</param>
        /// <param name="id">Google Id</param>
        /// <param name="firstName">Given name as in Google</param>
        /// <param name="lastName">Family name as in Google</param>
        /// <param name="gender">Gender</param>
        /// <param name="link">URI</param>
        /// <param name="profilePic">Profile picture URI</param>
        /// <param name="emailVerified">Is email verified</param>
        /// <param name="locale">User locale</param>
        /// <returns></returns>
        private string AddMembershipUser(string email, string id, string firstName, string lastName, string gender, string link, string profilePic,
                                        bool emailVerified, string locale, short uType, string socialAcc, string sessionId, string city, string countryName)
        {
            email = email.Trim().ToLower();

            // User always should be considered as Talent-user-type.
            uType = (short)UserType.JobSeeker;

            if (gender.ToLower().Trim().Equals("male"))
                gender = "M";
            else if (gender.ToLower().Trim().Equals("female"))
                gender = "F";
            else
                gender = "O";

            DataProvider objProvider = new DataProvider();
            return objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { email, id, firstName, lastName, gender, link, profilePic, emailVerified, locale, uType, socialAcc, sessionId, city, countryName.Trim() }, ""),
                            string.Empty);
        }


        /// <summary>
        /// Request to reset user's password.
        /// </summary>
        /// <param name="email">Email id</param>
        /// <returns></returns>
        public bool ForgotPassword(string email)
        {
            DataProvider objProvider = new DataProvider();
            string userName = objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { email }, ""), string.Empty);

            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("NOT_EXISTS");

            try
            {
                Message smtpMsg = new Message(userName, email, "TalentsProfile- Account Password reset link");
                smtpMsg.SmtpMailType = MailType.FORGET_PASSWORD;
                smtpMsg.Link = "";
                smtpMsg.SendMail();
            }
            catch (Exception ex) { throw new ArgumentException("MAIL_ERR"); }
            return true;
        }


        public UserTicket Login(string email, string pwd, string tocken)
        {
            string id = string.Empty;
            ticket = new UserTicket();
            tocken = HttpContext.Current.Session.SessionID;
            DataProvider objProvider = new DataProvider();

            switch (SocialUserInfo.UserSocialAccountType)
            {
                case SocialAccountType.Google:
                    GoogleUser g = (TalentsProfile.DTO.GoogleUser)this.SocialUserInfo;
                    id = AddMembershipUser(g.Email, g.Id, g.Given_Name, g.Family_Name, g.Gender, g.Link, g.Picture, g.Verified_Email, g.Locale, g.UserType,
                                           g.UserSocialAccountType.ToString(), tocken, string.Empty, string.Empty);

                    ticket.Id = "#" + id;
                    ticket.Email = g.Email;
                    ticket.Name = string.Format("{0} {1}", g.Given_Name, g.Family_Name);
                    ticket.Tocken = tocken;
                    ticket.UType = g.UserTypeName;
                    break;

                case SocialAccountType.LinkedIn:
                    LinkedInUser li = (LinkedInUser)SocialUserInfo;
                    string[] location = ((string)li.Location.name).Split(',');
                    id = AddMembershipUser(li.EmailAddress, li.Id, li.FirstName, li.LastName, "", li.PublicProfileUrl, li.PictureUrl, true, "", li.UserType,
                                      li.UserSocialAccountType.ToString(), tocken, location[0], location[1]);

                    ticket.Id = "#" + id;
                    ticket.Email = li.EmailAddress;
                    ticket.Name = string.Format("{0} {1}", li.FirstName, li.LastName);
                    ticket.Tocken = tocken;
                    ticket.UType = li.UserTypeName;

                    try
                    {
                        string compType = (bool)li.Positions.values[0].isCurrent ? "CO" : string.Empty;

                        // Update user profile:
                        DateTime dateFrom = new DateTime((int)li.Positions.values[0].startDate.year, (int)li.Positions.values[0].startDate.month, 1);
                        ManageCompanyWorked(li.EmailAddress, tocken, (string)li.Positions.values[0].company.name, dateFrom, null,
                                            (string)li.Positions.values[0].location.name, null, (string)li.Positions.values[0].location.country.name,
                                            (string)li.Positions.values[0].title, 15, compType,
                                            UserAction.Add.ToString().ToUpper(), long.Parse(id));

                        // Update summary profile:
                        UpdateSummaryProfile(li.Summary, 0, string.Empty, id);
                    }
                    catch { }
                    break;

                default:
                    DataTable user = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                                    new object[] { email, pwd, tocken }, ""), string.Empty);

                    //Enum.GetName(typeof(UserType), uType)
                    if (user != null && user.Rows.Count > 0)
                    {
                        ticket.Email = user.Rows[0]["Email"].ToString();
                        ticket.Name = user.Rows[0]["Name"].ToString();
                        ticket.Tocken = user.Rows[0]["Tocken"].ToString();
                        ticket.Id = "#" + user.Rows[0]["Id"].ToString();
                        ticket.UType = Enum.GetName(typeof(UserType), user.Rows[0]["UType"]);
                    }
                    else
                        throw new ArgumentException("NOT_EXISTS");

                    break;
            }

            return ticket;
        }


        /// <summary>
        /// Check for the profile completeion and navigate page.
        /// </summary>
        /// <param name="Session">Session id</param>
        /// <param name="Token">Email</param>
        /// <returns>Redirect page</returns>
        public string RedirectAction(string Session, string Token)
        {
            //UserTicket ticket = (UserTicket)HttpContext.Current.Session["ticket"];
            DataProvider objProvider = new DataProvider();

            if (Token == null || HttpContext.Current.Session.SessionID != Session)
                throw new ArgumentException("INVALID_REQ");

            string redirectToAction = string.Empty;
            redirectToAction = objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { HttpContext.Current.Session.SessionID, Token }, ""), string.Empty);

            return redirectToAction;
        }


        /// <summary>
        /// Create and initialize the profile.
        /// </summary>
        /// <returns>Success/fail</returns>
        public void CreateProfile(string title, bool isExperienced, decimal totExpr, string session, string token)
        {
            UserTicket ticket = (UserTicket)HttpContext.Current.Session["ticket"];
            DataProvider objProvider = new DataProvider();

            if (session != ticket.Tocken)
                throw new ArgumentException("INVALID_REQ");

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { title, isExperienced, totExpr, ticket.Tocken, ticket.Email }, ""), string.Empty);
        }


        /// <summary>
        /// Activate user's account
        /// </summary>
        public bool ActivateAccount(string userAcc, string userType)
        {
            Arcmatics.Security.Cryptography crypt = new Arcmatics.Security.Cryptography();
            userAcc = crypt.DecryptString(userAcc);
            DataProvider objProvider = new DataProvider();
            string status = objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { userAcc, userType }, ""), string.Empty);

            if (status.Equals("OK"))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Set user's account password while activating the account.
        /// </summary>
        /// <returns></returns>
        public bool SetAccountPassword(string userAcc, string password)
        {
            Arcmatics.Security.Cryptography crypt = new Arcmatics.Security.Cryptography();
            userAcc = crypt.DecryptString(userAcc);
            DataProvider objProvider = new DataProvider();
            string status = objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { userAcc, password }, ""), string.Empty);

            if (status.Equals("OK"))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Update personal info.
        /// </summary>
        public void JSPersonalInfo(string Action, string FirstName, string LastName, char Gender, string DoB, string Phone1, string Phone2, string Address, string City,
                                   string PostalCode, int Nationality, string LangKnown, int MaritalStatus, int PassportStatus, int VisaStatus, int visaCountry,
                                   string Session, string Uid)
        {

            UserTicket ticket = (UserTicket)HttpContext.Current.Session["ticket"];
            DataProvider objProvider = new DataProvider();

            if (Session != ticket.Tocken)
                throw new ArgumentException("INVALID_REQ");

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { Action, FirstName, LastName, Gender, DoB, Phone1, Phone2, Address, City, PostalCode, Nationality, LangKnown, MaritalStatus,
                                PassportStatus, VisaStatus, visaCountry, ticket.Tocken, ticket.Email }, ""), string.Empty);

        }


        /// <summary>
        /// Update salary information of jobseeker.
        /// </summary>
        public void UpdateSalaryInfo(string ctc, string ctcCurrency, string ectc, string ectcCurrency, string Session, string Uid)
        {
            DataProvider objProvider = new DataProvider();

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ctc, ctcCurrency, ectc, ectcCurrency, ticket.Tocken, ticket.Email }, ""), string.Empty);
        }


        /// <summary>
        /// Update jobseeker's skills set.
        /// </summary>
        public void UpdateSkills(string primarySkills, string secondarySkills, string Session, string Uid)
        {
            DataProvider objProvider = new DataProvider();

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { primarySkills, secondarySkills, ticket.Tocken, ticket.Email }, ""), string.Empty);
        }


        /// <summary>
        /// Update profile summary.
        /// </summary>
        public void UpdateSummaryProfile(string profileSummary, decimal totExpr, string Session, string Uid)
        {
            DataProvider objProvider = new DataProvider();

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { profileSummary, totExpr, ticket.Tocken, ticket.Email }, ""), string.Empty);
        }


        /// <summary>
        /// Update graduation detail.
        /// </summary>
        public void UpdateGraduationInfo(string degree, string course, string passedYear, string institute, string university, string Session, string Uid)
        {
            DataProvider objProvider = new DataProvider();

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { degree, course, passedYear, institute, university, ticket.Tocken, ticket.Email }, ""), string.Empty);
        }


        /// <summary>
        /// Update secondary school information (10th and 12th)
        /// </summary>
        public void UpdateUnderGraduation(string courseName, string discipline, string passedYear, string school, string board, string courseName10,
                                          string discipline10, string passedYear10, string school10, string board10, string Session, string Uid)
        {
            DataProvider objProvider = new DataProvider();

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { courseName, discipline, passedYear, school, board,
                                           courseName10, discipline10, passedYear10, school10, board10, ticket.Tocken, ticket.Email }, ""), string.Empty);
        }


        /// <summary>
        /// Update post-graduation detail.
        /// </summary>
        public EducationalDetail UpdatePostGraduationInfo(string degree, string course, string passedYear, string institute, string university, string Session, string Uid, Nullable<Int64> pgId)
        {
            DataProvider objProvider = new DataProvider();

            DataTable dtEdu = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { degree, course, passedYear, institute, university, ticket.Tocken, ticket.Email, pgId }, ""), string.Empty);

            EducationalDetail eduDtl = new EducationalDetail();

            if (dtEdu != null)
            {
                eduDtl.PostGraduation = new List<EducationalProfile>(from ug in dtEdu.AsEnumerable()
                                                                     select new EducationalProfile
                                                                     {
                                                                         SrNo = Convert.ToInt64(ug["SrNo"]),
                                                                         Degree = Convert.ToString(ug["Degree"]),
                                                                         Course = Convert.ToString(ug["Course"]),
                                                                         InstituteName = Convert.ToString(ug["InstituteName"]),
                                                                         PassedYear = Convert.ToString(ug["PassedYear"]),
                                                                         University = Convert.ToString(ug["University"])
                                                                     });
            }

            return eduDtl;
        }


        /// <summary>
        /// Get the complete profile of candidate, for job-seeker view.
        /// </summary>
        /// <param name="Session">Session id</param>
        /// <param name="Token">Email/uid/tocken</param>
        /// <returns>Set of tables</returns>
        public CandidateProfile GetProfile(string Session, string Token)
        {
            // Get the profile id.
            string id = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Substring(HttpContext.Current.Request.UrlReferrer.AbsoluteUri.LastIndexOf('/') + 1);
            Nullable<Int64> profileId = null;

            try
            {
                // If not accessing by self (talent).
                if (!string.IsNullOrEmpty(id))
                    profileId = Int64.Parse(id);
            }
            catch { }

            DataSet profileDtl = ViewProfile(Session, Token, profileId);
            CandidateProfile profileInfo = new CandidateProfile();

            /*
            UserTicket ticket = (UserTicket)HttpContext.Current.Session["ticket"];
            DataProvider objProvider = new DataProvider();

            if (Session != ticket.Tocken)
                throw new ArgumentException("INVALID_REQ");

            DataSet profileDtl = objProvider.GetDataSet(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ticket.Tocken, ticket.Email }, ""), string.Empty);
            */
            if (profileDtl != null && profileDtl.Tables.Count > 0)
            {
                Random rnd = new Random();
                int _rnd = rnd.Next(1, 99);
                PersonalProfile profile = new PersonalProfile();

                foreach (DataRow row in profileDtl.Tables[0].Rows)
                {
                    profile.Name = Convert.ToString(row["Name"]);
                    profile.Phone = Convert.ToString(row["Phone"]);
                    profile.AltNum = Convert.ToString(row["AlternateNum"]);
                    profile.Email = Convert.ToString(row["Email"]);
                    profile.BirthDate = Convert.ToString(row["DoB"]);
                    profile.Gender = Convert.ToString(row["Gender"]);
                    profile.Address = Convert.ToString(row["Address"]);
                    profile.City = Convert.ToString(row["City"]);
                    profile.PostalCode = Convert.ToString(row["PostalCode"]);
                    profile.Country = Convert.ToString(row["Country"]);
                    profile.LanguagesKnown = Convert.ToString(row["LanguagesKnown"]);
                    profile.MaritalStatus = Convert.ToString(row["MarritalStatus"]);
                    profile.PassportStatus = Convert.ToString(row["PassportStatus"]);
                    profile.VisaStatus = Convert.ToString(row["VisaStatus"]);
                    profile.VisaCountry = Convert.ToString(row["VisaCountry"]);
                    profile.VisaExpireDate = Convert.ToString(row["VisaExpireDate"]);

                    // URI contains http:// or https:// means user is from other social account.
                    if ((Convert.ToString(row["Photo"])).Contains("http://") || (Convert.ToString(row["Photo"])).Contains("https://"))
                        profile.Photo = Convert.ToString(row["Photo"]);
                    else
                        profile.Photo = (ConfigurationManager.AppSettings["ProfilePic"].ToString() + Convert.ToString(row["Photo"]) + "?pic=" + _rnd.ToString());

                    profile.CTC = Convert.ToString(row["CTC"]);
                    profile.CTCCurrencyType = Convert.ToString(row["CurrencyCTC"]);
                    profile.ECTC = Convert.ToString(row["ECTC"]);
                    profile.ECTCCurrencyType = Convert.ToString(row["CurrencyECTC"]);
                    profile.NoticePeriod = Convert.ToString(row["NoticePeriod"]);
                    profile.PrimarySkills = Convert.ToString(row["PrimarySkills"]);
                    profile.SecondarySkills = Convert.ToString(row["SecondarySkills"]);
                    profile.SummaryProfile = Convert.ToString(row["SummaryProfile"]).Replace("\n", "<br />");
                    profile.ResumeTitle = Convert.ToString(row["Title"]);
                    profile.Designation = Convert.ToString(row["Designation"]);
                    profile.Education = Convert.ToString(row["Education"]);
                    profile.UpdatedOn = Convert.ToString(row["UpdatedOn"]);
                    profile.ProfileCompleted = Convert.ToString(row["ProfComplete"]);
                    profile.ResumeAttached = Convert.ToString(row["CvAttachment"]);
                }


                profileInfo.PersonalDetail = profile;

                EducationalProfile education = new EducationalProfile();
                // Graduation:
                foreach (DataRow row in profileDtl.Tables[3].Rows)
                {
                    education.Degree = Convert.ToString(row["Degree"]);
                    education.Course = Convert.ToString(row["Course"]);
                    education.InstituteName = Convert.ToString(row["InstituteName"]);
                    education.University = Convert.ToString(row["University"]);
                    education.PassedYear = Convert.ToString(row["PassedYear"]);
                }

                profileInfo.Graduation = education;

                // Post Graduation:
                List<EducationalProfile> pg = new List<EducationalProfile>();
                foreach (DataRow row in profileDtl.Tables[4].Rows)
                {
                    pg.Add(new EducationalProfile
                    {
                        Degree = Convert.ToString(row["Degree"]),
                        Course = Convert.ToString(row["Course"]),
                        InstituteName = Convert.ToString(row["InstituteName"]),
                        University = Convert.ToString(row["University"]),
                        PassedYear = Convert.ToString(row["PassedYear"])
                    });
                }

                profileInfo.PostGraduation = pg;

                profileInfo.UnderGraduation = new List<EducationalProfile>(from ug in profileDtl.Tables[5].AsEnumerable()
                                                                           select new EducationalProfile
                                                                           {
                                                                               Degree = Convert.ToString(ug["CourseName"]),
                                                                               Course = Convert.ToString(ug["Discipline"]),
                                                                               InstituteName = Convert.ToString(ug["InstituteName"]),
                                                                               PassedYear = Convert.ToString(ug["PassedYear"]),
                                                                               University = Convert.ToString(ug["Board"])
                                                                           });
            }

            return profileInfo;
        }


        /// <summary>
        /// Get the JS personal profile.
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="Token"></param>
        public PersonalProfile GetJSProfile(string Session, string Token)
        {
            //UserTicket ticket = (UserTicket)HttpContext.Current.Session["ticket"];
            DataProvider objProvider = new DataProvider();

            //if (Session != ticket.Tocken)
            //    throw new ArgumentException("INVALID_REQ");

            DataTable dtProfile = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ticket.Tocken, ticket.Email }, ""), string.Empty);

            PersonalProfile profile = null;
            Random rnd = new Random();
            int _rnd = rnd.Next(1, 99);

            if (dtProfile != null && dtProfile.Rows.Count > 0)
            {
                profile = new PersonalProfile
                {
                    Name = Convert.ToString(dtProfile.Rows[0]["FirstName"]),
                    Surname = Convert.ToString(dtProfile.Rows[0]["LastName"]),
                    Phone = Convert.ToString(dtProfile.Rows[0]["Phone"]),
                    AltNum = Convert.ToString(dtProfile.Rows[0]["AlternateNum"]),
                    BirthDate = Convert.ToString(dtProfile.Rows[0]["DoB"]),
                    Gender = Convert.ToString(dtProfile.Rows[0]["Gender"]),
                    MaritalStatus = Convert.ToString(dtProfile.Rows[0]["MarritalStatus"]),
                    Address = Convert.ToString(dtProfile.Rows[0]["Address"]),
                    City = Convert.ToString(dtProfile.Rows[0]["City"]),
                    PostalCode = Convert.ToString(dtProfile.Rows[0]["PostalCode"]),
                    Country = Convert.ToString(dtProfile.Rows[0]["Country"]),
                    LanguagesKnown = Convert.ToString(dtProfile.Rows[0]["LanguagesKnown"]),
                    PassportStatus = Convert.ToString(dtProfile.Rows[0]["PassportStatus"]),
                    VisaStatus = Convert.ToString(dtProfile.Rows[0]["VisaStatus"]),
                    VisaCountry = Convert.ToString(dtProfile.Rows[0]["VisaCountry"]),

                    Photo = ((Convert.ToString(dtProfile.Rows[0]["Photo"])).Contains("http://") || (Convert.ToString(dtProfile.Rows[0]["Photo"])).Contains("https://")) ?
                            Convert.ToString(dtProfile.Rows[0]["Photo"]) :
                            (ConfigurationManager.AppSettings["ProfilePic"].ToString() + Convert.ToString(dtProfile.Rows[0]["Photo"]) + "?pic=" + _rnd.ToString()),

                    //Photo = ConfigurationManager.AppSettings["ProfilePic"].ToString() + Convert.ToString(dtProfile.Rows[0]["Photo"]) + "?pic=" + _rnd.ToString()
                };
            }

            return profile;
        }


        /// <summary>
        /// Get job-seeker's salary detail.
        /// </summary>
        public SalaryDetail GetJSSalaryInfo(string Session, string Token)
        {
            DataProvider objProvider = new DataProvider();
            DataTable dtSalary = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ticket.Tocken, ticket.Email }, ""), string.Empty);

            SalaryDetail salaryInfo = null;
            if (dtSalary != null && dtSalary.Rows.Count > 0)
            {
                salaryInfo = new SalaryDetail
                {
                    CTC = Convert.ToString(dtSalary.Rows[0]["CTC"]),
                    CTCCurrencyType = Convert.ToString(dtSalary.Rows[0]["CurrencyCTC"]),
                    ECTC = Convert.ToString(dtSalary.Rows[0]["ECTC"]),
                    ECTCCurrencyType = Convert.ToString(dtSalary.Rows[0]["CurrencyECTC"])
                };
            }
            return salaryInfo;
        }


        public SummaryProfile GetSummaryProfile(string Session, string Token)
        {
            DataProvider objProvider = new DataProvider();
            DataTable dtProfile = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ticket.Tocken, ticket.Email }, ""), string.Empty);

            SummaryProfile profile = null;
            if (dtProfile != null && dtProfile.Rows.Count > 0)
            {
                profile = new SummaryProfile
                {
                    PrimarySkills = Convert.ToString(dtProfile.Rows[0]["PrimarySkills"]),
                    SecondarySkills = Convert.ToString(dtProfile.Rows[0]["SecondarySkills"]),
                    TotalExperienceYear = Convert.ToString(dtProfile.Rows[0]["TotExpr"]).Split('.')[0],
                    TotalExperienceMonth = Convert.ToString(dtProfile.Rows[0]["TotExpr"]).Split('.')[1],
                    ProfileSummary = Convert.ToString(dtProfile.Rows[0]["SummaryProfile"]),
                    NoticePeriod = Convert.ToString(dtProfile.Rows[0]["NoticePeriod"])
                };
            }
            return profile;
        }


        /// <summary>
        /// Manage profile projects- add & update.
        /// </summary>
        public void ManageProjects(string title, string roles, string synopsis, string tools, string session, string uid, Nullable<Int64> id, string action)
        {
            DataProvider objProvider = new DataProvider();

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { title, roles, synopsis, tools, ticket.Tocken, ticket.Email, id, action }, ""), string.Empty);
        }



        /// <summary>
        /// Get the list of projects done.
        /// </summary>
        /// <returns></returns>
        public List<ProjectDetail> GetProjects(string session, string token, Nullable<Int64> id, string action, Nullable<Int64> profileId)
        {
            DataProvider objProvider = new DataProvider();

            //if (ticket != null && HttpContext.Current.Session.SessionID != ticket.Tocken)
            //    throw new ArgumentException("INVALID_REQ");

            //// Get the profile id.
            //string pId = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Substring(HttpContext.Current.Request.UrlReferrer.AbsoluteUri.LastIndexOf('/') + 1);

            //try
            //{
            //    // If not accessing by self (talent).
            //    if (!string.IsNullOrEmpty(pId))
            //        profileId = Int64.Parse(pId);
            //}
            //catch { }

            profileId = Int64.Parse(ticket.Id.Replace("#", ""));

            DataTable dtProj = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ticket.Tocken, ticket.Email, id, action, profileId }, ""), string.Empty);

            List<ProjectDetail> proj = null;

            if (dtProj != null && dtProj.Rows.Count > 0)
            {
                proj = new List<ProjectDetail>();

                foreach (DataRow row in dtProj.Rows)
                {
                    proj.Add(new ProjectDetail
                    {
                        ProjId = Convert.ToInt64(row["SrNo"]),
                        Title = Convert.ToString(row["Title"]),
                        Synopsis = Convert.ToString(row["Synopsis"]),
                        ToolsUsed = Convert.ToString(row["ToolsUsed"]),
                        Duration = Convert.ToString(row["Duration"]),
                        RolePlayed = Convert.ToString(row["RolePlayed"])
                    });
                }
            }

            return proj;
        }


        /// <summary>
        /// Remove a project from candidates resume.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <param name="profileId"></param>
        public void DeleteProjects(string session, string token, Nullable<Int64> id, Nullable<Int64> profileId)
        {
            DataProvider objProvider = new DataProvider();
            profileId = Int64.Parse(ticket.Id.Replace("#", ""));

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ticket.Tocken, ticket.Email, id, profileId }, ""), string.Empty);
        }


        /// <summary>
        /// Remove an organization from candidates resume.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <param name="profileId"></param>
        public void DeleteOrganization(string session, string token, Nullable<Int64> id, Nullable<Int64> profileId)
        {
            DataProvider objProvider = new DataProvider();
            profileId = Int64.Parse(ticket.Id.Replace("#", ""));

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ticket.Tocken, ticket.Email, id, profileId }, ""), string.Empty);
        }


        /// <summary>
        /// Remove a record from candidate's post-graduation list.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <param name="profileId"></param>
        public void DeletePGEducation(string session, string token, Nullable<Int64> id, Nullable<Int64> profileId)
        {
            DataProvider objProvider = new DataProvider();
            profileId = Int64.Parse(ticket.Id.Replace("#", ""));

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ticket.Tocken, ticket.Email, id, profileId }, ""), string.Empty);
        }

        /// <summary>
        /// Get the list of educational qualification.
        /// </summary>
        /// <returns></returns>
        public EducationalDetail GetQualification(string session, string token, string action, Nullable<long> eduVal)
        {
            DataProvider objProvider = new DataProvider();

            //if (ticket != null && HttpContext.Current.Session.SessionID != ticket.Tocken)
            //    throw new ArgumentException("INVALID_REQ");

            DataSet dtEdu = objProvider.GetDataSet(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ticket.Tocken, ticket.Email, action, eduVal }, ""), string.Empty);

            EducationalDetail eduDtl = new EducationalDetail();

            if (dtEdu != null && dtEdu.Tables.Count > 0 && dtEdu.Tables[0].Rows.Count > 0)
            {
                eduDtl.Graduation = new EducationalProfile
                {
                    SrNo = Convert.ToInt64(dtEdu.Tables[0].Rows[0]["SrNo"]),
                    Degree = Convert.ToString(dtEdu.Tables[0].Rows[0]["Degree"]),
                    Course = Convert.ToString(dtEdu.Tables[0].Rows[0]["Course"]),
                    InstituteName = Convert.ToString(dtEdu.Tables[0].Rows[0]["InstituteName"]),
                    PassedYear = Convert.ToString(dtEdu.Tables[0].Rows[0]["PassedYear"]),
                    University = Convert.ToString(dtEdu.Tables[0].Rows[0]["University"])
                };

            }

            if (dtEdu != null && dtEdu.Tables.Count > 1 && dtEdu.Tables[1].Rows.Count > 0)
            {
                eduDtl.PostGraduation = new List<EducationalProfile>(from ug in dtEdu.Tables[1].AsEnumerable()
                                                                     select new EducationalProfile
                                                                     {
                                                                         SrNo = Convert.ToInt64(ug["SrNo"]),
                                                                         Degree = Convert.ToString(ug["Degree"]),
                                                                         Course = Convert.ToString(ug["Course"]),
                                                                         InstituteName = Convert.ToString(ug["InstituteName"]),
                                                                         PassedYear = Convert.ToString(ug["PassedYear"]),
                                                                         University = Convert.ToString(ug["University"])
                                                                     });


            }

            if (dtEdu != null && dtEdu.Tables.Count > 2 && dtEdu.Tables[2].Rows.Count > 0)
            {
                eduDtl.UnderGraduation = new List<EducationalProfile>(from ug in dtEdu.Tables[2].AsEnumerable()
                                                                      select new EducationalProfile
                                                                      {
                                                                          Degree = Convert.ToString(ug["CourseName"]),
                                                                          Course = Convert.ToString(ug["Discipline"]),
                                                                          InstituteName = Convert.ToString(ug["InstituteName"]),
                                                                          PassedYear = Convert.ToString(ug["PassedYear"]),
                                                                          University = Convert.ToString(ug["Board"])
                                                                      });
            }

            return eduDtl;
        }


        /// <summary>
        /// Manage job-seeker's organizational detail.
        /// </summary>
        /// <param name="session">User session id</param>
        /// <param name="token">User's email id</param>
        /// <param name="compName">Company name</param>
        /// <param name="fromDate">Start date</param>
        /// <param name="toDate">End date</param>
        /// <param name="city">Job location</param>
        /// <param name="country">Job country id</param>
        /// <param name="countryName">Job country name, if id not available</param>
        /// <param name="designation">Job designation</param>
        /// <param name="noticePeriod">Notice period, by default 15 days</param>
        /// <param name="compType">Denotes if this is current company</param>
        /// <param name="action">Action to perform- add/update/delete</param>
        /// <param name="id">User id</param>
        public void ManageCompanyWorked(string session, string token, string compName, Nullable<DateTime> fromDate, Nullable<DateTime> toDate, string city, Nullable<int> country,
                                        string countryName, string designation, int noticePeriod, string compType, string action, Nullable<Int64> id)
        {
            //if (ticket != null && session != ticket.Tocken)
            //    throw new ArgumentException("INVALID_REQ");

            // Country code cannot be available in API e.g. LinkedIn. For data integration country name can be used.
            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                                     new object[] { ticket.Tocken, ticket.Email, compName, fromDate, toDate, city, country, countryName.Trim(),
                                     designation, noticePeriod, compType, action, id }, ""), string.Empty);
        }


        /// <summary>
        /// Read detail of organizational detail job-seeker worked in.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="token"></param>
        public List<OrganizationalProfile> GetOrganizationalInfo(string session, string token, Nullable<Int64> id, string action, Nullable<Int64> profileId)
        {
            //if (ticket != null && HttpContext.Current.Session.SessionID != ticket.Tocken)
            //    throw new ArgumentException("INVALID_REQ");

            // Get the profile id.
            //string pId = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Substring(HttpContext.Current.Request.UrlReferrer.AbsoluteUri.LastIndexOf('/') + 1);

            //try
            //{
            //    // If not accessing by self (talent).
            //    if (!string.IsNullOrEmpty(pId))
            //        profileId = Int64.Parse(pId);
            //}
            //catch { }

            profileId = Int64.Parse(ticket.Id.Replace("#", ""));

            DataTable tbl = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                           new object[] { ticket.Tocken, ticket.Email, id, action, profileId }, ""), string.Empty);


            List<OrganizationalProfile> orgLst = new List<OrganizationalProfile>(
                                                     from org in tbl.AsEnumerable()
                                                     select new OrganizationalProfile
                                                     {
                                                         SrNo = Convert.ToInt64(org["SrNo"]),
                                                         OrganizationName = Convert.ToString(org["OrganizationName"]),
                                                         DateFrom = Convert.ToString(org["FromDate"]),
                                                         ToDate = Convert.ToString(org["ToDate"]),
                                                         City = Convert.ToString(org["City"]),
                                                         Designation = Convert.ToString(org["Designation"]),
                                                         Country = Convert.ToInt32(org["Country"]),
                                                         CountryName = Convert.ToString(org["CountryName"]),
                                                         CompanyType = (Convert.ToString(org["Flag"]) == "CO") ? true : false,
                                                         NoticePeriod = Convert.ToString(org["NoticePeriod"])
                                                     });
            return orgLst;

        }


        /// <summary>
        /// Upload resume to database as binary
        /// </summary>
        /// <param name="fileObj"></param>
        public void ManageResume(byte[] file, string title, string session, string token, string action)
        {
            //ResourceType resxType,
            if (ticket != null && session != ticket.Tocken)
                throw new ArgumentException("INVALID_REQ");

            objProvider.ExecuteQuery(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                new object[] { file, title, ticket.Tocken, ticket.Email, action }, ""), string.Empty);
            //FileType = resxType;
            //SaveResume(fileObj, "new_file.txt");
        }

        /// <summary>
        /// Upload resume to repository in actual format.
        /// </summary>
        /// <param name="fileObj"></param>
        public void ManageResume(HttpPostedFileBase fileObj, string domain, string email, string phone)
        {
            FileType = ResourceType.Resume;
            string fileName = string.Format("{0}{1}{2}{3}-{4}-{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, domain, fileObj.FileName.Replace(" ", ""));
            SaveResume(fileObj, fileName);
        }



        public string ManageProfilePic(string photo, string session, string token)
        {
            base.FileType = ResourceType.ProfilePic;
            DateTime dt = DateTime.Now;
            string fileName = dt.Day.ToString() + dt.Year.ToString() + dt.Month.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Millisecond.ToString() + System.IO.Path.GetExtension(Photo.FileName);

            fileName = objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                new object[] { fileName, ticket.Tocken, ticket.Email }, ""), string.Empty);

            base.SaveResume(Photo, fileName);

            return ConfigurationManager.AppSettings["ProfilePic"].ToString() + fileName;
        }


        /// <summary>
        /// Insert job serch verbs.
        /// Returns list of jobs found.
        /// </summary>
        public List<JobSearchResult> JobSearched(string session, string email, string keyword, string location, string jobType, int? minExpr, int? maxExpr, string salary, string postedBy, int? postedDate)
        {
            string usrEmail = null;
            string usrTocken = null;

            if (ticket != null && session != ticket.Tocken)
                throw new ArgumentException("INVALID_REQ");

            if (ticket != null)
            {
                usrEmail = ticket.Email;
                usrTocken = ticket.Tocken;
            }

            DataTable jobsTbl = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                                new object[] { usrTocken, usrEmail, keyword, location, jobType, minExpr, maxExpr,
                                salary, postedBy, postedDate }, ""), string.Empty);

            if (jobsTbl == null)
                return null;

            List<JobSearchResult> jobs = new List<JobSearchResult>(from job in jobsTbl.AsEnumerable()
                                                                   select new JobSearchResult
                                                                   {
                                                                       JobId = crypt.EncryptString(string.Format("{0}{1}",
                                                                                       job["Key"].ToString(), job["JobId"].ToString())),
                                                                       JobCode = Convert.ToString(job["JobRefCode"]),
                                                                       JobTitle = Convert.ToString(job["JobTitle"]),
                                                                       Designation = Convert.ToString(job["Designation"]),
                                                                       Experience = Convert.ToString(job["Experience"]),
                                                                       SkillsSet = Convert.ToString(job["ReqSkills"]),
                                                                       Organization = Convert.ToString(job["Organization"]),
                                                                       Location = Convert.ToString(job["JobLocation"]),
                                                                       Description = Convert.ToString(job["JobDesc"]),
                                                                       PostedDate = Convert.ToString(job["PostedDate"]),
                                                                       CompanyLogo = Convert.ToString(job["CompanyLogo"]),
                                                                       JobType = Convert.ToString(job["JobType"]),
                                                                   });

            return jobs;
        }


        /// <summary>
        /// Get detail of a particular job
        /// </summary>
        public JobSearchResult JobDetail(long? jobId)
        {
            //string usrEmail = null;
            //string usrTocken = null;

            //if (ticket != null && session != ticket.Tocken)
            //    throw new ArgumentException("INVALID_REQ");

            //if (ticket != null)
            //{
            //    usrEmail = ticket.Email;
            //    usrTocken = ticket.Tocken;
            //}

            // Decrypt salted byte and read the id.
            jobId = int.Parse(crypt.DecryptString(ticket.Id).Substring(8));
            DataTable jobsTbl = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                                                         new object[] { jobId }, ""), string.Empty);

            if (jobsTbl == null)
                return null;

            List<JobDetail> jobs = new List<JobDetail>(from job in jobsTbl.AsEnumerable()
                                                       select new JobDetail
                                                       {
                                                           //JobId = crypt.EncryptString(string.Format("{0}{1}",
                                                           //                            job["Key"].ToString(), job["JobId"].ToString())),
                                                           JobCode = Convert.ToString(job["JobRefCode"]),
                                                           JobTitle = Convert.ToString(job["JobTitle"]),
                                                           Designation = Convert.ToString(job["Designation"]),
                                                           Experience = Convert.ToString(job["Experience"]),
                                                           SkillsSet = Convert.ToString(job["ReqSkills"]),
                                                           Organization = Convert.ToString(job["Organization"]),
                                                           Location = Convert.ToString(job["JobLocation"]),
                                                           Country = Convert.ToString(job["CountryName"]),
                                                           Description = Convert.ToString(job["JobDesc"]),
                                                           PostedDate = Convert.ToString(job["PostedDate"]),
                                                           CompUri = Convert.ToString(job["CompUri"]),
                                                           CompanyLogo = Convert.ToString(job["CompanyLogo"]),
                                                           CompanyProfile = Convert.ToString(job["CompProfile"]),
                                                           CloseDate = Convert.ToString(job["CloseDate"]),
                                                           JobType = Convert.ToString(job["JobType"]),

                                                           Salary = (Convert.ToString(job["SalaryType"]) == "CONFIDN") ? "Confidential" :
                                                                    (string.Format("{0} {1} - {2}", Convert.ToString(job["CurrencyType"]),
                                                                    Convert.ToString(job["MinSalary"]), Convert.ToString(job["MaxSalary"]))),

                                                           Education = Convert.ToString(job["Education"]),
                                                           Responsibity = Convert.ToString(job["Responsiblity"]),
                                                           ContactPerson = Convert.ToString(job["ContactPerson"]),
                                                           ContactEmail = Convert.ToString(job["ContactEmail"]),
                                                           ContactPhone = Convert.ToString(job["ContactPhone"]),
                                                           Url = Convert.ToString(job["WebUrl"]),   // Job external link.
                                                           IsWalkin = Convert.ToBoolean(job["IsWalkIn"]),
                                                       });

            return jobs.FirstOrDefault();
        }


        /// <summary>
        /// Manage history for user's interested jobs based on search.
        /// </summary>
        /// <param name="userId">User numeric id</param>
        /// <param name="jobId">Job numeric id</param>
        /// <param name="isApplied">True: If applied to the job</param>
        /// <param name="action">GET/POST</param>
        public string JobSearchHistory(long userId, long jobId, bool isApplied, string action)
        {
            return objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                                new object[] { userId, jobId, isApplied, action }, ""), string.Empty);
        }


        /// <summary>
        /// Get list of profile related jobs:
        /// </summary>
        /// <param name="session"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<ProfileJobs> GetDesiredJobs(string session, string token)
        {
            DataTable tbl = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                new object[] { ticket.Tocken, ticket.Email }, ""), string.Empty);

            List<ProfileJobs> jobs = new List<ProfileJobs>(from a in tbl.AsEnumerable()
                                                           select new ProfileJobs
                                                           {
                                                               Desc = Convert.ToString(a["Desc"]),
                                                               JobsCount = Convert.ToString(a["Jobs"]),
                                                               Keywords = Convert.ToString(a["Keyword"])
                                                           });

            return jobs;
        }


        /// <summary>
        /// Get detailed list of profile related jobs:
        /// </summary>
        /// <param name="session"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<JobSearchResult> GetDesiredJobsList(string desc, string session, string token)
        {
            DataTable tbl = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                new object[] { desc, ticket.Tocken, ticket.Email }, ""), string.Empty);

            if (tbl == null)
                return null;

            List<JobSearchResult> jobs = new List<JobSearchResult>(from job in tbl.AsEnumerable()
                                                                   select new JobSearchResult
                                                                   {
                                                                       JobId = job["JobId"].ToString(),
                                                                       JobCode = Convert.ToString(job["JobRefCode"]),
                                                                       JobTitle = Convert.ToString(job["JobTitle"]),
                                                                       Designation = Convert.ToString(job["Designation"]),
                                                                       Experience = Convert.ToString(job["Experience"]),
                                                                       SkillsSet = Convert.ToString(job["ReqSkills"]),
                                                                       Organization = Convert.ToString(job["Organization"]),
                                                                       Location = Convert.ToString(job["JobLocation"]),
                                                                       Description = Convert.ToString(job["JobDesc"]),
                                                                       PostedDate = Convert.ToString(job["PostedDate"]),
                                                                       CompanyLogo = Convert.ToString(job["CompanyLogo"])
                                                                   });

            return jobs;
        }


        /// <summary>
        /// Method to return complete profile to generate sys-generated resume.
        /// </summary>
        public CandidateProfile GetProfileDataForPdf(string id)
        {
            Nullable<Int64> profileId = null;

            if (string.IsNullOrEmpty(id))
            {
                // Get the profile id.
                id = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Substring(HttpContext.Current.Request.UrlReferrer.AbsoluteUri.LastIndexOf('/') + 1);
            }

            try
            {
                // If not accessing by self (talent).
                if (!string.IsNullOrEmpty(id))
                    profileId = Int64.Parse(id);
            }
            catch { }


            CandidateProfile profileInfo = new CandidateProfile();
            DataSet profileDtl = ViewProfile(ticket.Tocken, ticket.Email, profileId);
            string _name = string.Empty;

            #region Persional

            profileInfo.PersonalDetail = new List<PersonalProfile>(from pf in profileDtl.Tables[0].AsEnumerable()
                                                                   select new PersonalProfile
                                                                   {
                                                                       Name = Convert.ToString(pf["Name"]),
                                                                       Phone = Convert.ToString(pf["Phone"]),
                                                                       AltNum = Convert.ToString(pf["AlternateNum"]),
                                                                       Email = Convert.ToString(pf["Email"]),
                                                                       BirthDate = Convert.ToString(pf["DoB"]),
                                                                       Gender = Convert.ToString(pf["Gender"]),
                                                                       Address = Convert.ToString(pf["Address"]),
                                                                       City = Convert.ToString(pf["City"]),
                                                                       PostalCode = Convert.ToString(pf["PostalCode"]),
                                                                       Country = Convert.ToString(pf["Country"]),
                                                                       LanguagesKnown = Convert.ToString(pf["LanguagesKnown"]),
                                                                       MaritalStatus = Convert.ToString(pf["MarritalStatus"]),
                                                                       PassportStatus = Convert.ToString(pf["PassportStatus"]),
                                                                       VisaStatus = Convert.ToString(pf["VisaStatus"]),
                                                                       VisaCountry = Convert.ToString(pf["VisaCountry"]),
                                                                       VisaExpireDate = Convert.ToString(pf["VisaExpireDate"]),
                                                                       Photo = "../.." + ConfigurationManager.AppSettings["ProfilePic"].ToString() + Convert.ToString(pf["Photo"]),
                                                                       CTC = Convert.ToString(pf["CTC"]),
                                                                       CTCCurrencyType = Convert.ToString(pf["CurrencyCTC"]),
                                                                       ECTC = Convert.ToString(pf["ECTC"]),
                                                                       ECTCCurrencyType = Convert.ToString(pf["CurrencyECTC"]),
                                                                       NoticePeriod = Convert.ToString(pf["NoticePeriod"]),
                                                                       PrimarySkills = Convert.ToString(pf["PrimarySkills"]),
                                                                       SecondarySkills = Convert.ToString(pf["SecondarySkills"]),
                                                                       SummaryProfile = Convert.ToString(pf["SummaryProfile"]).Replace("\n", "<br />"),
                                                                       ResumeTitle = Convert.ToString(pf["Title"]),
                                                                       Designation = Convert.ToString(pf["Designation"]),
                                                                       Education = Convert.ToString(pf["Education"]),
                                                                       UpdatedOn = Convert.ToString(pf["UpdatedOn"]),
                                                                       ProfileCompleted = Convert.ToString(pf["ProfComplete"]),
                                                                       ResumeAttached = Convert.ToString(pf["CvAttachment"])

                                                                   }).FirstOrDefault();

            #endregion Persional

            #region Organization
            profileInfo.OrganizationalDetail = new List<OrganizationalProfile>(from org in profileDtl.Tables[1].AsEnumerable()
                                                                               select new OrganizationalProfile
                                                                               {
                                                                                   OrganizationName = Convert.ToString(org["OrganizationName"]),
                                                                                   DateFrom = Convert.ToString(org["FromDate"]),
                                                                                   ToDate = Convert.ToString(org["ToDate"]),
                                                                                   City = Convert.ToString(org["Location"]),
                                                                                   Designation = Convert.ToString(org["Designation"])
                                                                                   //CountryName = Convert.ToString(org["CountryName"])
                                                                                   //CompanyType = (Convert.ToString(org["Flag"]) == "CO") ? true : false
                                                                               });
            #endregion Organization

            #region Projects
            profileInfo.ProjectsDetail = new List<ProjectDetail>(from proj in profileDtl.Tables[2].AsEnumerable()
                                                                 select new ProjectDetail
                                                                 {
                                                                     Title = Convert.ToString(proj["ProjTitle"]),
                                                                     Synopsis = Convert.ToString(proj["Synopsis"]),
                                                                     //ToolsUsed = Convert.ToString(proj["ToolsUsed"]),
                                                                     RolePlayed = Convert.ToString(proj["RolePlayed"]),
                                                                     Duration = Convert.ToString(proj["Duration"])
                                                                 });
            #endregion Projects


            #region Graduation
            profileInfo.Graduation = new List<EducationalProfile>(from ug in profileDtl.Tables[3].AsEnumerable()
                                                                  select new EducationalProfile
                                                                  {
                                                                      Degree = Convert.ToString(ug["Degree"]),
                                                                      Course = Convert.ToString(ug["Course"]),
                                                                      InstituteName = Convert.ToString(ug["InstituteName"]),
                                                                      PassedYear = Convert.ToString(ug["PassedYear"]),
                                                                      University = Convert.ToString(ug["University"])
                                                                  }).FirstOrDefault();

            #endregion Graduation

            #region Post-Graduation
            profileInfo.PostGraduation = new List<EducationalProfile>(from ug in profileDtl.Tables[4].AsEnumerable()
                                                                      select new EducationalProfile
                                                                      {
                                                                          Degree = Convert.ToString(ug["Degree"]),
                                                                          Course = Convert.ToString(ug["Course"]),
                                                                          InstituteName = Convert.ToString(ug["InstituteName"]),
                                                                          PassedYear = Convert.ToString(ug["PassedYear"]),
                                                                          University = Convert.ToString(ug["University"])
                                                                      });
            #endregion Post-Graduation

            #region Under-Graduation
            profileInfo.UnderGraduation = new List<EducationalProfile>(from ug in profileDtl.Tables[5].AsEnumerable()
                                                                       select new EducationalProfile
                                                                       {
                                                                           Degree = Convert.ToString(ug["CourseName"]),
                                                                           Course = Convert.ToString(ug["Discipline"]),
                                                                           InstituteName = Convert.ToString(ug["InstituteName"]),
                                                                           PassedYear = Convert.ToString(ug["PassedYear"]),
                                                                           University = Convert.ToString(ug["Board"])
                                                                       });
            #endregion Under-Graduation

            return profileInfo;
        }


        /// <summary>
        /// Manage cover letter by job-seeker.
        /// </summary>
        /// <param name="session">Session</param>
        /// <param name="token">E-mail</param>
        /// <param name="text">Text description</param>
        /// <returns></returns>
        public string ManageCoverLetter(string session, string token, string letter, string action)
        {
            if (ticket != null && session != ticket.Tocken)
                throw new ArgumentException("INVALID_REQ");

            return objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                new object[] { ticket.Tocken, ticket.Email, letter, action }, ""), string.Empty);
        }


        /// <summary>
        /// Activate/deactivate user's subscription.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="isSubscribe"></param>
        /// <returns></returns>
        public string ManageSubscription(string email, bool? isSubscribe)
        {

            return objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                                             new object[] { email, isSubscribe }, ""), string.Empty);
        }


        /// <summary>
        /// Submit an apllication, from a user, for a particular job.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="token"></param>
        /// <returns>Return code: 1 =  Success; -1 = Candidate already applied for the job; 0 = Void/fail.</returns>
        public string PostJobApplication(long jobId, string session, string token)
        {
            string status = objProvider.ExecuteScalar(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { jobId, ticket.Tocken, ticket.Email }, ""), string.Empty);

            if (status.Equals("-1"))
                return "ALREADY_APPLIED";
            else if (status.Equals("1"))
                return "OK";
            else
                return "ERR";
        }


        /// <summary>
        /// Get organization profile
        /// </summary>
        /// <param name="session"></param>
        /// <param name="token"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public CompanyProfile GetCompanyProfile(string session, string token, int? profileId)
        {
            DataProvider objProvider = new DataProvider();
            profileId = int.Parse(ticket.Id.Replace("#", ""));
            DataTable dtProfile = objProvider.GetDataTable(AseCommandGenerator.GenerateCommand((MethodInfo)MethodBase.GetCurrentMethod(),
                            new object[] { ticket.Tocken, ticket.Email, profileId }, ""), string.Empty);

            CompanyProfile profile = null;

            if (dtProfile != null && dtProfile.Rows.Count > 0)
            {
                profile = new CompanyProfile
                {
                    CompanyName = Convert.ToString(dtProfile.Rows[0]["OrganizationName"]),
                    AuthorizedPerson = Convert.ToString(dtProfile.Rows[0]["AuthPerson"]),
                    AuthorizedPersonPhone = Convert.ToString(dtProfile.Rows[0]["AuthPerPhone"]),
                    AuthorizedPersonDesignation = Convert.ToString(dtProfile.Rows[0]["AuthPerDesignation"]),
                    IndustryType = Convert.ToString(dtProfile.Rows[0]["IndustryType"]),
                    Website = Convert.ToString(dtProfile.Rows[0]["Website"]),
                    Address = Convert.ToString(dtProfile.Rows[0]["Address"]),
                    City = Convert.ToString(dtProfile.Rows[0]["City"]),
                    CountryName = Convert.ToString(dtProfile.Rows[0]["Country"]),
                    CompanyLogo = Convert.ToString(dtProfile.Rows[0]["CompLogo"]),
                    Profile = Convert.ToString(dtProfile.Rows[0]["CompProfile"]),
                    Strength = string.IsNullOrEmpty(Convert.ToString(dtProfile.Rows[0]["Strength"])) ? null : (int?)int.Parse(Convert.ToString(dtProfile.Rows[0]["Strength"]))
                };
            }

            return profile;
        }

    }
}