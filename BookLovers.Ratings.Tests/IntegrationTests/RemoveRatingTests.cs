using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
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
    public class RemoveRatingTests : IntegrationTest<RatingsModule, RemoveRatingCommand>
    {
        private Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private Mock<IBookInBookcaseChecker> _checkerMock = new Mock<IBookInBookcaseChecker>();
        private Guid _readerGuid;
        private int _readerId;
        private Book _book;
        private int _bookId;

        [Test]
        public async Task RemoveRating_WhenCalled_ShouldRemoveBookRating()
        {
            var queryResult =
                await this.Module.ExecuteQueryAsync<ReaderBookRatingQuery, RatingDto>(
                    new ReaderBookRatingQuery(this._bookId, this._readerId));

            queryResult.Value.Should().NotBeNull();

            var validationResult = await this.Module.SendCommandAsync(this.Command);
            validationResult.HasErrors.Should().BeFalse();

            queryResult =
                await this.Module.ExecuteQueryAsync<ReaderBookRatingQuery, RatingDto>(
                    new ReaderBookRatingQuery(this._bookId, this._readerId));

            queryResult.Value.Should().BeNull();
        }

        [Test]
        public async Task RemoveRating_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            this.Command = null;

            var validationResult = await this.Module.SendCommandAsync(this.Command);

            validationResult.HasErrors.Should().BeTrue();
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

            var removeRatingDto = this.Fixture.Build<RemoveRatingWriteModel>().With(p => p.BookGuid, bookGuid).Create();

            this.Command = new RemoveRatingCommand(removeRatingDto);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}