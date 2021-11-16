using System;
using System.Collections.Generic;
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
    public class ChangeRatingTests : IntegrationTest<RatingsModule, ChangeRatingCommand>
    {
        private Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private Mock<IBookInBookcaseChecker> _checkerMock = new Mock<IBookInBookcaseChecker>();
        private Guid _readerGuid;
        private Book _book;
        private int _readerId;
        private int _bookId;

        [Test]
        public async Task ChangeRating_WhenCalled_ShouldChangeBookRating()
        {
            var validationResult = await this.Module.SendCommandAsync(this.Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult = await this.Module.ExecuteQueryAsync<ReaderBookRatingQuery, RatingDto>(
                new ReaderBookRatingQuery(this._bookId, this._readerId));

            queryResult.Should().NotBeNull();
            queryResult.Value.Stars.Should().Be(Star.Four.Value);
        }

        [Test]
        public async Task ChangeRating_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            this.Command = null;

            var validationResult = await this.Module.SendCommandAsync(this.Command);

            validationResult.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task ChangeRating_WhenCalledAndCommandHasNotValidData_ShouldReturnFailureResult()
        {
            this.Command = new ChangeRatingCommand(new ChangeRatingWriteModel()
            {
                BookGuid = Guid.Empty,
                Stars = -1
            });

            var validationResult = await this.Module.SendCommandAsync(this.Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "BookGuid");
        }

        [Test]
        public async Task
            ChangeRating_WhenCalledAndBookDoesNotHaveRatingFromSelectedReader_ShouldThrowBusinessRuleNotMeetException()
        {
            await this.Module.SendCommandAsync(new RemoveRatingCommand(
                new RemoveRatingWriteModel()
                {
                    BookGuid = this._book.Identification.BookGuid
                }));

            this.Command.WriteModel.BookGuid = Guid.NewGuid();

            Func<Task> act = async () => await this.Module.SendCommandAsync(this.Command);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void ChangeRating_WhenCalledAndRatingIsNotValid_ShouldReturnFailureResult()
        {
            this.Command.WriteModel.Stars = -1;

            Func<Task> func = async () => await this.Module.SendCommandAsync(this.Command);

            func.Should().Throw<BusinessRuleNotMetException>().WithMessage("Rating stars is not valid.");
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

            var reader = new RatingGiver(this.Fixture.Create<Guid>(), this._readerGuid, this._readerId);
            await this.UnitOfWork.CommitAsync(reader);

            var author =
                Author.Create(new AuthorIdentification(this.Fixture.Create<Guid>(), this.Fixture.Create<int>()));
            await this.UnitOfWork.CommitAsync(author);

            var bookGuid = this.Fixture.Create<Guid>();
            this._bookId = this.Fixture.Create<int>();

            this._book = Book.Create(new BookIdentification(bookGuid, this._bookId), new List<Author>()
            {
                author
            });

            await this.UnitOfWork.CommitAsync(this._book);

            this._book.AddRating(new Rating(this._book.Identification.BookId, this._readerId, Star.Five.Value));
            await this.UnitOfWork.CommitAsync(this._book);

            var editRatingDto = this.Fixture.Build<ChangeRatingWriteModel>()
                .With(p => p.Stars, Star.Four.Value)
                .With(p => p.BookGuid, bookGuid)
                .Create();

            this.Command = new ChangeRatingCommand(editRatingDto);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}