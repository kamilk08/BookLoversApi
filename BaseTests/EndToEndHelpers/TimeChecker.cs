using System;

namespace BaseTests.EndToEndHelpers
{
    public class TimeChecker
    {
        private readonly TimeSpan _currentTimeSpan;
        private DateTime _endTime;

        private TimeChecker(TimeSpan timeSpan)
        {
            _currentTimeSpan = timeSpan;
            _endTime = DateTime.Now.Add(timeSpan);
        }

        public static TimeChecker WithSeconds(int seconds) =>
            new TimeChecker(TimeSpan.FromSeconds(seconds));

        public bool TimeEnded() => DateTime.Now > _endTime;

        public void Stop() => _endTime = DateTime.UtcNow;

        public void ResetEndTime() => _endTime = DateTime.Now.Add(_currentTimeSpan);
    }
}