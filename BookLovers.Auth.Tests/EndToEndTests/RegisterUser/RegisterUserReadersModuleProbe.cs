using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Queries.Readers;
using BookLovers.Readers.Infrastructure.Root;

namespace BookLovers.Auth.Tests.EndToEndTests.RegisterUser
{
    internal class RegisterUserReadersModuleProbe : IProbe
    {
        private readonly IModule<ReadersModule> _module;
        private readonly int _readerId;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;
        private ReaderDto _dto;

        public RegisterUserReadersModuleProbe(IModule<ReadersModule> module, int readerId)
        {
            _module = module;
            _readerId = readerId;
        }

        public bool IsSatisfied()
        {
            return _dto != null && _dto.ReaderId == _readerId && AreInboxMessagesProcessed() &&
                   AreOutboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var inboxMessagesQuery = await _module.ExecuteQueryAsync<GetReadersInboxMessagesQuery, List<InBoxMessage>>(
                new GetReadersInboxMessagesQuery());
            var outboxMessagesQuery =
                await _module.ExecuteQueryAsync<GetReadersOutboxMessagesQuery, List<OutboxMessage>>(
                    new GetReadersOutboxMessagesQuery());

            var dtoQueryResult =
                await _module.ExecuteQueryAsync<ReaderByIdQuery, ReaderDto>(new ReaderByIdQuery(_readerId));

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