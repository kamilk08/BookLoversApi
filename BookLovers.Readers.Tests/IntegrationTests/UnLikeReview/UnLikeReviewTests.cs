using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.Services;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Readers.Domain.Reviews.Services;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Readers.Store.Persistence;
using BookLovers.Shared.Likes;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.IntegrationTests.UnLikeReview
{
    public class UnLikeReviewTests : IntegrationTest<ReadersModule, UnlikeReviewCommand>
    {
        private Mock<IHttpContextAccessor> _httpAccessorMock = new Mock<IHttpContextAccessor>();
        private Guid _reviewUnlikerGuid;
        private Guid _readerGuid;
        private int _bookId;
        private int _readerId;

        [Test]
        public async Task UnLikeReview_WhenCalled_ShouldRemoveLike()
        {
            var queryResult =
                await Module.ExecuteQueryAsync<ReaderBookReviewQuery, ReviewDto>(
                    new ReaderBookReviewQuery(_readerId, _bookId));

            queryResult.Value.Likes.Count.Should().Be(1);

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            queryResult = await Module.ExecuteQueryAsync<ReaderBookReviewQuery, ReviewDto>(
                new ReaderBookReviewQuery(_readerId, _bookId));

            queryResult.Value.Likes.Count.Should().Be(0);
        }

        [Test]
        public async Task UnLikeReview_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
        }

        [Test]
        public async Task UnLikeReview_WhenCalledAndCommandHasInvalidData_ShouldReturnValidationResultWithErrors()
        {
            Command = new UnlikeReviewCommand(Guid.Empty);

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "ReviewGuid");
        }

        [Test]
        public async Task UnLikeReview_WhenCalledAndThereIsNoLikeToRemove_ShouldThrowBusinessRuleNotMeetException()
        {
            await Module.SendCommandAsync(Command);

            Func<Task> act = async () => await Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Review has not been liked by selected reader.");
        }

        protected override void InitializeRoot()
        {
            _reviewUnlikerGuid = Fixture.Create<Guid>();
            _httpAccessorMock.Setup(s => s.UserGuid).Returns(_reviewUnlikerGuid);

            var appManagerMock = new Mock<IAppManager>();
            var readersConnectionString = Environment.GetEnvironmentVariable(ReadersContext.ConnectionStringKey);
            if (readersConnectionString.IsEmpty())
                readersConnectionString = E2EConstants.ReadersConnectionString;

            var readersStoreConnectionString =
                Environment.GetEnvironmentVariable(ReadersStoreContext.ConnectionStringKey);
            if (readersStoreConnectionString.IsEmpty())
                readersStoreConnectionString = E2EConstants.ReadersStoreConnectionString;

            appManagerMock.Setup(s => s.GetConfigValue(ReadersContext.ConnectionStringKey))
                .Returns(readersConnectionString);

            appManagerMock.Setup(s => s.GetConfigValue(ReadersStoreContext.ConnectionStringKey))
                .Returns(readersStoreConnectionString);

            ReadersModuleStartup.Initialize(_httpAccessorMock.Object, appManagerMock.Object, new FakeLogger().GetLogger(),
                PersistenceSettings.Default());
        }

        protected override Task ClearStore()
        {
            CompositionRoot.Kernel.Get<ReadersStoreContext>().CleanReadersStore();

            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            CompositionRoot.Kernel.Get<ReadersContext>().CleanReadersContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            var readerFactory = new ReaderFactory();

            _readerGuid = this.Fixture.Create<Guid>();
            _readerId = this.Fixture.Create<int>();
            var profileGuid = this.Fixture.Create<Guid>();
            var notificationWallGuid = this.Fixture.Create<Guid>();
            var userName = this.Fixture.Create<string>();
            var email = this.Fixture.Create<string>();

            var identification = new ReaderIdentification(_readerId, userName, email);
            var socials = new ReaderSocials(profileGuid, notificationWallGuid, Fixture.Create<Guid>());

            var reader = readerFactory.Create(_readerGuid, identification, socials);

            await UnitOfWork.CommitAsync(reader);

            var likeAdderEmail = Fixture.Create<string>();
            var likeAdderId = Fixture.Create<int>();
            var likeAdderIdentification =
                new ReaderIdentification(likeAdderId, Fixture.Create<string>(), likeAdderEmail);
            var likeAdderSocials =
                new ReaderSocials(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<Guid>());

            var likeAdder = readerFactory.Create(_reviewUnlikerGuid, likeAdderIdentification, likeAdderSocials);

            await UnitOfWork.CommitAsync(likeAdder);

            var bookGuid = Fixture.Create<Guid>();
            _bookId = Fixture.Create<int>();
            var date = Fixture.Create<DateTime>();

            var bookAdder = new BookResourceAdder();
            bookAdder.AddResource(reader, new AddedBook(bookGuid, _bookId, date));

            await UnitOfWork.CommitAsync(reader);

            var reviewParts = ReviewParts.Initialize()
                .WithGuid(Fixture.Create<Guid>())
                .WitBook(bookGuid)
                .AddedBy(_readerGuid)
                .WithContent(Fixture.Create<string>())
                .WithDates(Fixture.Create<DateTime>(), Fixture.Create<DateTime>())
                .HasSpoilers();

            var reviewFactory = new ReviewFactory();

            var review = reviewFactory.Create(reviewParts);

            review.AddLike(Like.NewLike(_reviewUnlikerGuid));

            await UnitOfWork.CommitAsync(review);

            Command = new UnlikeReviewCommand(reviewParts.ReviewGuid);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}