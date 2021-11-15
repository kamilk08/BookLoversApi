using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Librarians.Domain.ReviewReportRegisters.BusinessRules
{
    internal class ReviewReportCannotBeDuplicated : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Review report cannot be added twice by the same reader.";

        private readonly ReviewReportRegister _reportRegister;
        private readonly ReportRegisterItem _reportRegisterItem;

        public ReviewReportCannotBeDuplicated(
            ReviewReportRegister reportRegister,
            ReportRegisterItem reportRegisterItem)
        {
            this._reportRegister = reportRegister;
            this._reportRegisterItem = reportRegisterItem;
        }

        public bool IsFulfilled()
        {
            return !this._reportRegister.ReviewReports.Contains<ReportRegisterItem>(this._reportRegisterItem);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}