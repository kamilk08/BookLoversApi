using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    internal class EditReviewActivityInternalCommand : ICommand, IInternalCommand
    {
        public Guid ReviewGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public DateTime EditDate { get; private set; }

        public Guid Guid { get; private set; }

        private EditReviewActivityInternalCommand()
        {
        }

        public EditReviewActivityInternalCommand(Guid reviewGuid, Guid readerGuid, DateTime editDate)
        {
            ReviewGuid = reviewGuid;
            ReaderGuid = readerGuid;
            EditDate = editDate;
            Guid = Guid.NewGuid();
        }
    }
}