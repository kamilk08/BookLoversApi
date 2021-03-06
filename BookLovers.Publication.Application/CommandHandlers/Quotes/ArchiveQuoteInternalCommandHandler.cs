using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Quotes;
using BookLovers.Publication.Domain.Quotes;

namespace BookLovers.Publication.Application.CommandHandlers.Quotes
{
    internal class ArchiveQuoteInternalCommandHandler : ICommandHandler<ArchiveQuoteInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<Quote> _quoteManager;

        public ArchiveQuoteInternalCommandHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<Quote> quoteManager)
        {
            this._unitOfWork = unitOfWork;
            this._quoteManager = quoteManager;
        }

        public async Task HandleAsync(ArchiveQuoteInternalCommand command)
        {
            var quote = await this._unitOfWork.GetAsync<Quote>(command.QuoteGuid);

            this._quoteManager.Archive(quote);

            await this._unitOfWork.CommitAsync(quote);
        }
    }
}