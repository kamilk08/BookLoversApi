using System;

namespace BookLovers.Readers.Application.WriteModels.Readers
{
    public class ReaderWriteModel
    {
        public Guid ReaderId { get; set; }

        public ReaderDetailsWriteModel DetailsWriteModel { get; set; }

        public ReaderSocialDetailsWriteModel SocialDetailsWriteModel { get; set; }
    }
}