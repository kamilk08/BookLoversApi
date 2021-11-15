using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Librarians.Application.Commands.ReviewRegisters
{
    internal class ArchiveReportRegistrationInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        private ArchiveReportRegistrationInternalCommand()
        {
        }

        public ArchiveReportRegistrationInternalCommand(Guid reviewGuid)
        {
            this.Guid = Guid.NewGuid();
            this.ReviewGuid = reviewGuid;
        }
    }
}