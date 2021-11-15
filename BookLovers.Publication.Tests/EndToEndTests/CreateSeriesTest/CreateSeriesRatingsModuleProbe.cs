using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Messages;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Queries.Series;
using BookLovers.Ratings.Infrastructure.Root;

namespace BookLovers.Publication.Tests.EndToEndTests.CreateSeriesTest
{
    internal class CreateSeriesRatingsModuleProbe : IProbe
    {
        private readonly IModule<RatingsModule> _module;
        private readonly Guid _seriesGuid;

        private List<OutboxMessage> _outboxMessages;
        private List<InBoxMessage> _inBoxMessages;

        private SeriesDto _dto;

        public CreateSeriesRatingsModuleProbe(IModule<RatingsModule> module, Guid seriesGuid)
        {
            _module = module;
            _seriesGuid = seriesGuid;
        }

        public bool IsSatisfied()
        {
            return _dto != null && _dto.SeriesGuid == _seriesGuid && AreInboxMessagesProcessed() &&
                   AreOutboxMessagesProcessed();
        }

        public async Task SampleAsync()
        {
            var dtoQuery =
                await _module.ExecuteQueryAsync<SeriesByGuidQuery, SeriesDto>(new SeriesByGuidQuery(_seriesGuid));

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