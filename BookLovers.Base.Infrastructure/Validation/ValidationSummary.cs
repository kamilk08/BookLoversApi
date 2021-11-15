using System.Collections.Generic;
using System.Linq;

namespace BookLovers.Base.Infrastructure.Validation
{
    public class ValidationSummary
    {
        public string Type { get; }

        public string Title { get; }

        public int Status { get; }

        public Dictionary<string, object> Errors { get; }

        public ValidationSummary(
            IEnumerable<ValidationError> errors,
            string title,
            string type,
            int status)
        {
            var dict = new Dictionary<string, object>();

            var errorGroups = errors.GroupBy(p => p.ErrorProperty)
                .OrderBy(p => p.Count());

            foreach (var errorGroup in errorGroups)
            {
                if (errorGroup.Count() > 1)
                    dict.Add(errorGroup.Key, errorGroup);
                else dict.Add(errorGroup.Key, errorGroup.First());
            }

            this.Errors = dict;
            this.Title = title;
            this.Type = type;
            this.Status = status;
        }

        public static ValidationSummary InvalidCommand(
            IEnumerable<ValidationError> errors)
        {
            return new ValidationSummary(errors, "Invalid request", "https://foo/validation-error", 400);
        }

        public static ValidationSummary Unauthorized()
        {
            return new ValidationSummary(
                Enumerable.Empty<ValidationError>(),
                nameof(Unauthorized),
                "https://foo/unauthorized-error",
                401);
        }

        public static ValidationSummary Forbidden()
        {
            return new ValidationSummary(
                Enumerable.Empty<ValidationError>(),
                "Accessing resource that is forbidden.",
                "https://foo/forbidden-error",
                403);
        }

        public static ValidationSummary NotFound()
        {
            return new ValidationSummary(
                Enumerable.Empty<ValidationError>(),
                "Not found",
                "https://foo/not-found-error",
                404);
        }

        public static ValidationSummary MethodNotAllowed()
        {
            return new ValidationSummary(
                Enumerable.Empty<ValidationError>(),
                "Http method not allowed",
                "https://foo/not-allowed-error",
                405);
        }

        public static ValidationSummary InvalidBusinessRule(string errorContent)
        {
            return new ValidationSummary(
                new List<ValidationError>
                {
                    new SimpleError("ErrorKey", errorContent)
                },
                "Conflict",
                "https://foo/validation-error",
                409);
        }
    }
}