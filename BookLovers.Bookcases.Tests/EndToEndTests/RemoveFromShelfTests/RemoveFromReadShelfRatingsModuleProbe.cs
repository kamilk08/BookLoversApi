using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Ratings.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Root;

namespace BookLovers.Bookcases.Tests.EndToEndTests.RemoveFromShelfTests
{
    internal class RemoveFromReadShelfRatingsModuleProbe : IProbe
    {
        private readonly IModule<RatingsModule> _ratingsModule;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        public RemoveFromReadShelfRatingsModuleProbe(IModule<RatingsModule> ratingsModule)
        {
            _ratingsModule = ratingsModule;
        }

        public bool IsSatisfied()
        {
            return AreInboxMessagesProcessed() && AreOutboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var outboxMessagesQuery =
                await _ratingsModule.ExecuteQueryAsync<GetRatingsOutboxMessagesQuery, List<OutboxMessage>>(
                    new GetRatingsOutboxMessagesQuery());

            var inboxMessagesQuery =
                await _ratingsModule.ExecuteQueryAsync<GetRatingsInboxMessagesQuery, List<InBoxMessage>>(
                    new GetRatingsInboxMessagesQuery());

            _outboxMessages = outboxMessagesQuery.Value;
            _inBoxMessages = inboxMessagesQuery.Value;
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