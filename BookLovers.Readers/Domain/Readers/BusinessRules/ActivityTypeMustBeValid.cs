using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class ActivityTypeMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Invalid activity type.";

        private readonly Activity _activity;

        public ActivityTypeMustBeValid(Activity activity) => _activity = activity;

        public bool IsFulfilled()
        {
            return _activity.Content.ActivityType != null
                   && Activities.Types.Contains(_activity.Content.ActivityType);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}