using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.PasswordResets
{
    public class GenerateResetTokenPasswordCommand : ICommand
    {
        public GenerateResetPasswordTokenWriteModel WriteModel { get; }

        public string Token { get; internal set; }

        public GenerateResetTokenPasswordCommand(GenerateResetPasswordTokenWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}