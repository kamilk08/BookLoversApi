using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Publication.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Root;

namespace BookLovers.Bookcases.Tests.EndToEndTests.RemoveFromShelfTests
{
    internal class RemoveFromReadShelfPublicationModuleProbe : IProbe
    {
        private readonly IModule<PublicationModule> _module;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        public RemoveFromReadShelfPublicationModuleProbe(IModule<PublicationModule> module)
        {
            _module = module;
        }

        public bool IsSatisfied()
        {
            return AreOutboxMessagesProcessed() && AreInboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var outboxMessagesQuery =
                await _module.ExecuteQueryAsync<GetBooksOutboxMessagesQuery, List<OutboxMessage>>(
                    new GetBooksOutboxMessagesQuery());

            var inboxMessagesQuery =
                await _module.ExecuteQueryAsync<GetBooksInboxMessagesQuery, List<InBoxMessage>>(
                    new GetBooksInboxMessagesQuery());

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