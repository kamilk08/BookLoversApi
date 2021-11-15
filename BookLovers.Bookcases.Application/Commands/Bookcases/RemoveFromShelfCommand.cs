using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.WriteModels;

namespace BookLovers.Bookcases.Application.Commands.Bookcases
{
    public class RemoveFromShelfCommand : ICommand
    {
        public RemoveFromShelfWriteModel WriteModel { get; }

        public RemoveFromShelfCommand(RemoveFromShelfWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}