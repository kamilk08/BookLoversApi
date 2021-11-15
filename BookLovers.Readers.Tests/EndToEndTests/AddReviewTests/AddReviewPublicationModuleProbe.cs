using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Publication.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Root;

namespace BookLovers.Readers.Tests.EndToEndTests.AddReviewTests
{
    public class AddReviewPublicationModuleProbe
    {
        private readonly IModule<PublicationModule> _module;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        public AddReviewPublicationModuleProbe(IModule<PublicationModule> module)
        {
            _module = module;
        }

        public bool IsSatisfied()
        {
            return AreOutboxMessagesProcessed() && AreInboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var outboxQuery = await _module.ExecuteQueryAsync<GetBooksOutboxMessagesQuery, List<OutboxMessage>>(
                new GetBooksOutboxMessagesQuery());

            var inboxQuery = await _module.ExecuteQueryAsync<GetBooksInboxMessagesQuery, List<InBoxMessage>>(
                new GetBooksInboxMessagesQuery());

            _outboxMessages = outboxQuery.Value;
            _inBoxMessages = inboxQuery.Value;
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