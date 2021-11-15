using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Readers.Infrastructure.Dtos;
using BookLovers.Readers.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Root;

namespace BookLovers.Publication.Tests.EndToEndTests.AddAuthorTest
{
    internal class AddAuthorReadersModuleProbe : IProbe
    {
        private readonly Guid _authorGuid;
        private readonly IModule<ReadersModule> _module;

        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;
        private AuthorDto _authorDto;

        public AddAuthorReadersModuleProbe(IModule<ReadersModule> module, Guid authorGuid)
        {
            _module = module;
            _authorGuid = authorGuid;
        }

        public bool IsSatisfied()
        {
            return _authorDto != null && _authorDto.AuthorGuid == _authorGuid &&
                   AreInboxMessagesProcessed() && AreOutboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var dtoQuery =
                await _module.ExecuteQueryAsync<AuthorByGuidQuery, AuthorDto>(new AuthorByGuidQuery(_authorGuid));

            var outboQuery = await _module.ExecuteQueryAsync<GetReadersOutboxMessagesQuery, List<OutboxMessage>>(
                new GetReadersOutboxMessagesQuery());

            var inboxQuery = await _module.ExecuteQueryAsync<GetReadersInboxMessagesQuery, List<InBoxMessage>>(
                new GetReadersInboxMessagesQuery());

            _authorDto = dtoQuery.Value;
            _outboxMessages = outboQuery.Value;
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