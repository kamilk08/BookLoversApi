using System.Linq;

namespace BookLovers.Base.Infrastructure.Validation
{
    public class FluentValidationError : ValidationError
    {
        public FluentValidationError(string errorProperty, string errorMessage)
            : base(errorProperty, errorMessage)
        {
        }

        public override string FormatErrorProperty(string error)
        {
            return error.Split('.').Last();
        }
    }
}