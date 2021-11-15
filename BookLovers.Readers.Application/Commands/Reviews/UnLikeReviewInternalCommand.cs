using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Reviews
{
    internal class UnLikeReviewInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        public Guid UserGuid { get; private set; }

        private UnLikeReviewInternalCommand()
        {
        }

        public UnLikeReviewInternalCommand(Guid reviewGuid, Guid userGuid)
        {
            Guid = Guid.NewGuid();
            UserGuid = userGuid;
            ReviewGuid = reviewGuid;
        }
    }
}