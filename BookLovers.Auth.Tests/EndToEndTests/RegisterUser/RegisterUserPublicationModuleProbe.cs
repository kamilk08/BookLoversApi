using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Publication.Infrastructure.Dtos;
using BookLovers.Publication.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Root;

namespace BookLovers.Auth.Tests.EndToEndTests.RegisterUser
{
    internal class RegisterUserPublicationModuleProbe : IProbe
    {
        private readonly IModule<PublicationModule> _module;
        private readonly int _readerId;

        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;
        private BookReaderDto _dto;

        public RegisterUserPublicationModuleProbe(IModule<PublicationModule> module, int readerId)
        {
            _module = module;
            _readerId = readerId;
        }

        public bool IsSatisfied()
        {
            return _dto != null && _dto.ReaderId == _readerId && AreOutboxMessagesProcessed() &&
                   AreInboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var dtoQuerResult = await _module.ExecuteQueryAsync<BookReaderByIdQuery, BookReaderDto>(
                new BookReaderByIdQuery(_readerId));

            var outboxMessagesQuery = await _module.ExecuteQueryAsync<GetBooksOutboxMessagesQuery, List<OutboxMessage>>(
                new GetBooksOutboxMessagesQuery());

            var inboxMessagesQuery = await _module.ExecuteQueryAsync<GetBooksInboxMessagesQuery, List<InBoxMessage>>(
                new GetBooksInboxMessagesQuery());

            _dto = dtoQuerResult.Value;
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