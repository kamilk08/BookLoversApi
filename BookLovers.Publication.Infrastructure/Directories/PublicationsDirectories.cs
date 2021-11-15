using System;

namespace BookLovers.Publication.Infrastructure.Directories
{
    internal static class PublicationsDirectories
    {
        internal static string AuthorsFilePath(Guid authorGuid) =>
            $"~/App_Data/Authors/{authorGuid}";

        internal static string BooksFilePath(Guid bookGuid) =>
            $"~/App_Data/Books/{bookGuid}";
    }
}