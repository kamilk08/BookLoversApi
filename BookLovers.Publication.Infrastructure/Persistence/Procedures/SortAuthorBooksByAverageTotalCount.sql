
CREATE PROCEDURE [dbo].[SortAuthorBooksByAverageTotalCount](@AUTHOR_ID INT,@TITLE VARCHAR(2000))
AS 
IF(@TITLE IS NOT NULL)
SELECT COUNT(*) FROM Books as Books
INNER JOIN [RatingsContext].dbo.Books as RatingsBooks ON RatingsBooks.BookId = Books.Id
INNER JOIN [AuthorBooks] as AuthorBooks ON AuthorBooks.BookId = Books.Id
WHERE AuthorBooks.AuthorId = @AUTHOR_ID AND Books.Title LIKE @TITLE + '%'
GROUP BY AuthorBooks.AuthorId
ELSE
SELECT COUNT(*) FROM Books as Books
INNER JOIN [RatingsContext].dbo.Books as RatingsBooks ON RatingsBooks.BookId = Books.Id
INNER JOIN [AuthorBooks] as AuthorBooks ON AuthorBooks.BookId = Books.Id
WHERE AuthorBooks.AuthorId = @AUTHOR_ID
GROUP BY AuthorBooks.AuthorId
