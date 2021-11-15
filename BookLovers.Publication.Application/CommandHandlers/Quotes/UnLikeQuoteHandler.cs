using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Quotes;
using BookLovers.Publication.Domain.Quotes;

namespace BookLovers.Publication.Application.CommandHandlers.Quotes
{
    internal class UnLikeQuoteHandler : ICommandHandler<UnLikeQuoteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public UnLikeQuoteHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(UnLikeQuoteCommand command)
        {
            var quote = await this._unitOfWork.GetAsync<Quote>(command.QuoteGuid);

            var like = quote.GetLike(this._contextAccessor.UserGuid);

            quote.UnLike(like);

            await this._unitOfWork.CommitAsync(quote);
        }
    }
}