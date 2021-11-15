using System;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.WriteModels;

namespace BookLovers.Librarians.Application.Commands
{
    internal class AssignSuperAdminToLibrarianInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public CreateLibrarianWriteModel WriteModel { get; private set; }

        private AssignSuperAdminToLibrarianInternalCommand()
        {
        }

        public AssignSuperAdminToLibrarianInternalCommand(CreateLibrarianWriteModel writeModel)
        {
            this.Guid = Guid.NewGuid();
            this.WriteModel = writeModel;
        }
    }
}