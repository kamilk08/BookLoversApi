using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class ActivityMustBeHidden : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Activity already vissible.";

        private readonly Activity _activity;

        public ActivityMustBeHidden(Activity activity)
        {
            _activity = activity;
        }

        public bool IsFulfilled() => !_activity.ShowToOthers;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}