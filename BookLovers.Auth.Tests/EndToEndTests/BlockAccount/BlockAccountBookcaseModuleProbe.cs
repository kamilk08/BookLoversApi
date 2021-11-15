using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Bookcases.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Root;

namespace BookLovers.Auth.Tests.EndToEndTests.BlockAccount
{
    internal class BlockAccountBookcaseModuleProbe : IProbe
    {
        private readonly IModule<BookcaseModule> _module;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inboxMessages;

        public BlockAccountBookcaseModuleProbe(IModule<BookcaseModule> module)
        {
            _module = module;
        }

        public bool IsSatisfied()
        {
            return AreOutboxMessagesProcessed() && AreInboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var outboxMessages = await _module.ExecuteQueryAsync<GetBookcaseOutboxMessagesQuery, List<OutboxMessage>>(
                new GetBookcaseOutboxMessagesQuery());

            var inboxMessages = await _module.ExecuteQueryAsync<GetBookcaseInboxMessagesQuery, List<InBoxMessage>>(
                new GetBookcaseInboxMessagesQuery());

            _outboxMessages = outboxMessages.Value;
            _inboxMessages = inboxMessages.Value;
        }

        private bool AreOutboxMessagesProcessed()
        {
            return _outboxMessages != null && _outboxMessages.Count == 0;
        }

        private bool AreInboxMessagesProcessed()
        {
            return _inboxMessages != null && _inboxMessages.Count == 0;
        }
    }
}