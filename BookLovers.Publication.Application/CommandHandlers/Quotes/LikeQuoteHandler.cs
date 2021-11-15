using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.Commands.Quotes;
using BookLovers.Publication.Domain.Quotes;
using BookLovers.Shared.Likes;

namespace BookLovers.Publication.Application.CommandHandlers.Quotes
{
    internal class LikeQuoteHandler : ICommandHandler<LikeQuoteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public LikeQuoteHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(LikeQuoteCommand command)
        {
            var quote = await this._unitOfWork.GetAsync<Quote>(command.QuoteGuid);

            quote.AddLike(Like.NewLike(this._contextAccessor.UserGuid));

            await this._unitOfWork.CommitAsync(quote);
        }
    }
}