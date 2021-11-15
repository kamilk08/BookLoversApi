using System;
using System.Threading.Tasks;

namespace BaseTests.EndToEndHelpers
{
    public class Poller
    {
        private readonly TimeChecker _timeChecker;
        private readonly int _pollingTimeInMillis;

        public Poller(TimeChecker timeChecker)
        {
            _timeChecker = timeChecker;
            _pollingTimeInMillis = 1000;
        }

        public Poller(TimeChecker timeChecker, int pollingTimeInMillis)
        {
            _timeChecker = timeChecker;
            _pollingTimeInMillis = pollingTimeInMillis;
        }

        public async Task Check(IProbe probe)
        {
            while (!probe.IsSatisfied())
            {
                if (_timeChecker.TimeEnded())
                    throw new TimeoutException("Time has ended.");

                await Task.Delay(_pollingTimeInMillis);
                await probe.SampleAsync();
            }
        }

        public void Reset() => _timeChecker.ResetEndTime();
    }
}