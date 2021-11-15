using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class ActivityCannotBeAlreadyOnTimeLine : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Acvity is already on timeline.";

        private readonly Reader _reader;
        private readonly Activity _activity;

        public ActivityCannotBeAlreadyOnTimeLine(Reader reader, Activity activity)
        {
            _reader = reader;
            _activity = activity;
        }

        public bool IsFulfilled() => !_reader.TimeLine.HasActivity(_activity);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}