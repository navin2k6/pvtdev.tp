using System;

namespace Communicator.Notification
{
    /// <summary>
    /// Type of mail to send to a user.
    /// </summary>
    public enum MailType
    {
        //OTR: One Time Registration
        OTR = 1,

        //APPROVED: When user application approved.
        APPROVED,

        //REJECTED: When an appliation rejected at any stage.
        REJECTED,

        //FORGET: Forget password.
        FORGET_PASSWORD,

        //OTP:Generate Onet Time Password.
        OTP,

        //NOMINATED: When a application submitted.
        SUBMITTED,

        // Admit card mail notification
        CARD_GEN,

        /// <summary>
        /// To send an other HTML formatted mail, e.g. contact, feedback etc.
        /// </summary>
        OTHER_HTML_MAIL
    }
}
