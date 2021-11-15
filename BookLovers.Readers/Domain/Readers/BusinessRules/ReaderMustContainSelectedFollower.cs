using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class ReaderMustContainSelectedFollower : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Reader does not have selected follower.";

        private readonly Reader _reader;
        private readonly Follower _follower;

        public ReaderMustContainSelectedFollower(Reader reader, Follower follower)
        {
            _reader = reader;
            _follower = follower;
        }

        public bool IsFulfilled()
        {
            return _reader.Followers.Contains(_follower);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}