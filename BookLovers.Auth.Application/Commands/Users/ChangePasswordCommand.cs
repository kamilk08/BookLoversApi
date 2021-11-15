using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Users
{
    public class ChangePasswordCommand : ICommand
    {
        public ChangePasswordWriteModel WriteModel { get; }

        public ChangePasswordCommand(ChangePasswordWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}