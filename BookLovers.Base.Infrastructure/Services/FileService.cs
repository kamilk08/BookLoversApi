using System;
using BookLovers.Base.Infrastructure.Services.Files;

namespace BookLovers.Base.Infrastructure.Services
{
    public class FileService : IDisposable
    {
        public FileSaver FileSaver { get; }

        public FileProvider FileProvider { get; }

        public FileService(FileSaver fileSaver, FileProvider fileProvider)
        {
            FileSaver = fileSaver;
            FileProvider = fileProvider;
        }

        public UploadFile GetFile(string path)
        {
            var fileStream = FileProvider.GetFileStream(path);

            FileProvider.GetContentType(path);
            FileProvider.GetFileName(path);

            return new UploadFile((int) fileStream.Length, FileProvider.ContentType, FileProvider.FileName, fileStream);
        }

        public void SaveFile(UploadFile file, string path)
        {
            FileSaver.SetDirectory(path);

            FileSaver.SetFileToSave(file);

            FileSaver.SaveFile();
        }

        public void Dispose()
        {
            FileProvider?.Dispose();
        }
    }
}