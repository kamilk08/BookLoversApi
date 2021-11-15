namespace BookLovers.Base.Infrastructure.Validation
{
    public static class StringValidator
    {
        public static bool NoWhiteSpace(string title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }
    }
}