namespace BookLovers.Auth.Application.WriteModels
{
    public class ChangeEmailWriteModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string NextEmail { get; set; }
    }
}