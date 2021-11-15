using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class ReaderMustBeAssociatedWithProfile : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Reader must have profile.";

        private readonly Reader _reader;

        public ReaderMustBeAssociatedWithProfile(Reader reader)
        {
            this._reader = reader;
        }

        public bool IsFulfilled() => this._reader.Socials.ProfileGuid != Guid.Empty;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}