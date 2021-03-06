CREATE PROCEDURE [dbo].[SortBookcaseCollectionByDate](@BOOKCASE_ID INT, @SHELVES_IDS VARCHAR(2000), @BOOK_IDS VARCHAR(2000),@TITLE VARCHAR(2000), @ORDER_BY BIT,@ROW_COUNT INT,@SKIP INT)
AS
IF(@SHELVES_IDS IS NOT NULL AND @BOOK_IDS IS NOT NULL AND @ORDER_BY = 1)
SELECT DISTINCT * FROM BookcaseContext.dbo.Bookcases as Bookcases
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.BookcaseId = Bookcases.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.ShelfRowId = Shelves.Id
INNER JOIN [BookcaseContext].dbo.ShelfRecords as ShelfRecords ON ShelfRecords.ShelfId = ShelvesWithBooks.ShelfRowId AND ShelfRecords.BookRowId = ShelvesWithBooks.BookRowId
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.Id = ShelvesWithBooks.BookRowId
INNER JOIN [PublicationsContext].dbo.Books as Books ON Books.Id = BookcaseBooks.BookId
WHERE Bookcases.Id = @BOOKCASE_ID
AND ShelvesWithBooks.ShelfRowId IN (SELECT VALUE FROM string_split(@SHELVES_IDS,','))
AND Books.Id IN (SELECT VALUE FROM string_split(@BOOK_IDS,','))
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
ORDER BY ShelfRecords.AddedAt DESC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY
IF(@SHELVES_IDS IS NOT NULL AND @ORDER_BY = 1)
SELECT DISTINCT * FROM BookcaseContext.dbo.Bookcases as Bookcases
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.BookcaseId = Bookcases.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.ShelfRowId = Shelves.Id
INNER JOIN [BookcaseContext].dbo.ShelfRecords as ShelfRecords ON ShelfRecords.ShelfId = ShelvesWithBooks.ShelfRowId AND ShelfRecords.BookRowId = ShelvesWithBooks.BookRowId
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.Id = ShelvesWithBooks.BookRowId
INNER JOIN [PublicationsContext].dbo.Books as Books ON Books.Id = BookcaseBooks.BookId
WHERE Bookcases.Id = @BOOKCASE_ID
AND ShelvesWithBooks.ShelfRowId IN (SELECT VALUE FROM string_split(@SHELVES_IDS,','))
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
ORDER BY ShelfRecords.AddedAt DESC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY
IF(@BOOK_IDS IS NOT NULL AND @ORDER_BY = 1)
SELECT DISTINCT * FROM BookcaseContext.dbo.Bookcases as Bookcases
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.BookcaseId = Bookcases.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.ShelfRowId = Shelves.Id
INNER JOIN [BookcaseContext].dbo.ShelfRecords as ShelfRecords ON ShelfRecords.ShelfId = ShelvesWithBooks.ShelfRowId AND ShelfRecords.BookRowId = ShelvesWithBooks.BookRowId
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.Id = ShelvesWithBooks.BookRowId
INNER JOIN [PublicationsContext].dbo.Books as Books ON Books.Id = BookcaseBooks.BookId
WHERE Bookcases.Id = @BOOKCASE_ID
AND Books.Id IN (SELECT VALUE FROM string_split(@BOOK_IDS,','))
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
ORDER BY ShelfRecords.AddedAt DESC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY
IF(@BOOK_IDS IS NULL AND @SHELVES_IDS IS NULL AND @ORDER_BY = 1)
SELECT DISTINCT * FROM BookcaseContext.dbo.Bookcases as Bookcases
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.BookcaseId = Bookcases.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.ShelfRowId = Shelves.Id
INNER JOIN [BookcaseContext].dbo.ShelfRecords as ShelfRecords ON ShelfRecords.ShelfId = ShelvesWithBooks.ShelfRowId AND ShelfRecords.BookRowId = ShelvesWithBooks.BookRowId
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.Id = ShelvesWithBooks.BookRowId
INNER JOIN [PublicationsContext].dbo.Books as Books ON Books.Id = BookcaseBooks.BookId
WHERE Bookcases.Id = @BOOKCASE_ID
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
ORDER BY ShelfRecords.AddedAt DESC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY
IF(@SHELVES_IDS IS NOT NULL AND @BOOK_IDS IS NOT NULL AND @ORDER_BY = 0)
SELECT DISTINCT * FROM BookcaseContext.dbo.Bookcases as Bookcases
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.BookcaseId = Bookcases.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.ShelfRowId = Shelves.Id
INNER JOIN [BookcaseContext].dbo.ShelfRecords as ShelfRecords ON ShelfRecords.ShelfId = ShelvesWithBooks.ShelfRowId AND ShelfRecords.BookRowId = ShelvesWithBooks.BookRowId
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.Id = ShelvesWithBooks.BookRowId
INNER JOIN [PublicationsContext].dbo.Books as Books ON Books.Id = BookcaseBooks.BookId
WHERE Bookcases.Id = @BOOKCASE_ID
AND ShelvesWithBooks.ShelfRowId IN (SELECT VALUE FROM string_split(@SHELVES_IDS,','))
AND Books.Id IN (SELECT VALUE FROM string_split(@BOOK_IDS,','))
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
ORDER BY ShelfRecords.AddedAt ASC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY
IF(@SHELVES_IDS IS NOT NULL AND @ORDER_BY = 0)
SELECT DISTINCT * FROM BookcaseContext.dbo.Bookcases as Bookcases
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.BookcaseId = Bookcases.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.ShelfRowId = Shelves.Id
INNER JOIN [BookcaseContext].dbo.ShelfRecords as ShelfRecords ON ShelfRecords.ShelfId = ShelvesWithBooks.ShelfRowId AND ShelfRecords.BookRowId = ShelvesWithBooks.BookRowId
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.Id = ShelvesWithBooks.BookRowId
INNER JOIN [PublicationsContext].dbo.Books as Books ON Books.Id = BookcaseBooks.BookId
WHERE Bookcases.Id = @BOOKCASE_ID
AND ShelvesWithBooks.ShelfRowId IN (SELECT VALUE FROM string_split(@SHELVES_IDS,','))
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
ORDER BY ShelfRecords.AddedAt ASC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY
IF(@BOOK_IDS IS NOT NULL AND @ORDER_BY = 0)
SELECT DISTINCT * FROM BookcaseContext.dbo.Bookcases as Bookcases
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.BookcaseId = Bookcases.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.ShelfRowId = Shelves.Id
INNER JOIN [BookcaseContext].dbo.ShelfRecords as ShelfRecords ON ShelfRecords.ShelfId = ShelvesWithBooks.ShelfRowId AND ShelfRecords.BookRowId = ShelvesWithBooks.BookRowId
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.Id = ShelvesWithBooks.BookRowId
INNER JOIN [PublicationsContext].dbo.Books as Books ON Books.Id = BookcaseBooks.BookId
WHERE Bookcases.Id = @BOOKCASE_ID
AND Books.Id IN (SELECT VALUE FROM string_split(@BOOK_IDS,','))
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
ORDER BY ShelfRecords.AddedAt ASC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY
IF(@BOOK_IDS IS NULL AND @SHELVES_IDS IS NULL AND @ORDER_BY =0)
SELECT DISTINCT * FROM BookcaseContext.dbo.Bookcases as Bookcases
INNER JOIN [BookcaseContext].dbo.Shelves as Shelves ON Shelves.BookcaseId = Bookcases.Id
INNER JOIN [BookcaseContext].dbo.ShelvesWithBooks as ShelvesWithBooks ON ShelvesWithBooks.ShelfRowId = Shelves.Id
INNER JOIN [BookcaseContext].dbo.ShelfRecords as ShelfRecords ON ShelfRecords.ShelfId = ShelvesWithBooks.ShelfRowId AND ShelfRecords.BookRowId = ShelvesWithBooks.BookRowId
INNER JOIN [BookcaseContext].dbo.Books as BookcaseBooks ON BookcaseBooks.Id = ShelvesWithBooks.BookRowId
INNER JOIN [PublicationsContext].dbo.Books as Books ON Books.Id = BookcaseBooks.BookId
WHERE Bookcases.Id = @BOOKCASE_ID
AND Books.Title LIKE @TITLE + '%'
AND BookcaseBooks.Status =1
ORDER BY ShelfRecords.AddedAt ASC
OFFSET @SKIP ROWS FETCH NEXT @ROW_COUNT ROWS ONLY