using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Librarians.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Root;

namespace BookLovers.Bookcases.Tests.EndToEndTests.RemoveFromShelfTests
{
    public class RemoveFromReadShelfLibrariansModuleProbe : IProbe
    {
        private readonly IModule<LibrarianModule> _module;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        public RemoveFromReadShelfLibrariansModuleProbe(IModule<LibrarianModule> module)
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
                await _module.ExecuteQueryAsync<GetLibrariansOutboxMessagesQuery, List<OutboxMessage>>(
                    new GetLibrariansOutboxMessagesQuery());

            var inboxMessagesQuery =
                await _module.ExecuteQueryAsync<GetLibrariansInboxMessagesQuery, List<InBoxMessage>>(
                    new GetLibrariansInboxMessagesQuery());

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