using Dapper;

namespace BookLovers.Photos.Models
{
    public static class Queries
    {
        public static CommandDefinition CoverQuery(int bookId)
        {
            var dynamicParameters = new DynamicParameters();

            dynamicParameters.Add("@bookId", bookId);

            return new CommandDefinition(
                "SELECT * FROM [PublicationsContext].dbo.BookCovers as Covers\r\n            JOIN [PublicationsContext].[dbo].Books as Books ON Books.Guid = Covers.BookGuid\r\n            WHERE Books.Id =@bookId",
                dynamicParameters);
        }

        public static CommandDefinition AuthorImageQuery(int authorId)
        {
            var dynamicParameters = new DynamicParameters();

            dynamicParameters.Add("@authorId", authorId);

            return new CommandDefinition(
                "SELECT * FROM [PublicationsContext].dbo.AuthorImages as AuthorImages\r\n            JOIN [PublicationsContext].[dbo].Authors as Authors ON Authors.Guid = AuthorImages.AuthorGuid\r\n            WHERE Authors.Id =@authorId",
                dynamicParameters);
        }

        public static CommandDefinition AvatarQuery(int readerId)
        {
            var dynamicParameters = new DynamicParameters();

            dynamicParameters.Add(nameof(readerId), readerId);

            return new CommandDefinition(
                "SELECT * FROM [ReadersContext].[dbo].[Avatars] WHERE ReaderId=@readerId",
                dynamicParameters);
        }
    }
}