using System.Threading;
using FluentScheduler;

namespace BookLovers.Base.Infrastructure.Messages
{
    public abstract class NonConcurrentJob : IJob
    {
        private static readonly object JobLocker = new object();

        public abstract void Execute();

        void IJob.Execute()
        {
            if (Monitor.IsEntered(JobLocker))
                return;

            lock (JobLocker)
                Execute();
        }
    }
}