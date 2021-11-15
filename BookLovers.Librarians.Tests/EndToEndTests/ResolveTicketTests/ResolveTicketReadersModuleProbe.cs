using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Readers.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Root;

namespace BookLovers.Librarians.Tests.EndToEndTests.ResolveTicketTests
{
    internal class ResolveTicketReadersModuleProbe : IProbe
    {
        private readonly IModule<ReadersModule> _module;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        public ResolveTicketReadersModuleProbe(IModule<ReadersModule> module)
        {
            _module = module;
        }

        public bool IsSatisfied()
        {
            return AreInboxMessagesProcessed() && AreOutboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var outboxQueryResult = await _module.ExecuteQueryAsync<GetReadersOutboxMessagesQuery, List<OutboxMessage>>(
                new GetReadersOutboxMessagesQuery());

            var inboxQueryResult = await _module.ExecuteQueryAsync<GetReadersInboxMessagesQuery, List<InBoxMessage>>(
                new GetReadersInboxMessagesQuery());

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