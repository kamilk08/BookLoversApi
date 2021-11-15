using System;
using System.Collections.Generic;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Publication.Events.Book;
using BookLovers.Publication.Infrastructure.Mementos;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Shared.Categories;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Mementos
{
    public class BookMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private Book _book;
        private ISnapshotMaker _snapshotMaker;
        private Fixture _fixture;

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            for (var index = 0; index < 10; ++index)
            {
                _book.AddAuthor(new BookAuthor(_fixture.Create<Guid>()));
                _book.AddBookQuote(new BookQuote(_fixture.Create<Guid>()));
                _book.AddReview(new BookReview(_fixture.Create<Guid>(), _fixture.Create<Guid>()));
            }

            var snapshot = _snapshotMaker.MakeSnapshot(_book);
            snapshot.AggregateGuid.Should().Be(_book.Guid);
            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            for (var index = 0; index < 10; ++index)
            {
                _book.AddAuthor(new BookAuthor(_fixture.Create<Guid>()));
                _book.AddBookQuote(new BookQuote(_fixture.Create<Guid>()));
                _book.AddReview(new BookReview(_fixture.Create<Guid>(), _fixture.Create<Guid>()));
            }

            var snapshot = _snapshotMaker.MakeSnapshot(_book);

            var book = (Book) ReflectionHelper.CreateInstance(typeof(Book));

            var memento = _mementoFactoryMock.Object.Create<Book>();
            memento = (IMemento<Book>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                memento.GetType(), SerializerSettings.GetSerializerSettings());

            book.ApplySnapshot(memento);
            book.Should().BeEquivalentTo(_book);
        }

        [SetUp]
        protected void SetUp()
        {
            _mementoFactoryMock.Setup(s => s.Create<Book>())
                .Returns(new BookMemento());

            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);
            _fixture = new Fixture();
            _book = (Book) ReflectionHelper.CreateInstance(typeof(Book));

            _book.ApplyChange(BookCreated.Initialize()
                .WithAggregate(_fixture.Create<Guid>())
                .WithAuthors(
                    new List<Guid>()
                    {
                        _fixture.Create<Guid>()
                    }).WithTitleAndIsbn("TITLE", "978-83-7515-495-5").WithPublisher(_fixture.Create<Guid>())
                .WithAddedBy(_fixture.Create<Guid>())
                .WithDetails(100, Language.Polish.Value, "DETAILS")
                .WithCategory(Category.Fiction.Value, Category.Fiction.Name)
                .WithCover("COVER_SOURCE", 1, "COVER TYPE")
                .WithCycles(new List<Guid>()
                {
                    Guid.NewGuid()
                }).WithDescription("BOOK_DESCRIPTION", "BOOK_DESCRIPTION_SOURCE")
                .WithSubCategory(
                    SubCategory.FictionSubCategory.Fantasy.Value,
                    SubCategory.FictionSubCategory.Fantasy.Name)
                .WithPublicationDate(new DateTime(1992, 12, 12))
                .WithSeries(_fixture.Create<Guid>(), 1).WithHashTags(new List<string>()));

            _book.CommitEvents();
        }
    }
}