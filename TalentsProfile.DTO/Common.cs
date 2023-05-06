using System;
using System.Runtime.Serialization;
using TalentsProfile.DataAnnotations;
using System.ComponentModel.DataAnnotations;


namespace TalentsProfile.DTO
{
    /// <summary>
    /// Login class validates defined user id and password rules.
    /// </summary>
    public class Login
    {
        [Required(ErrorMessage = "Please enter e-mail.", AllowEmptyStrings = false)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid e-mail")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Please enter password.", AllowEmptyStrings = false)]
        [Password(ErrorMessage = "Invalid pasword.")]
        public String Password { get; set; }
    }


    /// <summary>
    /// New registration for job-seeker.
    /// </summary>
    public class Registeration
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid e-mail")]
        [Required(ErrorMessage = "E-mail required", AllowEmptyStrings = false)]
        [StringLength(60, ErrorMessage = "Email is either invalid or too long", MinimumLength = 5)]
        //[RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid e-mail", ErrorMessageResourceName = "Email")]
        public String Email { get; set; }

        //[Required(ErrorMessage = "Please enter password", AllowEmptyStrings = false, ErrorMessageResourceName = "Password")]
        //[Password(ErrorMessage = "Password should be in between 6-15 characters", ErrorMessageResourceName = "Password")]
        //public String Password { get; set; }

        //[Required(ErrorMessage = "Please enter re-type password", AllowEmptyStrings = false, ErrorMessageResourceName = "Re-type password")]
        //[Password(ErrorMessage = "Password should be in between 6-15 characters", ErrorMessageResourceName = "Password")]
        //public String ConfirmPassword { get; set; }

        //public UserType UType { get; set; }

        [Required(ErrorMessage = "Terms and conditions not accepted", AllowEmptyStrings = false)]
        public Boolean IsAgreed { get; set; }

        [Required(ErrorMessage = "Country required", AllowEmptyStrings = false)]
        public int Country { get; set; }

        [Required(ErrorMessage = "First name required", AllowEmptyStrings = false)]
        [StringLength(24, ErrorMessage = "Invalid first name", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name required", AllowEmptyStrings = false)]
        [StringLength(24, ErrorMessage = "Invalid last name", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender required", AllowEmptyStrings = false)]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Captcha required", AllowEmptyStrings = false)]
        public string Captcha { get; set; }
    }


    /// <summary>
    /// New registration for job-seeker/employer.
    /// </summary>
    public class EmployerRegisteration
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid e-mail")]
        [Required(ErrorMessage = "E-mail required", AllowEmptyStrings = false)]
        [StringLength(60, ErrorMessage = "Email is either invalid or too long", MinimumLength = 5)]
        public String Email { get; set; }

        [Required(ErrorMessage = "Company name required", AllowEmptyStrings = false)]
        [StringLength(60, ErrorMessage = "Invalid company name", MinimumLength = 2)]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "Terms and conditions not accepted", AllowEmptyStrings = false)]
        public Boolean IsAgreed { get; set; }

        [Required(ErrorMessage = "Country required", AllowEmptyStrings = false)]
        public int Country { get; set; }

        //[Required(ErrorMessage = "Registrant first name required", AllowEmptyStrings = false)]
        //[StringLength(24, ErrorMessage = "Invalid first name", MinimumLength = 3)]
        //public string RegistrantFirstName { get; set; }

        [Required(ErrorMessage = "Registrant Last name required", AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Invalid last name", MinimumLength = 3)]
        public string RegistrantFullName { get; set; }

        //[Required(ErrorMessage = "Registrant gender required", AllowEmptyStrings = false)]
        //public char Gender { get; set; }

        [Required(ErrorMessage = "Registrant designation required", AllowEmptyStrings = false)]
        [StringLength(30, ErrorMessage = "Invalid designation", MinimumLength = 2)]
        public string RegistrantDesignation { get; set; }

        [Required(ErrorMessage = "Captcha required", AllowEmptyStrings = false)]
        public string Captcha { get; set; }
    }


    /// <summary>
    /// User ticket after successfull credential.
    /// </summary>
    [Serializable]
    public class UserTicket : ISerializable
    {
        public String Email { get; set; }
        public String Tocken { get; set; }
        public String Name { get; set; }
        public String UType { get; set; }
        public String Id { get; set; }

        // Add serialization method
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Email", Email);
            info.AddValue("Tocken", Tocken);
            info.AddValue("Name", Name);
            info.AddValue("UType", UType);
            info.AddValue("Id", Id);
        }


        // Deserialize object
        public UserTicket(SerializationInfo info, StreamingContext context)
        {
            Email = (String)info.GetValue("Email", typeof(string));
            Tocken = (String)info.GetValue("Tocken", typeof(string));
            Name = (String)info.GetValue("Name", typeof(string));
            UType = (String)info.GetValue("UType", typeof(string));
            Id = (String)info.GetValue("Id", typeof(string));
        }


        // Default
        public UserTicket()
        {
        }
    }



    /// <summary>
    /// Generic class to bind the lookup DDL items.
    /// </summary>
    public class ListLookup
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
