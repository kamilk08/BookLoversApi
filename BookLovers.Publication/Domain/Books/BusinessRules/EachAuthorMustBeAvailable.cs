using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Authors;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    public class EachAuthorMustBeAvailable : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "One of the authors does not exist";

        private readonly List<Author> _authors;

        public EachAuthorMustBeAvailable(List<Author> authors)
        {
            this._authors = authors;
        }

        public bool IsFulfilled()
        {
            foreach (var author in this._authors)
            {
                if (author == null || author.Guid == Guid.Empty)
                    return false;
            }

            return true;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}