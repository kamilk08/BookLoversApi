using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;
using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Publication.Application.CommandHandlers.Authors
{
    internal class CreateUnknownAuthorHandler : ICommandHandler<CreateUnknownAuthorCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;
        private readonly AuthorFactory _authorFactory;
        private readonly IReadContextAccessor _readContextAccessor;
        private readonly IBookReaderAccessor _bookReaderAccessor;

        public CreateUnknownAuthorHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus eventBus,
            AuthorFactory authorFactory,
            IReadContextAccessor readContextAccessor,
            IBookReaderAccessor bookReaderAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._eventBus = eventBus;
            this._authorFactory = authorFactory;
            this._readContextAccessor = readContextAccessor;
            this._bookReaderAccessor = bookReaderAccessor;
        }

        public async Task HandleAsync(CreateUnknownAuthorCommand command)
        {
            var bookReader = await this._unitOfWork.GetAsync<BookReader>(
                await this._bookReaderAccessor.GetAggregateGuidAsync(command.AddedByGuid));

            var data = AuthorData.Initialize()
                .WithGuid(command.AuthorGuid)
                .WithBasics(new FullName("Unknown", "Author"), Sex.Hidden)
                .WithDetails(new LifeLength(null, null), "Unknown")
                .WithDescription("UnknownAuthor", null, null)
                .WithGenres(new List<int>()).WithBooks(new List<Guid>())
                .AddedBy(bookReader);

            var author = this._authorFactory.CreateAuthor(data);

            await this._unitOfWork.CommitAsync(author);

            await this._eventBus.Publish(new NewAuthorAddedIntegrationEvent(
                author.Guid,
                this._readContextAccessor.GetReadModelId(author.Guid),
                command.AddedByGuid));
        }
    }
}