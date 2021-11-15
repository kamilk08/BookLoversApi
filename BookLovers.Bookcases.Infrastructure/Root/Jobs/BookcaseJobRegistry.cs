using FluentScheduler;

namespace BookLovers.Bookcases.Infrastructure.Root.Jobs
{
    internal class BookcaseJobRegistry : Registry
    {
        public BookcaseJobRegistry()
        {
            NonReentrantAsDefault();
            Schedule<ProcessInBoxMessagesJob>().ToRunEvery(15).Seconds();
            Schedule<ProcessOutboxMessagesJob>().ToRunEvery(15).Seconds();
        }
    }
}