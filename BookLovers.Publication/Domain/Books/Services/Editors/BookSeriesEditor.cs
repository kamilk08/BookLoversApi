using System;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Domain.SeriesCycle;

namespace BookLovers.Publication.Domain.Books.Services.Editors
{
    public class BookSeriesEditor : IBookEditor
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookSeriesEditor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task EditBook(Book book, BookData bookData)
        {
            var series = await _unitOfWork.GetAsync<Series>(bookData.SeriesData.SeriesGuid);

            if (bookData.SeriesData.SeriesGuid != Guid.Empty)
            {
                if (series == null)
                    throw new BusinessRuleNotMetException(
                        $"Series with GUID {bookData.SeriesData.SeriesGuid} does not exist.");
            }

            var bookSeries = new BookSeries(bookData.SeriesData.SeriesGuid, bookData.SeriesData.PositionInSeries);
            if (bookSeries == book.Series)
                return;

            book.ChangeSeries(bookSeries);
        }
    }
}