using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Statistics
{
    internal class UpdateStatisticsGathererInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int StatisticsType { get; private set; }

        public int StatisticsStep { get; private set; }

        private UpdateStatisticsGathererInternalCommand()
        {
        }

        public UpdateStatisticsGathererInternalCommand(
            Guid readerGuid,
            int statisticsType,
            int statisticsStep)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            StatisticsType = statisticsType;
            StatisticsStep = statisticsStep;
        }
    }
}