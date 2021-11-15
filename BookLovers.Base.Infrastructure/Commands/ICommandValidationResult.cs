using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Validation;

namespace BookLovers.Base.Infrastructure.Commands
{
    public interface ICommandValidationResult
    {
        string Status { get; }

        bool HasErrors { get; }

        IEnumerable<ValidationError> Errors { get; }
    }
}