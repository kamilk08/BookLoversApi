using FluentScheduler;

namespace BookLovers.Publication.Infrastructure.Root.Jobs
{
    internal class PublicationJobsRegistry : Registry
    {
        public PublicationJobsRegistry()
        {
            NonReentrantAsDefault();

            Schedule<ProcessInboxMessagesJob>().ToRunEvery(15).Seconds();
            Schedule<ProcessOutboxMessagesJob>().ToRunEvery(15).Seconds();
        }
    }
}