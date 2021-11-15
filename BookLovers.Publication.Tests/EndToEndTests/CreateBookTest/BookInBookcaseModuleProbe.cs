using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Bookcases.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Root;

namespace BookLovers.Publication.Tests.EndToEndTests.CreateBookTest
{
    public class BookInBookcaseModuleProbe : IProbe
    {
        private readonly IModule<BookcaseModule> _module;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        public BookInBookcaseModuleProbe(IModule<BookcaseModule> module)
        {
            _module = module;
        }

        public bool IsSatisfied()
        {
            return AreInboxMessagesProcessed() && AreOutboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var outboxQuery = await _module.ExecuteQueryAsync<GetBookcaseOutboxMessagesQuery, List<OutboxMessage>>(
                new GetBookcaseOutboxMessagesQuery());

            var inboxQuery = await _module.ExecuteQueryAsync<GetBookcaseInboxMessagesQuery, List<InBoxMessage>>(
                new GetBookcaseInboxMessagesQuery());

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