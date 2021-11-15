using BookLovers.Base.Domain.Rules;
using BookLovers.Shared;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class ReaderCannotHaveMultipleSameFollowers : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Reader cannot have duplicated followers.";

        private readonly Reader _reader;
        private readonly Follower _follower;

        public ReaderCannotHaveMultipleSameFollowers(Reader reader, Follower follower)
        {
            _reader = reader;
            _follower = follower;
        }

        public bool IsFulfilled() => _reader.GetFollower(_follower.FollowedBy) == null;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}