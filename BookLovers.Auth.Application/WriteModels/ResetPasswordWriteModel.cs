namespace BookLovers.Auth.Application.WriteModels
{
    public class ResetPasswordWriteModel
    {
        public string Token { get; set; }

        public string Password { get; set; }
    }
}