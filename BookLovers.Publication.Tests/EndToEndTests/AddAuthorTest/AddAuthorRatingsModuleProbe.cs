using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Queries.Authors;
using BookLovers.Ratings.Infrastructure.Root;

namespace BookLovers.Publication.Tests.EndToEndTests.AddAuthorTest
{
    internal class AddAuthorRatingsModuleProbe : IProbe
    {
        private readonly IModule<RatingsModule> _module;
        private readonly Guid _authorGuid;

        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;
        private AuthorDto _dto;

        public AddAuthorRatingsModuleProbe(IModule<RatingsModule> module, Guid authorGuid)
        {
            _module = module;
            _authorGuid = authorGuid;
        }

        public bool IsSatisfied()
        {
            return _dto != null && _dto.AuthorGuid == _authorGuid && AreOutboxMessagesProcessed() &&
                   AreInboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var dtoQuery =
                await _module.ExecuteQueryAsync<AuthorByGuidQuery, AuthorDto>(new AuthorByGuidQuery(_authorGuid));
            var outboxQuery = await _module.ExecuteQueryAsync<GetRatingsOutboxMessagesQuery, List<OutboxMessage>>(
                new GetRatingsOutboxMessagesQuery());
            var inboxQuery = await _module.ExecuteQueryAsync<GetRatingsInboxMessagesQuery, List<InBoxMessage>>(
                new GetRatingsInboxMessagesQuery());

            _dto = dtoQuery.Value;
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