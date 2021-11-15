using System;
using System.IO;
using BookLovers.Base.Infrastructure.Extensions;

namespace BookLovers.Base.Infrastructure.Services.Files
{
    public class FileProvider : IService, IDisposable
    {
        private FileStream _fileStream;

        public string FileName { get; private set; }

        public string ContentType { get; private set; }

        internal FileStream GetFileStream(string path)
        {
            if (path.IsEmpty())
                throw new ArgumentNullException(nameof(path), "File path cannot be null or empty");

            _fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            return _fileStream;
        }

        internal void GetFileName(string path)
        {
            this.FileName = Path.GetFileName(path);
        }

        internal void GetContentType(string path)
        {
            this.ContentType = Path.GetExtension(path);
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
        }
    }
}