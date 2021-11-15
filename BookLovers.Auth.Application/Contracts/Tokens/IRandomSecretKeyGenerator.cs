namespace BookLovers.Auth.Application.Contracts.Tokens
{
    public interface IRandomSecretKeyGenerator
    {
        string CreateSecretKey();
    }
}