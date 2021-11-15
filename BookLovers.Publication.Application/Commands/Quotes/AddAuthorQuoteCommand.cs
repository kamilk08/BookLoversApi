using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.WriteModels.Quotes;

namespace BookLovers.Publication.Application.Commands.Quotes
{
    public class AddAuthorQuoteCommand : ICommand
    {
        public AddQuoteWriteModel WriteModel { get; }

        public AddAuthorQuoteCommand(AddQuoteWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}