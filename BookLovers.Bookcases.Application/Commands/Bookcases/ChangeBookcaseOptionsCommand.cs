using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.WriteModels;

namespace BookLovers.Bookcases.Application.Commands.Bookcases
{
    public class ChangeBookcaseOptionsCommand : ICommand
    {
        public ChangeBookcaseOptionsWriteModel WriteModel { get; }

        public ChangeBookcaseOptionsCommand(ChangeBookcaseOptionsWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}