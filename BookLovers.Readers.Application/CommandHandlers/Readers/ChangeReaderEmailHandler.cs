using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Domain.Readers;

namespace BookLovers.Readers.Application.CommandHandlers.Readers
{
    internal class ChangeReaderEmailHandler : ICommandHandler<ChangeReaderEmailInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeReaderEmailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ChangeReaderEmailInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.Guid);

            reader.ChangeEmail(command.Email);

            await _unitOfWork.CommitAsync(reader);
        }
    }
}