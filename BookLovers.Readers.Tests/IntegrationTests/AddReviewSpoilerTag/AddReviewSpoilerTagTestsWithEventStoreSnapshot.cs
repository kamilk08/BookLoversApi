﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
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

namespace BookLovers.Readers.Tests.IntegrationTests.AddReviewSpoilerTag
{
    public class AddReviewSpoilerTagTestsWithEventStoreSnapshot :
        IntegrationTest<ReadersModule, AddSpoilerTagCommand>
    {
        private Guid _readerGuid;
        private Guid _tagAdderGuid;
        private int _readerId;
        private int _bookId;

        [Test]
        public async Task AddReviewSpoilerTag_WhenCalled_ShouldAddNewSpoilerTag()
        {
            var validationResult = await this.Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<ReaderBookReviewQuery, ReviewDto>(
                    new ReaderBookReviewQuery(_readerId, _bookId));

            queryResult.Value.SpoilerTags.Count.Should().Be(1);
        }

        protected override void InitializeRoot()
        {
            _tagAdderGuid = Fixture.Create<Guid>();
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
            
            ReadersModuleStartup.Initialize(new FakeHttpContextAccessor(_tagAdderGuid, true), appManagerMock.Object,
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

            _readerGuid = this.Fixture.Create<Guid>();
            _readerId = this.Fixture.Create<int>();
            var profileGuid = this.Fixture.Create<Guid>();
            var notificationWallGuid = this.Fixture.Create<Guid>();
            var statisticsGathererGuid = this.Fixture.Create<Guid>();
            var userName = this.Fixture.Create<string>();
            var email = this.Fixture.Create<string>();

            var identification = new ReaderIdentification(_readerId, userName, email);
            var socials = new ReaderSocials(profileGuid, notificationWallGuid, statisticsGathererGuid);

            var reader = readerFactory.Create(_readerGuid, identification, socials);

            await UnitOfWork.CommitAsync(reader);

            var tagAdderEmail = Fixture.Create<string>();
            var tagAdderId = Fixture.Create<int>();
            var tagAdderIdentification = new ReaderIdentification(tagAdderId, Fixture.Create<string>(), tagAdderEmail);
            var tagAdderSocials =
                new ReaderSocials(Fixture.Create<Guid>(), Fixture.Create<Guid>(), Fixture.Create<Guid>());

            var spoilerTagAdder = readerFactory.Create(_tagAdderGuid, tagAdderIdentification, tagAdderSocials);

            await UnitOfWork.CommitAsync(spoilerTagAdder);

            var bookGuid = Fixture.Create<Guid>();
            _bookId = Fixture.Create<int>();
            var date = Fixture.Create<DateTime>();

            var reviewParts = ReviewParts.Initialize()
                .WithGuid(Fixture.Create<Guid>())
                .WitBook(bookGuid)
                .AddedBy(_readerGuid)
                .WithContent(Fixture.Create<string>())
                .WithDates(Fixture.Create<DateTime>(), Fixture.Create<DateTime>())
                .HasSpoilers();

            var bookAdder = new BookResourceAdder();
            bookAdder.AddResource(reader, new AddedBook(bookGuid, _bookId, date));

            await UnitOfWork.CommitAsync(reader);

            var reviewFactory = new ReviewFactory();
            var review = reviewFactory.Create(reviewParts);

            await UnitOfWork.CommitAsync(review);

            await Enumerable.Range(0, 30).ForEach(async (i) =>
            {
                await this.Module.SendCommandAsync(new AddSpoilerTagCommand(review.Guid));
                await this.Module.SendCommandAsync(new RemoveSpoilerTagCommand(review.Guid));
            });

            this.Command = new AddSpoilerTagCommand(review.Guid);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}