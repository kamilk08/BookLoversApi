using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.WriteModels;

namespace BookLovers.Bookcases.Application.Commands.Bookcases
{
    public class RemoveFromBookcaseCommand : ICommand
    {
        public RemoveFromBookcaseWriteModel WriteModel { get; }

        public RemoveFromBookcaseCommand(RemoveFromBookcaseWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}