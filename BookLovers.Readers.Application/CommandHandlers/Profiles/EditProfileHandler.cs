using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Application.Contracts.Conversions;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Domain.Profiles.Services;

namespace BookLovers.Readers.Application.CommandHandlers.Profiles
{
    internal class EditProfileHandler : ICommandHandler<EditProfileCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProfileEditor _editor;

        public EditProfileHandler(IUnitOfWork unitOfWork, ProfileEditor editor)
        {
            _unitOfWork = unitOfWork;
            _editor = editor;
        }

        public async Task HandleAsync(EditProfileCommand command)
        {
            var profile = await _unitOfWork.GetAsync<Profile>(command.WriteModel.ProfileGuid);
            var profileData = command.WriteModel.ConvertToProfileData();

            _editor.EditProfile(profile, profileData);

            await _unitOfWork.CommitAsync(profile);
        }
    }
}