// This class is used to send a notification via different mode.
// Possible modes: SMTP mail, SMS Gateway.
// By: Navin Pandit.

// By: Navin Pandit.
using System;
using System.Xml;
using System.Web;
using System.Net.Mail;
using Communicator.Message;
using System.Configuration;
using System.IO;

namespace Communicator.Notification
{
    public class Message : IDisposable
    {
        // Define private variables
        string _subject = string.Empty;
        string _message = string.Empty;
        string _emailTo = string.Empty;
        string _receiverName = string.Empty;

        SmtpClient smtpClient;

        public string Phone { get; set; }
        public string Link { get; set; }
        public string Remark { get; set; }
        public string ApplicationNumber { get; set; }

        public MailType SmtpMailType { get; set; }

        public string MessageBody { get; set; }
        public string SenderName { get; set; }

        public string SenderEmail { get; set; }
        public string MailSubject { get; set; }

        public Message(string ReceiverName, string EmailId, string subject, bool EnableSSL = false)
        {
            _receiverName = ReceiverName;
            _emailTo = EmailId;
            _subject = subject;
            smtpClient = MessageGateways.GetSmtpClient(EnableSSL);
        }

        public Message(bool EnableSSL = false)
        {
            smtpClient = MessageGateways.GetSmtpClient(EnableSSL);
        }

        /// <summary>
        /// Read the mail format to set message body and subject line.
        /// </summary>
        private void MailFormatter()
        {
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            XmlDocument doc = new XmlDocument();
            doc.Load(path + "/format/Mail.xml");

            XmlNode subjNode;
            XmlNode msgNode;

            subjNode = doc.SelectSingleNode("MailFormatter/" + SmtpMailType.ToString() + "/Subject");
            msgNode = doc.SelectSingleNode("MailFormatter/" + SmtpMailType.ToString() + "/MailBody");
            _subject = subjNode.InnerText;
            _message = msgNode.InnerText;

            if (String.IsNullOrEmpty(_subject))
                throw new ArgumentException("Subject line is missing. You must define subject line before sending a mail.");

            if (String.IsNullOrEmpty(_message))
                throw new ArgumentException("Message body is empty. You must provide message body to send a mail.");

            if (SmtpMailType.Equals(MailType.OTR))
            {
                Link = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/Home/Activate?uac=" + Link + "&reg=" + EncryptText(_emailTo) + "&usr=new");
                _message = _message.Replace("$name$", _receiverName).Replace("$link$", Link);
            }

            if (SmtpMailType.Equals(MailType.FORGET_PASSWORD))
            {
                Link = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/Home/Reset?fa=" + EncryptText(Link) + "&ac=" + EncryptText(_emailTo) + "&usr=fgt");
                _message = _message.Replace("$name$", _receiverName).Replace("$email$", _emailTo).Replace("$link$", Link);
            }

            if (SmtpMailType.Equals(MailType.APPROVED))
            {
                _message = _message.Replace("$name$", _receiverName).Replace("$applicationCode$", ApplicationNumber);
            }

            if (SmtpMailType.Equals(MailType.REJECTED))
            {
                _message = _message.Replace("$name$", _receiverName).Replace("$applicationCode$", ApplicationNumber).Replace("$remarks$", Remark);
            }
            if (SmtpMailType.Equals(MailType.CARD_GEN))
            {
                _message = _message.Replace("$name$", _receiverName).Replace("$applicationCode$", ApplicationNumber);
            }
            else
            {
            }
        }

        private void HtmlMailFormatter()
        {
            Arcmatics.Security.Cryptography encrypt = new Arcmatics.Security.Cryptography();
            string path = HttpContext.Current.Request.PhysicalApplicationPath;

            if (SmtpMailType.Equals(MailType.OTR))
            {
                _message = File.ReadAllText(path + "/Contents/AccActivation.htm");
                Link = ConfigurationManager.AppSettings["BaseUri"].ToString() + "/account?uac=" + encrypt.EncryptString(_emailTo) + "&usr=new";
            }
            else if (SmtpMailType.Equals(MailType.FORGET_PASSWORD))
            {
                _message = File.ReadAllText(path + "/Contents/PasswordRequest.htm");
                Link = ConfigurationManager.AppSettings["BaseUri"].ToString() + "/account?emi=" + encrypt.EncryptString(_emailTo) + "&usr=reset";
            }

            _message = _message.Replace("$name$", _receiverName).Replace("$date$", DateTime.Now.ToString()).Replace("$url$", Link).Replace("$style$", ConfigurationManager.AppSettings["BaseUri"].ToString());
        }


        /// <summary>
        /// send SMTP mail in case user forget his password.
        /// </summary>
        public bool SendMail()
        {
            MailMessage mail = new MailMessage();

            if (!SmtpMailType.ToString().Equals("OTHER_HTML_MAIL"))
            {
                //MailFormatter();
                HtmlMailFormatter();

                //Provide host name and SMTP credentials.
                mail.To.Add(_emailTo);
                mail.From = new MailAddress(MessageGateways.MailFrom);

                //Define subject line and set text message.
                mail.Subject = _subject;
                mail.Body = _message;
                mail.IsBodyHtml = true;
                smtpClient.Send(mail);
            }
            else
            {
                mail.To.Add(MessageGateways.MailFrom);
                mail.From = new MailAddress(MessageGateways.MailFrom);

                // For feedback, email id may not be available.
                if (!this.MailSubject.ToLower().Contains("feedback"))
                    mail.ReplyToList.Add(new MailAddress(this.SenderEmail));

                mail.Subject = this.MailSubject;
                mail.Body = this.MessageBody + "</br></br>" + string.Format("From: {0}, </br>Email: {1}", this.SenderName, this.SenderEmail);
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;
                smtpClient.Send(mail);
            }
            return true;
        }


        /// <summary>
        /// Encrypt text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private String EncryptText(string text)
        {
            string encodedText = String.Empty;
            byte[] encodedBytes = null;

            if (!String.IsNullOrEmpty(text))
            {
                encodedBytes = System.Text.Encoding.Unicode.GetBytes(text);
                encodedText = System.Convert.ToBase64String(encodedBytes);
            }

            return encodedText;
        }


        /// <summary>
        /// Dispose the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}





