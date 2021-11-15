using System;
using BookLovers.Base.Infrastructure.Services;

namespace BaseTests.EndToEndHelpers.Mocks
{
    public class FakeReadContextAccessor : IReadContextAccessor
    {
        private int _readeModelId;

        public int GetReadModelId() => _readeModelId;

        public void SetReadModelId(int readModelId) => _readeModelId = readModelId;

        public int GetReadModelId(Guid guid) => _readeModelId;
    }
}