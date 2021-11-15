using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Publication.Domain.Quotes.BusinessRules
{
    internal class QuoteCanBeOnlyManagedByItsCreatorOrLibrarian : IBusinessRule
    {
        private const string BrokenRuleErrorMessage =
            "Quote can be only archived by creator of the quoute or librarian.";

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly Quote _quote;

        public QuoteCanBeOnlyManagedByItsCreatorOrLibrarian(
            IHttpContextAccessor contextAccessor,
            Quote quote)
        {
            this._contextAccessor = contextAccessor;
            this._quote = quote;
        }

        public bool IsFulfilled()
        {
            return this._quote.QuoteDetails.AddedByGuid ==
                   this._contextAccessor.UserGuid ||
                   this._contextAccessor.IsLibrarian();
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}