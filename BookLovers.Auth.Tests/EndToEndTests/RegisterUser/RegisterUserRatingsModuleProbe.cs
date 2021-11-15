using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Queries.Ratings;
using BookLovers.Ratings.Infrastructure.Root;

namespace BookLovers.Auth.Tests.EndToEndTests.RegisterUser
{
    internal class RegisterUserRatingsModuleProbe : IProbe
    {
        private readonly IModule<RatingsModule> _module;
        private readonly int _readerId;
        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;
        private ReaderDto _dto;

        public RegisterUserRatingsModuleProbe(IModule<RatingsModule> module, int readerId)
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
            var dtoQueryResult =
                await _module.ExecuteQueryAsync<ReaderByIdQuery, ReaderDto>(new ReaderByIdQuery(_readerId));
            var outboxMessagesQuery =
                await _module.ExecuteQueryAsync<GetRatingsOutboxMessagesQuery, List<OutboxMessage>>(
                    new GetRatingsOutboxMessagesQuery());
            var inboxMessagesQuery = await _module.ExecuteQueryAsync<GetRatingsInboxMessagesQuery, List<InBoxMessage>>(
                new GetRatingsInboxMessagesQuery());

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