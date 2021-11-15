using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Publication.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Root;

namespace BookLovers.Auth.Tests.EndToEndTests.BlockAccount
{
    internal class BlockAccountPublicationModuleProbe : IProbe
    {
        private readonly IModule<PublicationModule> _module;

        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        public BlockAccountPublicationModuleProbe(IModule<PublicationModule> module)
        {
            _module = module;
        }

        public bool IsSatisfied()
        {
            return AreOutboxMessagesProcessed() && AreInboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var outboxQueryResult = await _module.ExecuteQueryAsync<GetBooksOutboxMessagesQuery, List<OutboxMessage>>(
                new GetBooksOutboxMessagesQuery());

            var inboxQueryResult = await _module.ExecuteQueryAsync<GetBooksInboxMessagesQuery, List<InBoxMessage>>(
                new GetBooksInboxMessagesQuery());

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