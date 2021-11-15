using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Books
{
    internal class RemoveBookFromSeriesInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        private RemoveBookFromSeriesInternalCommand()
        {
        }

        public RemoveBookFromSeriesInternalCommand(Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
        }
    }
}