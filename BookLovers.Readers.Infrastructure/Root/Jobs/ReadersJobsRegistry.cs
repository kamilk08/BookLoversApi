using FluentScheduler;

namespace BookLovers.Readers.Infrastructure.Root.Jobs
{
    internal class ReadersJobsRegistry : Registry
    {
        public ReadersJobsRegistry()
        {
            this.NonReentrantAsDefault();

            this.Schedule<ProcessInboxMessagesJob>().ToRunEvery(15).Seconds();
            this.Schedule<ProcessOutboxMessagesJob>().ToRunEvery(15).Seconds();
        }
    }
}