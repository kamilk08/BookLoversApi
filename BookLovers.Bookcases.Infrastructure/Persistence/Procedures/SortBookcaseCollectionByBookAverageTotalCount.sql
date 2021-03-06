CREATE PROCEDURE [dbo].[SortBookcaseCollectionByBookAverageTotalCount](@BOOKCASE_ID INT,@SHELVES_IDS VARCHAR(2000),@BOOK_IDS VARCHAR(2000),@TITLE VARCHAR(2000))
AS
IF(@SHELVES_IDS IS NOT NULL AND @BOOK_IDS IS NOT NULL)
SELECT DISTINCT COUNT(*) FROM [PublicationsContext].dbo.Books as Books
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.BookId = Books.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.BookRowId = BookcaseBooks.Id
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.Id = ShelvesWithBooks.ShelfRowId
INNER JOIN [BookcaseContext].dbo.Bookcases as Bookcases ON Bookcases.Id = Shelves.BookcaseId
INNER JOIN [RatingsContext].dbo.Books as RatingsBooks ON RatingsBooks.BookId = BookcaseBooks.BookId
WHERE Shelves.BookcaseId = @BOOKCASE_ID
AND ShelvesWithBooks.ShelfRowId IN (SELECT VALUE FROM string_split(@SHELVES_IDS,','))
AND Books.Id IN (SELECT VALUE FROM string_split(@BOOK_IDS,','))
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
GROUP BY Bookcases.Id
IF(@SHELVES_IDS IS NOT NULL)
SELECT DISTINCT COUNT(*) FROM [PublicationsContext].dbo.Books as Books
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.BookId = Books.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.BookRowId = BookcaseBooks.Id
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.Id = ShelvesWithBooks.ShelfRowId
INNER JOIN [BookcaseContext].dbo.Bookcases as Bookcases ON Bookcases.Id = Shelves.BookcaseId
INNER JOIN [RatingsContext].dbo.Books as RatingsBooks ON RatingsBooks.BookId = BookcaseBooks.BookId
WHERE Shelves.BookcaseId = @BOOKCASE_ID
AND ShelvesWithBooks.ShelfRowId IN (SELECT VALUE FROM string_split(@SHELVES_IDS,','))
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
GROUP BY Bookcases.Id
IF(@BOOK_IDS IS NOT NULL)
SELECT DISTINCT COUNT(*) FROM [PublicationsContext].dbo.Books as Books
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.BookId = Books.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.BookRowId = BookcaseBooks.Id
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.Id = ShelvesWithBooks.ShelfRowId
INNER JOIN [BookcaseContext].dbo.Bookcases as Bookcases ON Bookcases.Id = Shelves.BookcaseId
INNER JOIN [RatingsContext].dbo.Books as RatingsBooks ON RatingsBooks.BookId = BookcaseBooks.BookId
WHERE Shelves.BookcaseId = @BOOKCASE_ID
AND Books.Id IN (SELECT VALUE FROM string_split(@BOOK_IDS,','))
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
GROUP BY Bookcases.Id
ELSE
SELECT COUNT(*) FROM [PublicationsContext].dbo.Books as Books
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.BookId = Books.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.BookRowId = BookcaseBooks.Id
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.Id = ShelvesWithBooks.ShelfRowId
INNER JOIN [BookcaseContext].dbo.Bookcases as Bookcases ON Bookcases.Id = Shelves.BookcaseId
INNER JOIN [RatingsContext].dbo.Books as RatingsBooks ON RatingsBooks.BookId = BookcaseBooks.BookId
WHERE Shelves.BookcaseId = @BOOKCASE_ID
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
GROUP BY Bookcases.Id