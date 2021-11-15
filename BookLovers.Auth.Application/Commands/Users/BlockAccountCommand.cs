using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Users
{
    public class BlockAccountCommand : ICommand
    {
        public BlockAccountWriteModel WriteModel { get; }

        public BlockAccountCommand(BlockAccountWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}