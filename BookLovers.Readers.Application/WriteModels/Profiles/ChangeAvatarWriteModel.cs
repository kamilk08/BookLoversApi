namespace BookLovers.Readers.Application.WriteModels.Profiles
{
    public class ChangeAvatarWriteModel
    {
        public string Avatar { get; set; }

        public string FileName { get; set; }

        public bool HasImage => !string.IsNullOrEmpty(Avatar);
    }
}