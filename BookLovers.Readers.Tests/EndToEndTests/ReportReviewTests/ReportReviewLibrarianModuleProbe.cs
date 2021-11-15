using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Librarians.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Root;

namespace BookLovers.Readers.Tests.EndToEndTests.ReportReviewTests
{
    internal class ReportReviewLibrarianModuleProbe : IProbe
    {
        private readonly IModule<LibrarianModule> _module;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        public ReportReviewLibrarianModuleProbe(IModule<LibrarianModule> module)
        {
            _module = module;
        }

        public bool IsSatisfied()
        {
            return _outboxMessages != null && _outboxMessages.Count == 0;
        }

        public async Task SampleAsync()
        {
            var outboxQuery = await _module.ExecuteQueryAsync<GetLibrariansOutboxMessagesQuery, List<OutboxMessage>>(
                new GetLibrariansOutboxMessagesQuery());

            var inboxQuery = await _module.ExecuteQueryAsync<GetLibrariansInboxMessagesQuery, List<InBoxMessage>>(
                new GetLibrariansInboxMessagesQuery());

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