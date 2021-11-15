using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.Readers
{
    internal class CreateReaderInternalCommand : ICommand, IInternalCommand
    {
        public Guid Guid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid ProfileGuid { get; private set; }

        public int ReaderId { get; private set; }

        public string UserName { get; private set; }

        public string Email { get; private set; }

        private CreateReaderInternalCommand()
        {
        }

        public CreateReaderInternalCommand(
            Guid readerGuid,
            Guid profileGuid,
            int readerId,
            string userName,
            string email)
        {
            Guid = Guid.NewGuid();
            ReaderGuid = readerGuid;
            ProfileGuid = profileGuid;
            ReaderId = readerId;
            UserName = userName;
            Email = email;
        }
    }
}