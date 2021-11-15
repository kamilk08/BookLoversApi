using System.Threading.Tasks;
using BookLovers.Auth.Application.Contracts;

namespace BookLovers.Auth.Infrastructure.Services.Emails
{
    internal class EmailService : IEmailService
    {
        public Task SendEmailAsync() => Task.CompletedTask;
    }
}