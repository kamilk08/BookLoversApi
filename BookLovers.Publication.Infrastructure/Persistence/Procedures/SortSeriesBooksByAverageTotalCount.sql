CREATE PROCEDURE [dbo].[SortSeriesBooksByAverageTotalCount] (@SERIES_ID INT,@TITLE VARCHAR(2000))
AS
IF @TITLE IS NOT NULL
SELECT COUNT(*) FROM PublicationsContext.dbo.Books as Books
INNER JOIN RatingsContext.dbo.Books as RatingsBooks ON RatingsBooks.BookId=Books.Id
WHERE Books.SeriesId = @SERIES_ID AND Books.Title LIKE @TITLE + '%'
GROUP BY Books.SeriesId
ELSE 
SELECT COUNT(*) FROM PublicationsContext.dbo.Books as Books
INNER JOIN RatingsContext.dbo.Books as RatingsBooks ON RatingsBooks.BookId=Books.Id
WHERE Books.SeriesId = @SERIES_ID
GROUP BY Books.SeriesId