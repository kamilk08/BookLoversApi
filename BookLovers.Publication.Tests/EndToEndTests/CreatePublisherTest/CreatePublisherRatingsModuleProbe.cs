using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Queries.Publishers;
using BookLovers.Ratings.Infrastructure.Root;

namespace BookLovers.Publication.Tests.EndToEndTests.CreatePublisherTest
{
    internal class CreatePublisherRatingsModuleProbe : IProbe
    {
        private readonly IModule<RatingsModule> _module;
        private readonly Guid _publisherGuid;

        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        private PublisherDto _dto;

        public CreatePublisherRatingsModuleProbe(IModule<RatingsModule> module, Guid publisherGuid)
        {
            _module = module;
            _publisherGuid = publisherGuid;
        }

        public bool IsSatisfied()
        {
            return _dto != null && _dto.PublisherGuid == _publisherGuid && AreInboxMessagesProcessed() &&
                   AreOutboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var dtoQuery = await _module.ExecuteQueryAsync<PublisherByGuidQuery, PublisherDto>(
                new PublisherByGuidQuery(_publisherGuid));

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