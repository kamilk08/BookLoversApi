using System;
using BookLovers.Base.Infrastructure.Commands;
using Newtonsoft.Json;

namespace BookLovers.Auth.Application.Commands.Users
{
    internal class DegradeToReaderInternalCommand : IInternalCommand, ICommand
    {
        [JsonProperty] public Guid Guid { get; private set; }

        [JsonProperty] public Guid UserGuid { get; private set; }

        private DegradeToReaderInternalCommand()
        {
        }

        public DegradeToReaderInternalCommand(Guid userGuid)
        {
            this.Guid = Guid.NewGuid();
            this.UserGuid = userGuid;
        }
    }
}