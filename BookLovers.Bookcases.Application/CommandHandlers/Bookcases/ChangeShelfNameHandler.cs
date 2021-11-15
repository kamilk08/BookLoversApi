using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands.Shelves;
using BookLovers.Bookcases.Domain;

namespace BookLovers.Bookcases.Application.CommandHandlers.Bookcases
{
    internal class ChangeShelfNameHandler : ICommandHandler<ChangeShelfNameCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeShelfNameHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ChangeShelfNameCommand command)
        {
            var bookcase = await _unitOfWork.GetAsync<Bookcase>(command.WriteModel.BookcaseGuid);

            bookcase.ChangeShelfName(command.WriteModel.ShelfGuid, command.WriteModel.ShelfName);

            await _unitOfWork.CommitAsync(bookcase);
        }
    }
}