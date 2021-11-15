using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseTests;
using BaseTests.EndToEndHelpers;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Application.Commands.Tickets;
using BookLovers.Librarians.Application.WriteModels;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Persistence;
using BookLovers.Librarians.Infrastructure.Queries.Tickets;
using BookLovers.Librarians.Infrastructure.Root;
using FluentAssertions;
using Moq;
using Ninject;
using NUnit.Framework;

namespace BookLovers.Librarians.Tests.IntegrationTests
{
    public class NewTicketTests : IntegrationTest<LibrarianModule, NewTicketCommand>
    {
        private readonly Mock<IHttpContextAccessor> _httpAccessor = new Mock<IHttpContextAccessor>();
        private Guid _ticketGuid;
        private Guid _readerGuid;

        [Test]
        public async Task CreateTicket_WhenCalled_ShouldCreateNewTicket()
        {
            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeFalse();

            var queryResult =
                await Module.ExecuteQueryAsync<TicketByGuidQuery, TicketDto>(new TicketByGuidQuery(_ticketGuid));
            queryResult.Value.Guid.Should().Be(_ticketGuid);
        }

        [Test]
        public async Task CreateTicket_WhenCalledAndCommandIsNotValid_ShouldReturnFailureResult()
        {
            Command = null;

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.First().ErrorMessage.Should().Be("Command cannot be null.");
        }

        [Test]
        public async Task CreateTicket_WhenCalledAndCommandHasInvalidData_ShouldReturnFailureResult()
        {
            Command = new NewTicketCommand(new CreateTicketWriteModel
            {
                TicketGuid = Guid.Empty
            });

            var validationResult = await Module.SendCommandAsync(Command);
            validationResult.HasErrors.Should().BeTrue();
            validationResult.Errors.Should().Contain(p => p.ErrorProperty == "TicketGuid");
        }

        protected override void InitializeRoot()
        {
            _readerGuid = Fixture.Create<Guid>();
            _httpAccessor.Setup(s => s.UserGuid).Returns(_readerGuid);

            var appManagerMock = new Mock<IAppManager>();
            
            var librariansConnectionString = Environment.GetEnvironmentVariable(LibrariansContext.ConnectionStringKey);
            if (librariansConnectionString.IsEmpty())
                librariansConnectionString = E2EConstants.LibrariansConnectionString;
            
            appManagerMock.Setup(s => s.GetConfigValue(LibrariansContext.ConnectionStringKey))
                .Returns(librariansConnectionString);

            LibrarianModuleStartup.Initialize(_httpAccessor.Object, appManagerMock.Object, new FakeLogger().GetLogger(),
                PersistenceSettings.Default());
        }

        protected override Task ClearStore()
        {
            return Task.CompletedTask;
        }

        protected override Task ClearReadContext()
        {
            CompositionRoot.Kernel.Get<LibrariansContext>().CleanLibrarianContext();

            return Task.CompletedTask;
        }

        protected override async Task PrepareData()
        {
            _ticketGuid = Fixture.Create<Guid>();

            var readerId = Fixture.Create<int>();

            var ticketOwner = new TicketOwner(Fixture.Create<Guid>(), _readerGuid, readerId);

            await UnitOfWork.CommitAsync(ticketOwner);

            var dto = Fixture.Build<CreateTicketWriteModel>()
                .With(p => p.Description)
                .With(p => p.Title)
                .With(p => p.CreatedAt)
                .With(p => p.TicketConcern, TicketConcern.Author.Value)
                .With(p => p.TicketData)
                .With(p => p.TicketGuid, _ticketGuid)
                .Create();

            Command = new NewTicketCommand(dto);
        }

        protected override void SetCompositionRoot()
        {
            this.CompositionRoot = new CompositionRoot();
        }
    }
}