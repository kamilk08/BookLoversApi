using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Publication.Application.Commands.Books
{
    public class ArchiveBookCommand : ICommand
    {
        public Guid BookGuid { get; }

        public ArchiveBookCommand(Guid bookGuid)
        {
            this.BookGuid = bookGuid;
        }
    }
}