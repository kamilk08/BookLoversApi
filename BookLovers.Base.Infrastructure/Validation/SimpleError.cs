namespace BookLovers.Base.Infrastructure.Validation
{
    public class SimpleError : ValidationError
    {
        public SimpleError(string errorProperty, string errorMessage)
            : base(errorProperty, errorMessage)
        {
        }

        public override string FormatErrorProperty(string error)
        {
            return error;
        }
    }
}