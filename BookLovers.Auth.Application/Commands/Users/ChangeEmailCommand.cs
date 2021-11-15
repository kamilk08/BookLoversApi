using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Users
{
    public class ChangeEmailCommand : ICommand
    {
        public ChangeEmailWriteModel WriteModel { get; }

        public ChangeEmailCommand(ChangeEmailWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}