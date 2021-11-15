using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Timelines
{
    internal class AddBookToBookcaseActivityInternalCommand : ICommand
    {
        public Guid ReaderGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public DateTime Date { get; private set; }

        private AddBookToBookcaseActivityInternalCommand()
        {
        }

        public AddBookToBookcaseActivityInternalCommand(Guid readerGuid, Guid bookGuid, DateTime date)
        {
            ReaderGuid = readerGuid;
            BookGuid = bookGuid;
            Date = date;
        }
    }
}