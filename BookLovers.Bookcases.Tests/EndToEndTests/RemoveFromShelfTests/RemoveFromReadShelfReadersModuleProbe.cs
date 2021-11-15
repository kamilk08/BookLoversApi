using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Readers.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Root;

namespace BookLovers.Bookcases.Tests.EndToEndTests.RemoveFromShelfTests
{
    public class RemoveFromReadShelfReadersModuleProbe : IProbe
    {
        private readonly IModule<ReadersModule> _module;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        public RemoveFromReadShelfReadersModuleProbe(IModule<ReadersModule> module)
        {
            _module = module;
        }

        public bool IsSatisfied()
        {
            return AreInboxMessagesProcessed() && AreOutboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var outboxMessagesQuery =
                await _module.ExecuteQueryAsync<GetReadersOutboxMessagesQuery, List<OutboxMessage>>(
                    new GetReadersOutboxMessagesQuery());

            var inboxMessagesQuery =
                await _module.ExecuteQueryAsync<GetReadersInboxMessagesQuery, List<InBoxMessage>>(
                    new GetReadersInboxMessagesQuery());

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