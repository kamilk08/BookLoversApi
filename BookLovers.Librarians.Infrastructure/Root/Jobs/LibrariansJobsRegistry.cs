using FluentScheduler;

namespace BookLovers.Librarians.Infrastructure.Root.Jobs
{
    internal class LibrariansJobsRegistry : Registry
    {
        public LibrariansJobsRegistry()
        {
            NonReentrantAsDefault();
            Schedule<ProcessInboxMessagesJob>().ToRunEvery(15).Seconds();
            Schedule<ProcessOutboxMessagesJob>().ToRunEvery(15).Seconds();
        }
    }
}