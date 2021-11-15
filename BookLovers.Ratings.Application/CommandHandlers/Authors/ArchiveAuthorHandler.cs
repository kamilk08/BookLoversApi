using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Authors;
using BookLovers.Ratings.Domain.Authors;

namespace BookLovers.Ratings.Application.CommandHandlers.Authors
{
    internal class ArchiveAuthorHandler : ICommandHandler<ArchiveAuthorInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorRepository _authorRepository;

        public ArchiveAuthorHandler(IUnitOfWork unitOfWork, IAuthorRepository authorRepository)
        {
            this._unitOfWork = unitOfWork;
            this._authorRepository = authorRepository;
        }

        public async Task HandleAsync(ArchiveAuthorInternalCommand command)
        {
            var author = await this._authorRepository.GetByAuthorGuidAsync(command.AuthorGuid);

            author.ArchiveAggregate();

            await this._unitOfWork.CommitAsync(author);
        }
    }
}