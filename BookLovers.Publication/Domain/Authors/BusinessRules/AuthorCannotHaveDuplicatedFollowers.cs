using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal class AuthorCannotHaveDuplicatedFollowers : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Author cannot have duplicated followers.";

        private readonly Author _author;
        private readonly Follower _follower;

        public AuthorCannotHaveDuplicatedFollowers(Author author, Follower follower)
        {
            this._author = author;
            this._follower = follower;
        }

        public bool IsFulfilled() =>
            !this._author.Followers.Contains(this._follower);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}