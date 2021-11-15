using System;
using System.Collections.Generic;

namespace BookLovers.Publication.Application.WriteModels.Books
{
    public class BookWriteModel
    {
        public Guid BookGuid { get; set; }

        public BookBasicsWriteModel Basics { get; set; }

        public BookDescriptionWriteModel Description { get; set; }

        public BookDetailsWriteModel Details { get; set; }

        public BookCoverWriteModel Cover { get; set; }

        public Guid? SeriesGuid { get; set; }

        public List<Guid> Cycles { get; set; }

        public List<string> HashTags { get; set; }

        public int? PositionInSeries { get; set; }

        public List<Guid> Authors { get; set; }

        public Guid AddedBy { get; set; }
    }
}