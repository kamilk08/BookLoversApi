namespace BookLovers.Base.Infrastructure.Validation
{
    public abstract class ValidationError
    {
        public string ErrorMessage { get; }

        public string ErrorProperty { get; }

        public ValidationError(string errorProperty, string errorMessage)
        {
            ErrorProperty = FormatErrorProperty(errorProperty);
            ErrorMessage = errorMessage;
        }

        public abstract string FormatErrorProperty(string error);
    }
}