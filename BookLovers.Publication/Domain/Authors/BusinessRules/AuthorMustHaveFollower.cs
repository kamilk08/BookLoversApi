using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal class AuthorMustHaveFollower : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Author does not have selected follower.";

        private readonly Author _author;
        private readonly Follower _follower;

        public AuthorMustHaveFollower(Author author, Follower follower)
        {
            this._author = author;
            this._follower = follower;
        }

        public bool IsFulfilled() => this._author.Followers.Contains(this._follower);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}