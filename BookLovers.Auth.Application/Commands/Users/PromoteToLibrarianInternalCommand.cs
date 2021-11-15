using System;
using BookLovers.Base.Infrastructure.Commands;
using Newtonsoft.Json;

namespace BookLovers.Auth.Application.Commands.Users
{
    internal class PromoteToLibrarianInternalCommand : IInternalCommand, ICommand
    {
        [JsonProperty] public Guid Guid { get; private set; }

        [JsonProperty] public Guid ReaderGuid { get; private set; }

        private PromoteToLibrarianInternalCommand()
        {
        }

        public PromoteToLibrarianInternalCommand(Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.ReaderGuid = readerGuid;
        }
    }
}