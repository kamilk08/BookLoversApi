using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Validation;

namespace BookLovers.Base.Infrastructure
{
    public class CommandValidationResult : ICommandValidationResult
    {
        public string Status => !HasErrors ? "Success" : "Failure";

        public bool HasErrors => Errors.Any();

        public IEnumerable<ValidationError> Errors { get; }

        protected CommandValidationResult(IEnumerable<ValidationError> errors)
        {
            Errors = errors;
        }

        public static CommandValidationResult SuccessResult()
        {
            return new CommandValidationResult(new List<ValidationError>());
        }

        public static CommandValidationResult FailureResult(
            IEnumerable<ValidationError> errors)
        {
            return new CommandValidationResult(errors);
        }
    }
}