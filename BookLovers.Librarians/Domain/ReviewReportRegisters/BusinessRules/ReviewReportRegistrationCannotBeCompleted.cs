using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.ReviewReportRegisters.BusinessRules
{
    internal class ReviewReportRegistrationCannotBeCompleted : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Review report registration already closed.";

        private readonly ReviewReportRegister _reportRegister;

        public ReviewReportRegistrationCannotBeCompleted(ReviewReportRegister reportRegister)
        {
            this._reportRegister = reportRegister;
        }

        public bool IsFulfilled()
        {
            return !this._reportRegister.SolvedBy.HasBeenSolved();
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}