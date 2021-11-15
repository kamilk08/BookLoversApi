using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Readers;

namespace BookLovers.Readers.Application.Commands.Readers
{
    public class UnFollowReaderCommand : ICommand
    {
        public ReaderFollowWriteModel Dto { get; }

        public UnFollowReaderCommand(ReaderFollowWriteModel dto)
        {
            Dto = dto;
        }
    }
}