CREATE PROCEDURE [dbo].[SortSeriesBooksByAverage] (@SERIES_ID INT,@TITLE VARCHAR(2000),@ORDER_BY BIT,@ROW_COUNT INT,@SKIP INT)
AS
IF @ORDER_BY =1 
IF @TITLE IS NOT NULL
SELECT * FROM PublicationsContext.dbo.Books as Books
INNER JOIN RatingsContext.dbo.Books as RatingsBooks ON RatingsBooks.BookId=Books.Id
WHERE Books.SeriesId = @SERIES_ID AND Books.Title LIKE @TITLE + '%'
ORDER BY RatingsBooks.Average DESC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY
ELSE 
SELECT * FROM PublicationsContext.dbo.Books as Books
INNER JOIN RatingsContext.dbo.Books as RatingsBooks ON RatingsBooks.BookId=Books.Id
WHERE Books.SeriesId = @SERIES_ID
ORDER BY RatingsBooks.Average DESC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY
ELSE
IF(@TITLE IS NOT NULL)
SELECT * FROM PublicationsContext.dbo.Books as Books
INNER JOIN RatingsContext.dbo.Books as RatingsBooks ON RatingsBooks.BookId=Books.Id
WHERE Books.SeriesId = @SERIES_ID AND Books.Title LIKE @TITLE + '%'
ORDER BY RatingsBooks.Average ASC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY
ELSE
SELECT * FROM PublicationsContext.dbo.Books as Books
INNER JOIN RatingsContext.dbo.Books as RatingsBooks ON RatingsBooks.BookId=Books.Id
WHERE Books.SeriesId = @SERIES_ID
ORDER BY RatingsBooks.Average ASC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY