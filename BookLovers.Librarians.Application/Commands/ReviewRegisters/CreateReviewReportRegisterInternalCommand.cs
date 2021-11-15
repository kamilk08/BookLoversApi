using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.ReviewRegisters
{
    internal class CreateReviewReportRegisterInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        private CreateReviewReportRegisterInternalCommand()
        {
        }

        public CreateReviewReportRegisterInternalCommand(Guid reviewGuid)
        {
            this.Guid = Guid.NewGuid();
            this.ReviewGuid = reviewGuid;
        }
    }
}