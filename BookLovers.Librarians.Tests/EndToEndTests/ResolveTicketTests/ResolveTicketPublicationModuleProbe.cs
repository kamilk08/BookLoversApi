using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Books;
using BookLovers.Publication.Infrastructure.Root;

namespace BookLovers.Librarians.Tests.EndToEndTests.ResolveTicketTests
{
    internal class ResolveTicketPublicationModuleProbe : IProbe
    {
        private readonly IModule<PublicationModule> _module;
        private readonly Guid _bookGuid;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;
        private BookDto _dto;

        public ResolveTicketPublicationModuleProbe(IModule<PublicationModule> module, Guid bookGuid)
        {
            _module = module;
            _bookGuid = bookGuid;
        }

        public bool IsSatisfied()
        {
            return AreInboxMessagesProcessed() && AreOutboxMessagesProcessed() && _dto != null;
        }

        public async Task SampleAsync()
        {
            var outboxQueryResult = await _module.ExecuteQueryAsync<GetBooksOutboxMessagesQuery, List<OutboxMessage>>(
                new GetBooksOutboxMessagesQuery());

            var inboxQueryResult = await _module.ExecuteQueryAsync<GetBooksInboxMessagesQuery, List<InBoxMessage>>(
                new GetBooksInboxMessagesQuery());

            var dtoQueryResult =
                await _module.ExecuteQueryAsync<BookByGuidQuery, BookDto>(new BookByGuidQuery(_bookGuid));

            _outboxMessages = outboxQueryResult.Value;
            _inBoxMessages = inboxQueryResult.Value;
            _dto = dtoQueryResult.Value;
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