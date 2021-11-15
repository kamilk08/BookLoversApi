using System.Text.RegularExpressions;

namespace BookLovers.Base.Infrastructure.Validation
{
    public class PasswordValidator
    {
        public static bool HasNumber(string password)
        {
            return new Regex("[0-9]").IsMatch(password);
        }

        public static bool HasUpperCase(string password)
        {
            return new Regex("[A-Z]+").IsMatch(password);
        }
    }
}