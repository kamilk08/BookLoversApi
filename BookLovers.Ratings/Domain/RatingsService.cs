using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.Books.BusinessRules;
using BookLovers.Ratings.Domain.RatingGivers;

namespace BookLovers.Ratings.Domain
{
    public class RatingsService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRatingGiverRepository _ratingGiverRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBookInBookcaseChecker _checker;

        private readonly List<Func<Book, RatingGiver, IBusinessRule>> _addRatingRules =
            new List<Func<Book, RatingGiver, IBusinessRule>>();

        private readonly List<Func<Book, RatingGiver, IBusinessRule>> _addOrRemoveRatingRules =
            new List<Func<Book, RatingGiver, IBusinessRule>>();

        public RatingsService(
            IBookRepository bookRepository,
            IRatingGiverRepository ratingGiverRepository,
            IHttpContextAccessor httpContextAccessor,
            IBookInBookcaseChecker checker)
        {
            this._bookRepository = bookRepository;
            this._ratingGiverRepository = ratingGiverRepository;
            this._httpContextAccessor = httpContextAccessor;
            this._checker = checker;

            this._addRatingRules.Add((book, ratingGiver) => new AggregateMustExist(book?.Guid ?? Guid.Empty));
            this._addRatingRules.Add((book, ratingGiver) => new AggregateMustExist(ratingGiver.Guid));
            this._addOrRemoveRatingRules.Add((book, ratingGiver) => new AggregateMustExist(book?.Guid ?? Guid.Empty));
            this._addOrRemoveRatingRules.Add((book, ratingGiver) => new AggregateMustExist(ratingGiver.Guid));
            this._addOrRemoveRatingRules.Add((book, ratingGiver) =>
                new BookMustHaveRatingFromSelectedReader(book, ratingGiver));
        }

        public async Task<Book> AddRatingToBookAsync(Guid bookGuid, int stars)
        {
            var book = await this._bookRepository.GetByBookGuidAsync(bookGuid);
            var ratingGiver =
                await this._ratingGiverRepository.GetRatingGiverByReaderGuid(this._httpContextAccessor.UserGuid);

            var isInBookcase = await this._checker.IsBookInBookcaseAsync(this._httpContextAccessor.UserGuid, bookGuid);

            if (!isInBookcase)
                throw new BusinessRuleNotMetException("Reader does not have certain book in his bookcase.");

            foreach (var rule in this._addRatingRules)
            {
                if (!rule(book, ratingGiver).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(book, ratingGiver).BrokenRuleMessage);
            }

            book.AddRating(new Rating(book.Identification.BookId, ratingGiver.ReaderId, stars));

            return book;
        }

        public async Task<Book> ChangeBookRatingAsync(Guid bookGuid, int stars)
        {
            var book = await this._bookRepository.GetByBookGuidAsync(bookGuid);
            var ratingGiver =
                await this._ratingGiverRepository.GetRatingGiverByReaderGuid(this._httpContextAccessor.UserGuid);
            var isInBookcase = await this._checker.IsBookInBookcaseAsync(this._httpContextAccessor.UserGuid, bookGuid);

            if (!isInBookcase)
                throw new BusinessRuleNotMetException("Reader does not have certain book in his bookcase.");

            foreach (var removeRatingRule in this._addOrRemoveRatingRules)
            {
                if (!removeRatingRule(book, ratingGiver).IsFulfilled())
                    throw new BusinessRuleNotMetException(removeRatingRule(book, ratingGiver).BrokenRuleMessage);
            }

            book.ChangeRating(ratingGiver, new Rating(book.Identification.BookId, ratingGiver.ReaderId, stars));

            return book;
        }

        public async Task<Book> RemoveRatingFromBookAsync(Guid bookGuid)
        {
            var book = await this._bookRepository.GetByBookGuidAsync(bookGuid);
            var ratingGiver =
                await this._ratingGiverRepository.GetRatingGiverByReaderGuid(this._httpContextAccessor.UserGuid);

            foreach (var removeRatingRule in this._addOrRemoveRatingRules)
            {
                if (!removeRatingRule(book, ratingGiver).IsFulfilled())
                    throw new BusinessRuleNotMetException(removeRatingRule(book, ratingGiver).BrokenRuleMessage);
            }

            book.RemoveRating(book.GetReaderRating(ratingGiver.ReaderId));

            return book;
        }
    }
}