using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class ActivityMustBeOnTimeLine : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Activity missing on timeline.";

        private readonly Reader _reader;
        private readonly Activity _activity;

        public ActivityMustBeOnTimeLine(Reader reader, Activity activity)
        {
            _reader = reader;
            _activity = activity;
        }

        public bool IsFulfilled()
        {
            if (_activity == null) return false;

            return _activity.Content.ActivityType != null
                   && _reader.TimeLine.TimeLineActivities.Contains(_activity);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}