using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Application.WriteModels.Reviews;
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

namespace BookLovers.Readers.Tests.IntegrationTests.EditReview
{
    public class EditReviewTestsWithEventStoreSnapshot :
        IntegrationTest<ReadersModule, EditReviewCommand>
    {
        private readonly Mock<IHttpContextAccessor> _httpContextMock = new Mock<IHttpContextAccessor>();
        private Guid _readerGuid;
        private ReviewWriteModel _reviewWriteModel;
        private string _userName;
        private int _bookId;
        private int _readerId;

        [Test]
        public async Task EditReview_WhenCalled_ShouldEditExistingReview()
        {
            var validationResult = await this.Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<ReaderBookReviewQuery, ReviewDto>(
                    new ReaderBookReviewQuery(_readerId, _bookId));

            queryResult.Should().NotBeNull();

            queryResult.Value.Guid.Should().Be(_reviewWriteModel.ReviewGuid);
            queryResult.Value.Likes.Should().BeEmpty();
            queryResult.Value.Review.Should().Be(_reviewWriteModel.ReviewDetails.Content);
            queryResult.Value.AddedBy.Should().Be(_userName);
            queryResult.Value.BookGuid.Should().Be(_reviewWriteModel.BookGuid);
            queryResult.Value.MarkedAsSpoiler.Should().Be(_reviewWriteModel.ReviewDetails.MarkedAsSpoiler);
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();
            _httpContextMock.Setup(s => s.UserGuid).Returns(_readerGuid);

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

            ReadersModuleStartup.Initialize(_httpContextMock.Object, appManagerMock.Object,
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

            _readerId = this.Fixture.Create<int>();
            var profileGuid = this.Fixture.Create<Guid>();
            var notificationWallGuid = this.Fixture.Create<Guid>();
            var statisticsGathererGuid = this.Fixture.Create<Guid>();
            _userName = this.Fixture.Create<string>();
            var email = this.Fixture.Create<string>();

            var identification = new ReaderIdentification(_readerId, _userName, email);
            var socials = new ReaderSocials(profileGuid, notificationWallGuid, statisticsGathererGuid);

            var reader = readerFactory.Create(_readerGuid, identification, socials);

            await UnitOfWork.CommitAsync(reader);

            identification = new ReaderIdentification(Fixture.Create<int>(), Fixture.Create<string>(),
                Fixture.Create<string>());
            socials = new ReaderSocials(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<Guid>());

            var readerWhoLikesAndUnLikesReview = readerFactory.Create(Fixture.Create<Guid>(), identification, socials);

            await UnitOfWork.CommitAsync(readerWhoLikesAndUnLikesReview);

            var reviewFactory = new ReviewFactory();

            var bookGuid = Fixture.Create<Guid>();
            _bookId = Fixture.Create<int>();
            var date = Fixture.Create<DateTime>();

            var bookAdder = new BookResourceAdder();
            bookAdder.AddResource(reader, new AddedBook(bookGuid, _bookId, date));

            await UnitOfWork.CommitAsync(reader);

            _reviewWriteModel = Fixture.Build<ReviewWriteModel>()
                .With(p => p.BookGuid, bookGuid)
                .With(p => p.ReviewGuid)
                .With(p => p.ReviewDetails)
                .Create();

            var reviewParts = ReviewParts.Initialize()
                .WithGuid(_reviewWriteModel.ReviewGuid)
                .WitBook(bookGuid)
                .AddedBy(_readerGuid)
                .WithContent(Fixture.Create<string>())
                .WithDates(Fixture.Create<DateTime>(), Fixture.Create<DateTime>())
                .HasSpoilers();

            var review = reviewFactory.Create(reviewParts);

            await UnitOfWork.CommitAsync(review);

            this._httpContextMock.Setup(s => s.UserGuid).Returns(readerWhoLikesAndUnLikesReview.Guid);

            await Enumerable.Range(0, 30).ForEach(async (i) =>
            {
                await this.Module.SendCommandAsync(new LikeReviewCommand(review.Guid));
                await this.Module.SendCommandAsync(new UnlikeReviewCommand(review.Guid));
            });

            this.Command = new EditReviewCommand(_reviewWriteModel);

            this._httpContextMock.Setup(s => s.UserGuid).Returns(_readerGuid);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}