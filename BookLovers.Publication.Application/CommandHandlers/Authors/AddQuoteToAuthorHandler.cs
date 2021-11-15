using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Domain.Authors;

namespace BookLovers.Publication.Application.CommandHandlers.Authors
{
    internal class AddQuoteToAuthorHandler : ICommandHandler<AddQuoteToAuthorInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddQuoteToAuthorHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AddQuoteToAuthorInternalCommand command)
        {
            var author = await this._unitOfWork.GetAsync<Author>(command.AuthorGuid);

            author.AddQuote(new AuthorQuote(command.QuoteGuid));

            await this._unitOfWork.CommitAsync(author);
        }
    }
}