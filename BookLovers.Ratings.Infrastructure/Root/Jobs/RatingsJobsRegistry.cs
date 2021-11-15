using FluentScheduler;

namespace BookLovers.Ratings.Infrastructure.Root.Jobs
{
    internal class RatingsJobsRegistry : Registry
    {
        public RatingsJobsRegistry()
        {
            NonReentrantAsDefault();

            Schedule<ProcessInboxMessagesJob>().ToRunEvery(15).Seconds();
            Schedule<ProcessOutboxMessagesJob>().ToRunEvery(15).Seconds();
        }
    }
}