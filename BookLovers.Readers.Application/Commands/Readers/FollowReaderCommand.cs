using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Readers;

namespace BookLovers.Readers.Application.Commands.Readers
{
    public class FollowReaderCommand : ICommand
    {
        public ReaderFollowWriteModel Dto { get; }

        public FollowReaderCommand(ReaderFollowWriteModel dto)
        {
            Dto = dto;
        }
    }
}