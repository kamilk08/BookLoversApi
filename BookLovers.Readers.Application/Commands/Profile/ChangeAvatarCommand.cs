using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Profiles;

namespace BookLovers.Readers.Application.Commands.Profile
{
    public class ChangeAvatarCommand : ICommand
    {
        public ChangeAvatarWriteModel WriteModel { get; }

        public ChangeAvatarCommand(ChangeAvatarWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}