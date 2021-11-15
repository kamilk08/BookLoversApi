using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.Services;

namespace BookLovers.Readers.Application.CommandHandlers.Readers
{
    internal class CreateReaderHandler : ICommandHandler<CreateReaderInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ReaderFactory _readerFactory;

        public CreateReaderHandler(IUnitOfWork unitOfWork, ReaderFactory readerFactory)
        {
            _unitOfWork = unitOfWork;
            _readerFactory = readerFactory;
        }

        public Task HandleAsync(CreateReaderInternalCommand command)
        {
            var reader = _readerFactory.Create(
                command.ReaderGuid,
                new ReaderIdentification(command.ReaderId, command.UserName, command.Email),
                new ReaderSocials(command.ProfileGuid, Guid.NewGuid(), Guid.NewGuid()));

            return _unitOfWork.CommitAsync(reader);
        }
    }
}