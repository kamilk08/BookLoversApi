using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Domain.ReviewReportRegisters;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Application.WriteModels.Reviews;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.Services;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Readers.Domain.Reviews.Services;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Readers.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.IntegrationTests.ReportReview
{
    public class ReportReviewTests : IntegrationTest<ReadersModule, ReportReviewCommand>
    {
        private Guid _readerWhoReports;
        private Guid _readerGuid;
        private int _readerId;
        private string _userName;
        private int _bookId;

        [Test]
        public async Task ReportReview_WhenCalled_ShouldReportReview()
        {
            var validationResult = await this.Module.SendCommandAsync(this.Command);
            validationResult.HasErrors.Should().BeFalse();
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();
            _readerWhoReports = Fixture.Create<Guid>();

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
            
            ReadersModuleStartup.Initialize(new FakeHttpContextAccessor(_readerWhoReports, true), appManagerMock.Object,
                new FakeLogger().GetLogger(), PersistenceSettings.Default());
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
            _userName = this.Fixture.Create<string>();
            var email = this.Fixture.Create<string>();

            var identification = new ReaderIdentification(_readerId, _userName, email);
            var socials = new ReaderSocials(profileGuid, notificationWallGuid, Fixture.Create<Guid>());

            var reader = readerFactory.Create(_readerGuid, identification, socials);

            await UnitOfWork.CommitAsync(reader);

            identification = new ReaderIdentification(Fixture.Create<int>(), Fixture.Create<string>(),
                Fixture.Create<string>());
            socials = new ReaderSocials(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<Guid>());

            var secondReader = readerFactory.Create(_readerWhoReports, identification, socials);

            await UnitOfWork.CommitAsync(secondReader);

            var reviewFactory = new ReviewFactory();

            var bookGuid = Fixture.Create<Guid>();
            _bookId = Fixture.Create<int>();
            var date = Fixture.Create<DateTime>();

            var bookAdder = new BookResourceAdder();
            bookAdder.AddResource(reader, new AddedBook(bookGuid, _bookId, date));

            await UnitOfWork.CommitAsync(reader);

            var reviewDto = Fixture.Build<ReviewWriteModel>()
                .With(p => p.BookGuid, bookGuid)
                .With(p => p.ReviewGuid)
                .With(p => p.ReviewDetails)
                .Create();

            var reviewParts = ReviewParts.Initialize()
                .WithGuid(reviewDto.ReviewGuid)
                .WitBook(bookGuid)
                .AddedBy(_readerGuid)
                .WithContent(Fixture.Create<string>())
                .WithDates(Fixture.Create<DateTime>(), Fixture.Create<DateTime>())
                .HasSpoilers();

            var review = reviewFactory.Create(reviewParts);

            await UnitOfWork.CommitAsync(review);

            Command = new ReportReviewCommand(new ReportReviewWriteModel
            {
                ReportReasonId = ReportReason.Spam.Value,
                ReviewGuid = review.Guid
            });
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}