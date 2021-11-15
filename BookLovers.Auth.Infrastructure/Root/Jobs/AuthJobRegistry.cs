using FluentScheduler;

namespace BookLovers.Auth.Infrastructure.Root.Jobs
{
    public class AuthJobRegistry : Registry
    {
        public AuthJobRegistry()
        {
            NonReentrantAsDefault();

            Schedule<ProcessOutboxMessagesJob>().ToRunEvery(15).Seconds();

            Schedule<ProcessInboxMessagesJob>().ToRunEvery(15).Seconds();

            Schedule<CheckNotConfirmedAccountsJob>().ToRunEvery(24).Hours();
        }
    }
}