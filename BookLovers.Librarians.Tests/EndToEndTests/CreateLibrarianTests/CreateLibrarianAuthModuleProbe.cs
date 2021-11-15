using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Auth.Domain.Users;
using BookLovers.Auth.Infrastructure.Dtos.Users;
using BookLovers.Auth.Infrastructure.Queries;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;

namespace BookLovers.Librarians.Tests.EndToEndTests.CreateLibrarianTests
{
    internal class CreateLibrarianAuthModuleProbe : IProbe
    {
        private readonly IModule<AuthModule> _authModule;
        private readonly Guid _readerGuid;
        private List<InBoxMessage> _inBoxMessages;
        private List<OutboxMessage> _outboxMessages;
        private UserDto _dto;

        public CreateLibrarianAuthModuleProbe(IModule<AuthModule> authModule, Guid readerGuid)
        {
            _authModule = authModule;
            _readerGuid = readerGuid;
        }

        public bool IsSatisfied()
        {
            return _dto != null && _dto.Roles.Contains(Role.Librarian.Name) && AreInboxMessagesProcessed() &&
                   AreOutboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var outboxQueryResult =
                await _authModule.ExecuteQueryAsync<GetAuthOutboxMessagesQuery, List<OutboxMessage>>(
                    new GetAuthOutboxMessagesQuery());

            var inboxQueryResult =
                await _authModule.ExecuteQueryAsync<GetAuthInboxMessagesQuery, List<InBoxMessage>>(
                    new GetAuthInboxMessagesQuery());

            var dtoQueryResult = await _authModule.ExecuteQueryAsync<GetUserByGuidQuery, UserDto>(
                new GetUserByGuidQuery(_readerGuid));

            _dto = dtoQueryResult.Value;
            _inBoxMessages = inboxQueryResult.Value;
            _outboxMessages = outboxQueryResult.Value;
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