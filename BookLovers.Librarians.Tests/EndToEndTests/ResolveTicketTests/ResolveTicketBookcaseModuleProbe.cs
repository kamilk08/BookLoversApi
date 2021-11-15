using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Bookcases.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Root;

namespace BookLovers.Librarians.Tests.EndToEndTests.ResolveTicketTests
{
    internal class ResolveTicketBookcaseModuleProbe : IProbe
    {
        private readonly IModule<BookcaseModule> _module;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        public ResolveTicketBookcaseModuleProbe(IModule<BookcaseModule> module)
        {
            _module = module;
        }

        public bool IsSatisfied()
        {
            return AreInboxMessagesProcessed() && AreOutboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var outboxQueryResult =
                await _module.ExecuteQueryAsync<GetBookcaseOutboxMessagesQuery, List<OutboxMessage>>(
                    new GetBookcaseOutboxMessagesQuery());

            var inboxQueryResult = await _module.ExecuteQueryAsync<GetBookcaseInboxMessagesQuery, List<InBoxMessage>>(
                new GetBookcaseInboxMessagesQuery());

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