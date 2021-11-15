namespace BookLovers.Auth.Application.Contracts.Tokens
{
    public interface ITokenWriter<T>
    {
        string WriteToken(ITokenDescriptor<T> descriptor);
    }
}