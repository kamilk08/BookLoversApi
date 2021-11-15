using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Statistics
{
    internal class CreateStatisticsGathererInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid StatisticsGathererGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid ProfileGuid { get; private set; }

        private CreateStatisticsGathererInternalCommand()
        {
        }

        public CreateStatisticsGathererInternalCommand(
            Guid statisticsGathererGuid,
            Guid readerGuid,
            Guid profileGuid)
        {
            Guid = Guid.NewGuid();
            StatisticsGathererGuid = statisticsGathererGuid;
            ReaderGuid = readerGuid;
            ProfileGuid = profileGuid;
        }
    }
}