using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Application.WriteModels.Reviews;
using BookLovers.Readers.Domain.Favourites;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Readers.Store.Persistence;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.IntegrationTests
{
    public class CreateReviewTests : IntegrationTest<ReadersModule, AddReviewCommand>
    {
        private Guid _readerGuid;
        private Guid _reviewGuid;
        private Guid _bookGuid;

        [Test]
        public async Task CreateReview_WhenCalled_ShouldReturnValidationResultWithNoErrorsAndCreateReview()
        {
            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<ReviewByGuidQuery, ReviewDto>(new ReviewByGuidQuery(_reviewGuid));

            queryResult.Value.Should().NotBeNull();
        }

        [Test]
        public async Task CreateReview_WhenCalledAndReviewHasInvalidData_ShouldReturnValidationResultWithErrors()
        {
            Command = new AddReviewCommand(new ReviewWriteModel
            {
                BookGuid = Guid.Empty,
                ReviewDetails = new ReviewDetailsWriteModel
                {
                    Content = string.Empty,
                    ReviewDate = default(DateTime)
                },
                ReviewGuid = Guid.Empty
            });

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "BookGuid");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "Content");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "ReviewDate");
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "ReviewGuid");
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();

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
            
            ReadersModuleStartup.Initialize(new FakeHttpContextAccessor(_readerGuid, true), appManagerMock.Object,
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
            _reviewGuid = Fixture.Create<Guid>();
            _bookGuid = Fixture.Create<Guid>();

            var identification = new ReaderIdentification(Fixture.Create<int>(), Fixture.Create<string>(),
                Fixture.Create<string>());

            var readerSocials =
                new ReaderSocials(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<Guid>());

            var reader = new Reader(_readerGuid, identification, readerSocials);

            await UnitOfWork.CommitAsync(reader);

            await UnitOfWork.CommitAsync(new Favourite(_bookGuid, Fixture.Create<int>(), _readerGuid));

            var readersContext = CompositionRoot.Kernel.Get<ReadersContext>();

            readersContext.Books.Add(new BookReadModel
            {
                BookGuid = _bookGuid,
                BookId = Fixture.Create<int>()
            });

            await readersContext.SaveChangesAsync();

            Command = new AddReviewCommand(new ReviewWriteModel
            {
                BookGuid = _bookGuid,
                ReviewDetails = new ReviewDetailsWriteModel
                {
                    Content = Fixture.Create<string>(),
                    ReviewDate = Fixture.Create<DateTime>()
                },
                ReviewGuid = _reviewGuid
            });
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}