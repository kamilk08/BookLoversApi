using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.Authors
{
    internal class RecalculateAuthorAverageInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid BookGuid { get; private set; }

        private RecalculateAuthorAverageInternalCommand()
        {
        }

        public RecalculateAuthorAverageInternalCommand(Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.BookGuid = bookGuid;
        }
    }
}