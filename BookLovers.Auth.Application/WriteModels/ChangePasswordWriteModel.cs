namespace BookLovers.Auth.Application.WriteModels
{
    public class ChangePasswordWriteModel
    {
        public string NewPassword { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}