using System;
using AutoFixture;
using BookLovers.Base.Infrastructure.AppCaching;
using FluentAssertions;
using NUnit.Framework;

namespace BaseTests.Infrastructure
{
    [TestFixture]
    public class MemoryCacheTests
    {
        private CacheService _cacheService;
        private Fixture _fixture;

        [Test]
        public void GetValue_WhenCalled_ShouldReturnObjectStoredInMemory()
        {
            var key = _fixture.Create<string>();
            var value = _fixture.Create<object>();
            var timeOffSet = new DateTimeOffset(DateTime.UtcNow.AddHours(1));

            _cacheService.Add(key, value, timeOffSet);

            var retrievedObject = _cacheService.GetValue(key);

            retrievedObject.Should().NotBeNull();
            retrievedObject.Should().BeOfType<object>();
        }

        [Test]
        public void GetValue_WhenCalledAndThereIsNoObjectWithGivenKey_ShouldReturnNull()
        {
            var key = _fixture.Create<string>();

            var retrievedObject = _cacheService.GetValue(key);
            retrievedObject.Should().BeNull();
        }

        [Test]
        public void AddValue_WhenCalled_ShouldAddObjectToMemoryAndReturnTrue()
        {
            var key = _fixture.Create<string>();
            var value = _fixture.Create<object>();
            var timeOffSet = new DateTimeOffset(DateTime.UtcNow.AddHours(1));

            var result = _cacheService.Add(key, value, timeOffSet);
            result.Should().BeTrue();
        }

        [Test]
        public void AddValue_WhenCalledAndThereIsAnObjectWithGivenKey_ShouldReturnFalse()
        {
            var key = _fixture.Create<string>();
            var value = _fixture.Create<object>();
            var timeOffSet = new DateTimeOffset(DateTime.UtcNow.AddHours(1));

            _cacheService.Add(key, value, timeOffSet);

            var result = _cacheService.Add(key, value, timeOffSet);

            result.Should().BeFalse();
        }

        [Test]
        public void Delete_WhenCalled_ShouldRemoveEntryAndReturnTrue()
        {
            var key = _fixture.Create<string>();
            var value = _fixture.Create<object>();
            var timeOffSet = new DateTimeOffset(DateTime.UtcNow.AddHours(1));

            _cacheService.Add(key, value, timeOffSet);

            var result = _cacheService.Delete(key);

            result.Should().BeTrue();
        }

        [Test]
        public void Delete_WhenCalledAndThereIsNoEntryWithGivenKey_ShouldReturnFalse()
        {
            var key = _fixture.Create<string>();

            var result = _cacheService.Delete(key);

            result.Should().BeFalse();
        }

        [Test]
        public void Contains_WhenCalledAndThereIsAnEntryWithGivenKey_ShouldReturnTrue()
        {
            var key = _fixture.Create<string>();
            var value = _fixture.Create<object>();
            var timeOffSet = new DateTimeOffset(DateTime.UtcNow.AddHours(1));

            _cacheService.Add(key, value, timeOffSet);

            var result = _cacheService.Contains(key);

            result.Should().BeTrue();
        }

        [Test]
        public void Contains_WhenCalledAndThereIsNoEntryWithGivenKey_ShouldReturnFalse()
        {
            var key = _fixture.Create<string>();

            var result = _cacheService.Contains(key);

            result.Should().BeFalse();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _cacheService = new CacheService();
        }
    }
}