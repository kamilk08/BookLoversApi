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
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.IntegrationTests.LikeReview
{
    public class LikeReviewTests : IntegrationTest<ReadersModule, LikeReviewCommand>
    {
        private Mock<IHttpContextAccessor> _httpAccessorMock = new Mock<IHttpContextAccessor>();
        private Guid _readerGuid;
        private Guid _likeAdderGuid;
        private int _readerId;
        private int _bookId;

        [Test]
        public async Task AddReviewLike_WhenCalled_ShouldAddLikeToReview()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<ReaderBookReviewQuery, ReviewDto>(
                    new ReaderBookReviewQuery(_readerId, _bookId));

            queryResult.Value.Likes.Count.Should().Be(1);
        }

        [Test]
        public async Task AddReviewLike_WhenCalledAndCommandIsNotValid_ShouldReturnValidationResultWithErrors()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
        }

        [Test]
        public async Task AddReviewLike_WhenCalledAndCommandDataIsNotValid_ShouldReturnValidationResultWithErrors()
        {
            Command = new LikeReviewCommand(Guid.Empty);

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.First().ErrorMessage.Should().Be("Review guid cannot be empty");
        }

        [Test]
        public void
            AddReviewLike_WhenCalledAndReaderTriesToLikeOwnReview_ShouldThrowBusinessRuleNotMeetException()
        {
            _httpAccessorMock.Setup(s => s.UserGuid).Returns(_readerGuid);

            Func<Task> act = async () => await Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Reader cannot like own reviews.");
        }

        [Test]
        public async Task
            AddReviewLike_WhenCalledAndReviewWasAlreadyLikedByTheReader_ShouldThrowBusinessRuleNotMeetException()
        {
            await Module.SendCommandAsync(Command);

            Func<Task> act = async () => await Module.SendCommandAsync(Command);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Review cannot have multiple likes from same reader.");
        }

        protected override void InitializeRoot()
        {
            _likeAdderGuid = Fixture.Create<Guid>();
            _httpAccessorMock.Setup(s => s.UserGuid).Returns(_likeAdderGuid);

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
            
            ReadersModuleStartup.Initialize(_httpAccessorMock.Object, appManagerMock.Object,
                new FakeLogger().GetLogger(),
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
            var gathererGuid = this.Fixture.Create<Guid>();
            var userName = this.Fixture.Create<string>();
            var email = this.Fixture.Create<string>();
            var identification = new ReaderIdentification(_readerId, userName, email);
            var socials = new ReaderSocials(profileGuid, notificationWallGuid, gathererGuid);

            var reader = readerFactory.Create(_readerGuid, identification, socials);

            await UnitOfWork.CommitAsync(reader);

            var likeAdderEmail = Fixture.Create<string>();
            var likeAdderId = Fixture.Create<int>();
            var likeAdderIdentification =
                new ReaderIdentification(likeAdderId, Fixture.Create<string>(), likeAdderEmail);
            var likeAdderSocials =
                new ReaderSocials(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<Guid>());

            var likeAdder = readerFactory.Create(_likeAdderGuid, likeAdderIdentification, likeAdderSocials);

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

            await UnitOfWork.CommitAsync(review);

            Command = new LikeReviewCommand(reviewParts.ReviewGuid);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}