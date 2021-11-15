using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Users
{
    public class SignUpCommand : ICommand
    {
        public SignUpWriteModel WriteModel { get; }

        public SignUpCommand(SignUpWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}