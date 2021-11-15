using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class ReaderMustHaveTimeline : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Reader must have timeline.";

        private readonly Reader _reader;

        public ReaderMustHaveTimeline(Reader reader)
        {
            _reader = reader;
        }

        public bool IsFulfilled() => _reader.TimeLine != null;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}