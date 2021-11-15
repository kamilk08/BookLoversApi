using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Quotes;
using BookLovers.Publication.Domain.Quotes;
using BookLovers.Publication.Domain.Quotes.Services;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;

namespace BookLovers.Publication.Application.CommandHandlers.Quotes
{
    internal class AddAuthorQuoteHandler : ICommandHandler<AddAuthorQuoteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IReadContextAccessor _readContextAccessor;
        private readonly IInMemoryEventBus _inMemoryEventBus;
        private readonly QuoteFactory _factory;

        public AddAuthorQuoteHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            IReadContextAccessor readContextAccessor,
            IInMemoryEventBus inMemoryEventBus,
            QuoteFactory factory)
        {
            this._unitOfWork = unitOfWork;
            this._contextAccessor = contextAccessor;
            this._readContextAccessor = readContextAccessor;
            this._inMemoryEventBus = inMemoryEventBus;
            this._factory = factory;
        }

        public async Task HandleAsync(AddAuthorQuoteCommand command)
        {
            var quoteContent = new QuoteContent(command.WriteModel.Quote, command.WriteModel.QuotedGuid);
            var quoteDetails = new QuoteDetails(this._contextAccessor.UserGuid, command.WriteModel.AddedAt,
                QuoteType.AuthorQuote);

            var quote = await this._factory.CreateQuote(command.WriteModel.QuoteGuid, quoteContent, quoteDetails);

            await this._unitOfWork.CommitAsync(quote);

            command.WriteModel.QuoteId = this._readContextAccessor.GetReadModelId(quote.Guid);

            await this._inMemoryEventBus.Publish(
                new AuthorQuoteAddedIntegrationEvent(quote.Guid, this._contextAccessor.UserGuid));
        }
    }
}