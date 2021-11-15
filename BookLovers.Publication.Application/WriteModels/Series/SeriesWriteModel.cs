using System;

namespace BookLovers.Publication.Application.WriteModels.Series
{
    public class SeriesWriteModel
    {
        public int SeriesId { get; set; }

        public Guid SeriesGuid { get; set; }

        public string SeriesName { get; set; }
    }
}