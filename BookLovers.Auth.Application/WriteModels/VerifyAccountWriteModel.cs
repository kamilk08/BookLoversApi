namespace BookLovers.Auth.Application.WriteModels
{
    public class VerifyAccountWriteModel
    {
        public string AudienceId { get; set; }

        public string SecretKey { get; set; }
    }
}