using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Quotes;
using BookLovers.Publication.Domain.Quotes;
using BookLovers.Publication.Domain.Quotes.Services;
using BookLovers.Publication.Integration.ApplicationEvents.Books;

namespace BookLovers.Publication.Application.CommandHandlers.Quotes
{
    internal class AddBookQuoteHandler : ICommandHandler<AddBookQuoteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IReadContextAccessor _readContextAccessor;
        private readonly IInMemoryEventBus _inMemoryEventBus;
        private readonly QuoteFactory _factory;

        public AddBookQuoteHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IReadContextAccessor readContextAccessor,
            IInMemoryEventBus inMemoryEventBus,
            QuoteFactory factory)
        {
            this._unitOfWork = unitOfWork;
            this._httpContextAccessor = httpContextAccessor;
            this._readContextAccessor = readContextAccessor;
            this._inMemoryEventBus = inMemoryEventBus;
            this._factory = factory;
        }

        public async Task HandleAsync(AddBookQuoteCommand command)
        {
            var quoteContent = new QuoteContent(command.WriteModel.Quote, command.WriteModel.QuotedGuid);
            var quoteDetails = new QuoteDetails(this._httpContextAccessor.UserGuid, command.WriteModel.AddedAt,
                QuoteType.BookQuote);

            var quote = await this._factory.CreateQuote(command.WriteModel.QuoteGuid, quoteContent, quoteDetails);

            await this._unitOfWork.CommitAsync(quote);

            command.WriteModel.QuoteId = this._readContextAccessor.GetReadModelId(quote.Guid);

            await this._inMemoryEventBus.Publish(
                new BookQuoteAddedIntegrationEvent(quote.Guid, this._httpContextAccessor.UserGuid));
        }
    }
}