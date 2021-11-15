using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Queries;
using BookLovers.Librarians.Infrastructure.Root;

namespace BookLovers.Auth.Tests.EndToEndTests.RegisterUser
{
    internal class RegisterUserLibrarianModuleProbe : IProbe
    {
        private readonly IModule<LibrarianModule> _module;
        private readonly int _readerId;

        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;
        private TicketOwnerDto _dto;

        public RegisterUserLibrarianModuleProbe(IModule<LibrarianModule> module, int readerId)
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
            var dtoQueryResult = await _module.ExecuteQueryAsync<TicketOwnerByIdQuery, TicketOwnerDto>(
                new TicketOwnerByIdQuery(_readerId));

            var outboxMessages = await _module.ExecuteQueryAsync<GetLibrariansOutboxMessagesQuery, List<OutboxMessage>>(
                new GetLibrariansOutboxMessagesQuery());

            var inboxMessages = await _module.ExecuteQueryAsync<GetLibrariansInboxMessagesQuery, List<InBoxMessage>>(
                new GetLibrariansInboxMessagesQuery());

            _dto = dtoQueryResult.Value;
            _outboxMessages = outboxMessages.Value;
            _inBoxMessages = inboxMessages.Value;
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