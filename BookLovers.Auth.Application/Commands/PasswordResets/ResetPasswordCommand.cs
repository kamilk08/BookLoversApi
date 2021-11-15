using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.PasswordResets
{
    public class ResetPasswordCommand : ICommand
    {
        public ResetPasswordWriteModel WriteModel { get; }

        public ResetPasswordCommand(ResetPasswordWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}