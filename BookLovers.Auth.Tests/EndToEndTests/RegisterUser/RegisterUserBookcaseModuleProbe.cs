using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using BookLovers.Bookcases.Infrastructure.Root;

namespace BookLovers.Auth.Tests.EndToEndTests.RegisterUser
{
    internal class RegisterUserBookcaseModuleProbe : IProbe
    {
        private readonly IModule<BookcaseModule> _module;
        private readonly int _readerId;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;
        private BookcaseDto _dto;

        public RegisterUserBookcaseModuleProbe(IModule<BookcaseModule> module, int readerId)
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
            var dtoQueryResult = await _module.ExecuteQueryAsync<BookcaseByReaderIdQuery, BookcaseDto>(
                new BookcaseByReaderIdQuery(_readerId));

            var outboxMessagesQuery =
                await _module.ExecuteQueryAsync<GetBookcaseOutboxMessagesQuery, List<OutboxMessage>>(
                    new GetBookcaseOutboxMessagesQuery());

            var inboxMessagesQuery = await _module.ExecuteQueryAsync<GetBookcaseInboxMessagesQuery, List<InBoxMessage>>(
                new GetBookcaseInboxMessagesQuery());

            _dto = dtoQueryResult.Value;

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