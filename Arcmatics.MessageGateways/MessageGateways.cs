// Base class is used to define message gateways/channels settings.
// Possible modes: SMTP mail, SMS Gateway.
// By: Navin Pandit.

using System;
using System.Net.Mail;
using System.Configuration;

namespace Communicator.Message
{
    // Define one time initialization base classes.
    static class MessageGateways
    {
        // Defined static private variables:
        public static string MailFrom = string.Empty;
        static string port = string.Empty;
        static string password = string.Empty;
        static string host = string.Empty;

        static MessageGateways()
        {
            MailFrom = ConfigurationManager.AppSettings["FromMail"].ToString();
            password = ConfigurationManager.AppSettings["Pwd"].ToString();
            host = ConfigurationManager.AppSettings["Host"].ToString();
            port = ConfigurationManager.AppSettings["Port"].ToString();
        }


        /// <summary>
        /// Provide SMTP credentials to send SMTP mail.
        /// </summary>
        /// <returns></returns>
        public static SmtpClient GetSmtpClient(bool enableSssl)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = host;
            smtp.Credentials = new System.Net.NetworkCredential(MailFrom, password);
            smtp.EnableSsl = enableSssl;
            return smtp;
        }
    }
}
