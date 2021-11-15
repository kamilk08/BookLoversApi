using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Ratings.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Root;

namespace BookLovers.Auth.Tests.EndToEndTests.BlockAccount
{
    internal class BlockAccountRatingsModuleProbe : IProbe
    {
        private readonly IModule<RatingsModule> _module;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        public BlockAccountRatingsModuleProbe(IModule<RatingsModule> module)
        {
            _module = module;
        }

        public bool IsSatisfied()
        {
            return AreInboxMessagesProcessed() && AreOutboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var outboxQueryResult = await _module.ExecuteQueryAsync<GetRatingsOutboxMessagesQuery, List<OutboxMessage>>(
                new GetRatingsOutboxMessagesQuery());

            var inboxQueryResult = await _module.ExecuteQueryAsync<GetRatingsInboxMessagesQuery, List<InBoxMessage>>(
                new GetRatingsInboxMessagesQuery());

            _outboxMessages = outboxQueryResult.Value;
            _inBoxMessages = inboxQueryResult.Value;
        }

        private bool AreOutboxMessagesProcessed()
        {
            return _outboxMessages != null && _outboxMessages.Count == 0;
        }

        private bool AreInboxMessagesProcessed()
        {
            return _inBoxMessages != null && _inBoxMessages.Count == 0;
        }
    }
}