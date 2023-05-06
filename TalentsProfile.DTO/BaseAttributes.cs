using System.ComponentModel.DataAnnotations;

namespace TalentsProfile.DataAnnotations
{
    /// <summary>
    /// Password required attributes
    /// </summary>
    public class PasswordAttribute : ValidationAttribute
    {
        int _minLen = 6;
        int _maxLen = 15;

        public override bool IsValid(object value)
        {
            if (value != null && value.ToString().Length >= _minLen && value.ToString().Length <= _maxLen)
                return true;
            else
                return false;
        }
    }


    /// <summary>
    /// Confirm password must be same as password.
    /// </summary>
    public class ConfirmPasswordAttribute : ValidationAttribute
    {
        object _password = string.Empty;

        public ConfirmPasswordAttribute(object password)
        {
            _password = password;
        }

        public override bool IsValid(object value)
        {
            if (value.ToString().Equals(_password.ToString()))
                return true;
            else
                return false;
        }
    }


    
}
