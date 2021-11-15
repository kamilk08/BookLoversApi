using System.Threading.Tasks;

namespace BookLovers.Auth.Application.Contracts
{
    public interface IEmailService
    {
        Task SendEmailAsync();
    }
}