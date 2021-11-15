using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.WriteModels.Profiles;

namespace BookLovers.Readers.Application.Commands.Profile
{
    public class EditProfileCommand : ICommand
    {
        public ProfileWriteModel WriteModel { get; }

        public EditProfileCommand(ProfileWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}