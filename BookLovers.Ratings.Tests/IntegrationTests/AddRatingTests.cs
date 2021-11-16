using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Ratings.Application.Commands;
using BookLovers.Ratings.Application.WriteModels;
using BookLovers.Ratings.Domain;
using BookLovers.Ratings.Domain.Authors;
using BookLovers.Ratings.Domain.Books;
using BookLovers.Ratings.Domain.RatingGivers;
using BookLovers.Ratings.Domain.RatingStars;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Queries.Ratings;
using BookLovers.Ratings.Infrastructure.Root;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Ratings.Tests.IntegrationTests
{
    public class AddRatingTests : IntegrationTest<RatingsModule, AddRatingCommand>
    {
        private Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private Mock<IBookInBookcaseChecker> _checkerMock = new Mock<IBookInBookcaseChecker>();

        private Guid _readerGuid;
        private Book _book;
        private int _bookId;
        private int _readerId;

        [Test]
        public async Task AddBookRating_WhenCalled_ShouldAddBookRating()
        {
            var validationResult = await this.Module.SendCommandAsync(this.Command);
            validationResult.HasErrors.Should().BeFalse();
            var rating = await this.Module.ExecuteQueryAsync<ReaderBookRatingQuery, RatingDto>(
                new ReaderBookRatingQuery(this._bookId, this._readerId));

            rating.Should().NotBeNull();
        }

        [Test]
        public async Task AddBookRating_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            this.Command = null;

            var validationResult = await this.Module.SendCommandAsync(this.Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
        }

        [Test]
        public async Task AddBookRating_WhenCalledAndCommandAndDataIsNotValid_ShouldReturnFailureResult()
        {
            this.Command = new AddRatingCommand(new AddRatingWriteModel()
            {
                BookGuid = Guid.Empty,
                Stars = -1
            });

            var validationResult = await this.Module.SendCommandAsync(this.Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "BookGuid");
        }

        [Test]
        public async Task AddBookRating_WhenCalledAndBookIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            var book = await this.UnitOfWork.GetAsync<Book>(this._book.Guid);
            book.ArchiveAggregate();
            await this.UnitOfWork.CommitAsync(book);

            Func<Task> act = async () => await this.Module.SendCommandAsync(this.Command);
            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public async Task
            AddBookRating_WhenCalledAndBookAlreadyHasRatingFromCertainReader_ShouldThrowBusinessRuleNotMeetException()
        {
            await this.Module.SendCommandAsync(this.Command);

            Func<Task> act = async () => await this.Module.SendCommandAsync(this.Command);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Book cannot have multiple ratings from same reader.");
        }

        protected override void InitializeRoot()
        {
            this._readerGuid = this.Fixture.Create<Guid>();
            this._mock.Setup(s => s.UserGuid).Returns(this._readerGuid);

            var appManagerMock = new Mock<IAppManager>();

            var ratingsConnectionString = Environment.GetEnvironmentVariable(RatingsContext.ConnectionStringKey);
            if (ratingsConnectionString.IsEmpty())
                ratingsConnectionString = E2EConstants.RatingsConnectionString;

            appManagerMock.Setup(s => s.GetConfigValue(RatingsContext.ConnectionStringKey))
                .Returns(ratingsConnectionString);

            this._checkerMock.Setup(s => s.IsBookInBookcaseAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);

            RatingsModuleStartup.Initialize(this._mock.Object, appManagerMock.Object, new FakeLogger().GetLogger(),
                PersistenceSettings.Default());

            RatingsModuleStartup.AddOrUpdateService(this._checkerMock.Object, true);
        }

        protected override Task ClearStore()
        {
            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            this.CompositionRoot.Kernel.Get<RatingsContext>().CleanRatingsContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            this._readerId = this.Fixture.Create<int>();
            var reader = new RatingGiver(Guid.NewGuid(), this._readerGuid, this._readerId);
            await this.UnitOfWork.CommitAsync(reader);

            var bookGuid = this.Fixture.Create<Guid>();
            this._bookId = this.Fixture.Create<int>();

            var author =
                Author.Create(new AuthorIdentification(this.Fixture.Create<Guid>(), this.Fixture.Create<int>()));
            await this.UnitOfWork.CommitAsync(author);

            var authorRepository = this.CompositionRoot.Kernel.Get<IAuthorRepository>();
            var authorWithId = await authorRepository.GetByAuthorGuidAsync(author.Identification.AuthorGuid);

            this._book = Book.Create(new BookIdentification(bookGuid, this._bookId), new List<Author>()
            {
                authorWithId
            });

            await this.UnitOfWork.CommitAsync(this._book);

            var bookcaseGuid = this.Fixture.Create<Guid>();

            var addRatingDto = this.Fixture.Build<AddRatingWriteModel>()
                .With(p => p.BookGuid, bookGuid)
                .With(p => p.Stars, Star.Five.Value)
                .Create();

            this.Command = new AddRatingCommand(addRatingDto);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}