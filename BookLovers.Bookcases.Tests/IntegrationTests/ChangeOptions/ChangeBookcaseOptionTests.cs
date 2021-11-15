using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
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
    public class ChangeBookcaseOptionTests :
        IntegrationTest<BookcaseModule, ChangeBookcaseOptionsCommand>
    {
        private readonly Mock<IHttpContextAccessor> _mock = new Mock<IHttpContextAccessor>();
        private readonly Mock<IAppManager> _managerMock = new Mock<IAppManager>();
        private int _readerId;
        private Guid _bookcaseGuid;

        [Test]
        public async Task ChangeBookcaseOption_WhenCalled_ShouldChangeBookcaseOption()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var bookcase =
                await Module.ExecuteQueryAsync<BookcaseByGuidQuery, BookcaseDto>(
                    new BookcaseByGuidQuery(_bookcaseGuid));

            bookcase.Value.BookcaseOptions.Privacy.Should().Be(PrivacyOption.OtherReaders.Value);
        }

        [Test]
        public async Task
            ChangeBookcaseOption_WhenCalledAndCommandIsNull_ShouldReturnCommandValidationResultWithErrors()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
        }

        [Test]
        public async Task
            ChangeBookcaseOption_WhenCalledAndCommandHasInvalidData_ShouldReturnCommandValidationResultWithErrors()
        {
            Command = new ChangeBookcaseOptionsCommand(new ChangeBookcaseOptionsWriteModel()
            {
                BookcaseGuid = Guid.Empty,
                SelectedOptions = new List<SelectedOptionWriteModel>
                {
                    new SelectedOptionWriteModel()
                    {
                        OptionType = 20,
                        Value = 20
                    }
                }
            });

            var validationResult = await Module.SendCommandAsync(Command);

            validationResult.HasErrors.Should().BeTrue();

            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "BookcaseGuid");
        }

        protected override void InitializeRoot()
        {
            var bookcaseConnectionString = Environment.GetEnvironmentVariable(BookcaseContext.ConnectionStringKey);
            if (bookcaseConnectionString.IsEmpty())
                bookcaseConnectionString = E2EConstants.BookcaseConnectionString;

            var bookcaseStoreConnectionString =
                Environment.GetEnvironmentVariable(BookcaseStoreContext.ConnectionStringKey);
            if (bookcaseStoreConnectionString.IsEmpty())
                bookcaseStoreConnectionString = E2EConstants.BookcaseStoreConnectionString;

            _managerMock
                .Setup(s => s.GetConfigValue(BookcaseContext.ConnectionStringKey))
                .Returns(bookcaseConnectionString);

            _managerMock
                .Setup(s => s.GetConfigValue(BookcaseStoreContext.ConnectionStringKey))
                .Returns(bookcaseStoreConnectionString);

            BookcaseModuleStartup.Initialize(_mock.Object, _managerMock.Object, new FakeLogger().GetLogger(),
                PersistenceSettings.Default());
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
                        Value = PrivacyOption.OtherReaders.Value
                    }
                }
            };

            Command = new ChangeBookcaseOptionsCommand(dto);
        }

        protected override void SetCompositionRoot()
        {
            CompositionRoot = new CompositionRoot();
        }
    }
}