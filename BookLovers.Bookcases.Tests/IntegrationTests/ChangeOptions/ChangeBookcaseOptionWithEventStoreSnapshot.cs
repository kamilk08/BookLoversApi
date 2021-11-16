using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BaseTests.EndToEndHelpers.Mocks;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Bookcases.Application.Commands.Bookcases;
using BookLovers.Bookcases.Application.WriteModels;
using BookLovers.Bookcases.Domain.Services;
using BookLovers.Bookcases.Domain.Settings;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Persistence;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using BookLovers.Bookcases.Infrastructure.Root;
using BookLovers.Bookcases.Store.Persistence;
using BookLovers.Shared.Privacy;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Bookcases.Tests.IntegrationTests.ChangeOptions
{
    public class ChangeBookcaseOptionWithEventStoreSnapshot :
        IntegrationTest<BookcaseModule, ChangeBookcaseOptionsCommand>
    {
        private int _readerId;
        private Guid _bookcaseGuid;
        private Guid _userGuid;
        private int _snapshottingFrequency = 10;
        private int _maxSnapshottingFrequency = 100;

        [Test]
        public async Task ChangeBookcaseOption_WhenCalled_ShouldChangeBookcaseOption()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var bookcase =
                await Module.ExecuteQueryAsync<BookcaseByGuidQuery, BookcaseDto>(
                    new BookcaseByGuidQuery(_bookcaseGuid));

            bookcase.Value.BookcaseOptions.Privacy.Should().Be(PrivacyOption.Public.Value);
        }

        protected override void InitializeRoot()
        {
            _userGuid = Fixture.Create<Guid>();

            var appManagerMock = new Mock<IAppManager>();

            var bookcaseConnectionString = Environment.GetEnvironmentVariable(BookcaseContext.ConnectionStringKey);
            if (bookcaseConnectionString.IsEmpty())
                bookcaseConnectionString = E2EConstants.BookcaseConnectionString;

            var bookcaseStoreConnectionString =
                Environment.GetEnvironmentVariable(BookcaseStoreContext.ConnectionStringKey);
            if (bookcaseStoreConnectionString.IsEmpty())
                bookcaseStoreConnectionString = E2EConstants.BookcaseStoreConnectionString;

            appManagerMock
                .Setup(s => s.GetConfigValue(BookcaseContext.ConnectionStringKey))
                .Returns(bookcaseConnectionString);

            appManagerMock
                .Setup(s => s.GetConfigValue(BookcaseStoreContext.ConnectionStringKey))
                .Returns(bookcaseStoreConnectionString);

            BookcaseModuleStartup.Initialize(new FakeHttpContextAccessor(_userGuid, true), appManagerMock.Object,
                new FakeLogger().GetLogger(),
                new PersistenceSettings(
                    new SnapshotSettings(
                        _snapshottingFrequency,
                        new SnapshotConstraints(_snapshottingFrequency, _maxSnapshottingFrequency)),
                    PersistenceInitialSettings.DropDatabases()));
        }

        protected override Task ClearStore()
        {
            CompositionRoot.Kernel.Get<BookcaseStoreContext>().ClearBookcaseStore();

            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            CompositionRoot.Kernel.Get<BookcaseContext>().CleanBookcaseContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            var bookcaseFactory = new BookcaseFactory();
            _bookcaseGuid = Fixture.Create<Guid>();
            var readerGuid = Fixture.Create<Guid>();
            _readerId = Fixture.Create<int>();

            var bookcase = bookcaseFactory.Create(_bookcaseGuid, readerGuid, _readerId);

            await UnitOfWork.CommitAsync(bookcase);

            var dto = new ChangeBookcaseOptionsWriteModel()
            {
                BookcaseGuid = _bookcaseGuid,
                SelectedOptions = new List<SelectedOptionWriteModel>()
                {
                    new SelectedOptionWriteModel()
                    {
                        OptionType = BookcaseOptionType.Privacy.Value,
                        Value = PrivacyOption.Public.Value
                    }
                }
            };

            Command = new ChangeBookcaseOptionsCommand(dto);

            await Enumerable.Range(20, 30).ForEach(async (i) =>
            {
                Command.WriteModel.SelectedOptions[0].OptionType = BookcaseOptionType.ShelfCapacity.Value;
                Command.WriteModel.SelectedOptions[0].Value = i;

                await Module.SendCommandAsync(Command);
            });

            Command = new ChangeBookcaseOptionsCommand(dto);
            Command.WriteModel.SelectedOptions[0].OptionType = BookcaseOptionType.Privacy.Value;
            Command.WriteModel.SelectedOptions[0].Value = PrivacyOption.Public.Value;
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}