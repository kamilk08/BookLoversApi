using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Services;
using FluentAssertions;
using NUnit.Framework;

namespace BaseTests.Infrastructure
{
    [TestFixture]
    public class DistinguisherTests
    {
        private Fixture _fixture;

        [Test]
        public void ToAdd_WhenCalledWithTwoDifferentSequences_ShouldReturnElementsToAdd()
        {
            var add = Distinguisher<int>.ToAdd(
                _fixture.Build<IEnumerable<int>>()
                    .FromSeed(factory => Enumerable.Range(0, 5)).Create(),
                _fixture.Build<IEnumerable<int>>()
                    .FromSeed(factory => Enumerable.Range(2, 7)).Create());

            add.Should().NotBeNull();

            add.Should().ContainInOrder(Enumerable.Range(5, 4));
        }

        [Test]
        public void ToAdd_WhenCalledWithTwoSameSequences_ShouldReturnEmptyEnumerable()
        {
            var add = Distinguisher<int>.ToAdd(
                _fixture.Build<IEnumerable<int>>()
                    .FromSeed(factory => Enumerable.Range(0, 5)).Create(),
                _fixture.Build<IEnumerable<int>>()
                    .FromSeed(factory => Enumerable.Range(0, 5)).Create());

            add.Should().NotBeNull();
            add.Should().BeEmpty();
        }

        [Test]
        public void ToRemove_WhenCalledWithTwoDifferentSequences_ShouldReturnElementsToRemove()
        {
            var remove = Distinguisher<int>.ToRemove(
                _fixture.Build<IEnumerable<int>>()
                    .FromSeed(factory => Enumerable.Range(0, 5)).Create(),
                _fixture.Build<IEnumerable<int>>()
                    .FromSeed(factory => Enumerable.Range(2, 7)).Create());

            remove.Should().NotBeNull();
            remove.Should().ContainInOrder(Enumerable.Range(0, 2));
        }

        [Test]
        public void ToRemove_WhenCalledWithTwoSameSequences_ShouldReturnEmptyEnumerable()
        {
            var remove = Distinguisher<int>.ToRemove(
                _fixture.Build<IEnumerable<int>>()
                    .FromSeed(factory => Enumerable.Range(0, 5)).Create(),
                _fixture.Build<IEnumerable<int>>()
                    .FromSeed(factory => Enumerable.Range(0, 5)).Create());

            remove.Should().NotBeNull();
            remove.Should().BeEmpty();
        }

        [SetUp]
        public void SetUp() => _fixture = new Fixture();
    }
}