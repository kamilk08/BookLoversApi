using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.WriteModels.Quotes;

namespace BookLovers.Publication.Application.Commands.Quotes
{
    public class AddBookQuoteCommand : ICommand
    {
        public AddQuoteWriteModel WriteModel { get; }

        public AddBookQuoteCommand(AddQuoteWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}