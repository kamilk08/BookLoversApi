using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Ratings.Application.Commands.Authors
{
    internal class CreateAuthorInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid AuthorGuid { get; private set; }

        public int AuthorId { get; private set; }

        private CreateAuthorInternalCommand()
        {
        }

        public CreateAuthorInternalCommand(Guid authorGuid, int authorId)
        {
            this.Guid = Guid.NewGuid();
            this.AuthorGuid = authorGuid;
            this.AuthorId = authorId;
        }
    }
}