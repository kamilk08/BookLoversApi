using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Tokens
{
    public class RevokeTokenCommand : ICommand
    {
        public RevokeTokenWriteModel WriteModel { get; }

        public RevokeTokenCommand(RevokeTokenWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}