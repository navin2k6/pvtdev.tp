using System;
using System.Text.RegularExpressions;

namespace TalentsProfile
{
    public static class ExtensionMethods
    {
        public static bool IsAlphabetOnly(this String text)
        {
            Regex rgx = new Regex("^[a-zA-Z]+$");
            return rgx.IsMatch(text);
        }

        public static bool IsAlphabetWithSpace(this String text)
        {
            Regex rgx = new Regex("^[a-zA-Z ]+$");
            return rgx.IsMatch(text);
        }

        public static bool IsValidEmail(this String text)
        {
            Regex rgx = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            return rgx.IsMatch(text);
        }
    }
}
