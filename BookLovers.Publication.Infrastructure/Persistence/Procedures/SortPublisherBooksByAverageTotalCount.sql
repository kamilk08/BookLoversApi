CREATE PROCEDURE [dbo].[SortPublisherBooksByAverageTotalCount](@PUBLISHER_ID INT,@TITLE VARCHAR(2000))
AS
IF @TITLE IS NOT NULL
SELECT COUNT(*) FROM PublicationsContext.dbo.Books as Books
INNER JOIN RatingsContext.dbo.Books as RatingsBooks ON RatingsBooks.BookId=Books.Id
WHERE Books.PublisherId = @PUBLISHER_ID AND Books.Title LIKE @TITLE + '%'
GROUP BY Books.PublisherId
ELSE 
SELECT COUNT(*) FROM PublicationsContext.dbo.Books as Books
INNER JOIN RatingsContext.dbo.Books as RatingsBooks ON RatingsBooks.BookId=Books.Id
WHERE Books.PublisherId = @PUBLISHER_ID
GROUP BY Books.PublisherId