using System.ComponentModel.DataAnnotations;

namespace BookLovers.Base.Infrastructure.Validation
{
    public static class EmailValidator
    {
        public static bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}