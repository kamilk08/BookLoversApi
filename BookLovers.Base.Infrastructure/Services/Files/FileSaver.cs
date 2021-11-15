using System;
using System.IO;
using System.Web.Hosting;
using BookLovers.Base.Infrastructure.Extensions;

namespace BookLovers.Base.Infrastructure.Services.Files
{
    public class FileSaver : IService
    {
        protected UploadFile File { get; private set; }

        protected DirectoryInfo DirectoryInfo { get; set; }

        public string FilePath { get; private set; }

        public string FileType => File.ContentType;

        public string FileName => File.FileName;

        internal void SaveFile()
        {
            if (!this.DirectoryInfo.Exists)
                DirectoryInfo.Create();

            this.FilePath = string.Concat(DirectoryInfo.ToString(), "/", File.FileName);

            using (var fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                File.InputStream.Seek(0, SeekOrigin.Begin);
                File.InputStream.CopyTo(fs);
            }
        }

        internal void SetDirectory(string path)
        {
            if (path.IsEmpty())
                throw new ArgumentException("Path cannot be empty", nameof(path));

            var selectedPath = HostingEnvironment.MapPath(path);
            if (selectedPath == null)
            {
                this.DirectoryInfo = new DirectoryInfo($@"{path}");
                return;
            }

            this.DirectoryInfo = new DirectoryInfo(selectedPath);
        }

        internal void SetFileToSave(UploadFile file)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file), "Uploaded file cannot be null");
        }
    }
}