using BookLovers.Base.Domain.Rules;
using BookLovers.Shared;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class ReaderCannotFollowHimself : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Reader cannot follow himself.";

        private readonly Reader _reader;
        private readonly Follower _follower;

        public ReaderCannotFollowHimself(Reader reader, Follower follower)
        {
            _reader = reader;
            _follower = follower;
        }

        public bool IsFulfilled() => _follower.FollowedBy != _reader.Guid;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}