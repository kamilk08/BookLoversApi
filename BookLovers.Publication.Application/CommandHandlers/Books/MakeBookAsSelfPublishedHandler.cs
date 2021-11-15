using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Publishers;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class MakeBookAsSelfPublishedHandler :
        ICommandHandler<MakeBookAsSelfPublishedInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MakeBookAsSelfPublishedHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(
            MakeBookAsSelfPublishedInternalCommand command)
        {
            var publisher = await this._unitOfWork.GetAsync<Publisher>(command.PublisherGuid);

            publisher.AddBook(new PublisherBook(command.BookGuid));

            await this._unitOfWork.CommitAsync(publisher);
        }
    }
}