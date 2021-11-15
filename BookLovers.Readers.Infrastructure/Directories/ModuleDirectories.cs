using System;

namespace BookLovers.Readers.Infrastructure.Directories
{
    public class ModuleDirectories
    {
        private static string ReadersFilePath(Guid guid)
        {
            return $"~/App_Data/Readers/{guid}";
        }

        public static string GetReadersDirectory(Guid readerGuid) =>
            ModuleDirectories.ReadersFilePath(readerGuid);
    }
}