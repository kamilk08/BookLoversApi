using System;
using System.Threading.Tasks;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Application.WriteModels;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Domain.Tickets.Services;
using BookLovers.Librarians.Domain.Tickets.Services.Factories;
using BookLovers.Librarians.Events.TicketOwners;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookLovers.Librarians.Tests.UnitTests
{
    [TestFixture]
    public class TicketOwnerTests
    {
        private TicketOwner _ticketOwner;
        private Fixture _fixture;
        private Ticket _ticket;
        private TicketFactory _ticketFactory;
        private Mock<ITicketConcernProvider> _tickerConcernProviderMock;
        private Mock<ITicketOwnerRepository> _ticketOwnerRepositoryMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<ITicketConcernChecker> _concernCheckerMock;

        [Test]
        public void AddTicket_WhenCalled_ShouldAddNewTicket()
        {
            _ticketOwner.AddTicket(_ticket);

            _ticketOwner.Tickets.Should().HaveCount(1);
            _ticketOwner.Tickets.Should().ContainSingle(p => p.TicketGuid == _ticket.Guid);
        }

        [Test]
        public void AddTicket_WhenCalledAndTicketOwnerIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _ticketOwner.ArchiveAggregate();

            Action act = () => _ticketOwner.AddTicket(_ticket);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddTicket_WhenCalledAndThereIsAlreadyAExactSameTicket_ShouldThrowBusinessRuleNotMeetException()
        {
            _ticketOwner.AddTicket(_ticket);

            Action act = () => _ticketOwner.AddTicket(_ticket);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Ticket cannot be duplicated.");
        }

        public void AddTicket_WhenCalledAndTicketWasIssuedByDifferentPerson_ShouldThrowBusinessRuleNotMeetException()
        {
            var ownerGuid = _fixture.Create<Guid>();
            var readerId = _fixture.Create<int>();

            _ticketOwnerRepositoryMock
                .Setup(p => p.GetOwnerByReaderGuidAsync(ownerGuid))
                .ReturnsAsync(new TicketOwner(_fixture.Create<Guid>(), ownerGuid, readerId));

            _ticketFactory = new TicketFactorySetup(new TicketFactory(), _ticketOwnerRepositoryMock.Object,
                    _tickerConcernProviderMock.Object, _httpContextAccessorMock.Object, _concernCheckerMock.Object)
                .GetFactory(TicketFactoryData.Initialize()
                    .WithGuid(_fixture.Create<Guid>())
                    .WithTicketObject(_fixture.Create<Guid>())
                    .WithContent(new TicketContentData(_fixture.Create<string>(), _fixture.Create<string>()))
                    .WithDetails(new TicketDetailsData(_fixture.Create<DateTime>(), TicketConcern.Book.Value,
                        _fixture.Create<string>())));

            _ticket = _ticketFactory.Create();

            Action act = () => _ticketOwner.AddTicket(_ticket);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Invalid owner of a ticket.");
        }

        [Test]
        public void NotifyOwner_WhenCalled_ShouldNotifyOwnerAboutResolvedTicket()
        {
            _ticketOwner.AddTicket(_ticket);
            _ticketOwner.NotifyOwner(CreatedBookAccepted.Initialize()
                .WithAggregate(_ticketOwner.Guid)
                .WithTicketOwner(_ticketOwner.ReaderGuid)
                .WithTicket(_ticket.Guid, _fixture.Create<string>())
                .WithBook(_fixture.Create<Guid>(), _fixture.Create<string>()));

            _ticketOwner.Tickets.Should().HaveCount(1);
            _ticketOwner.Tickets.Should().ContainSingle(p => p.IsSolved);

            var uncommittedEvents = _ticketOwner.GetUncommittedEvents();
            uncommittedEvents.Should().HaveCount(1);

            var @events = _ticketOwner.GetUncommittedEvents();
            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(CreatedBookAccepted));
        }

        [Test]
        public void NotifyOwner_WhenCalled_ShouldThrowBusinessRuleNotMeetException()
        {
            _ticketOwner.ArchiveAggregate();

            var notification = CreatedBookAccepted.Initialize()
                .WithAggregate(_ticketOwner.Guid)
                .WithTicketOwner(_ticketOwner.ReaderGuid)
                .WithTicket(_ticket.Guid, _fixture.Create<string>())
                .WithBook(_fixture.Create<Guid>(), _fixture.Create<string>());

            Action act = () => _ticketOwner.NotifyOwner(notification);
        }

        [Test]
        public void IsTicketSolved_WhenCalledAndTicketIsSolved_ShouldReturnTrue()
        {
            _ticketOwner.AddTicket(_ticket);

            var notification = CreatedBookAccepted.Initialize()
                .WithAggregate(_ticketOwner.Guid)
                .WithTicketOwner(_ticketOwner.ReaderGuid)
                .WithTicket(_ticket.Guid, _fixture.Create<string>())
                .WithBook(_fixture.Create<Guid>(), _fixture.Create<string>());

            _ticketOwner.NotifyOwner(notification);

            var result = _ticketOwner.IsTicketSolved(_ticket.Guid);
            result.Should().BeTrue();
        }

        [Test]
        public void IsTicketSolved_WhenCalledAndTicketIsNotSolved_ShouldReturnFalse()
        {
            _ticketOwner.AddTicket(_ticket);

            var result = _ticketOwner.IsTicketSolved(_ticket.Guid);

            result.Should().BeFalse();
        }

        [Test]
        public void HasPendingTickets_WhenCalledAndThereAreOnlySolvedTickets_ShouldReturnFalse()
        {
            _ticketOwner.AddTicket(_ticket);

            var notification = CreatedBookAccepted.Initialize()
                .WithAggregate(_ticketOwner.Guid)
                .WithTicketOwner(_ticketOwner.ReaderGuid)
                .WithTicket(_ticket.Guid, _fixture.Create<string>())
                .WithBook(_fixture.Create<Guid>(), _fixture.Create<string>());

            _ticketOwner.NotifyOwner(notification);

            var result = _ticketOwner.HasPendingTickets();
            result.Should().BeFalse();
        }

        [Test]
        public void HadPendingTickets_WhenCalledAndThereAreSomeUnsolvedTickets_ShouldReturnTrue()
        {
            _ticketOwner.AddTicket(_ticket);

            var result = _ticketOwner.HasPendingTickets();
            result.Should().BeTrue();
        }

        [Test]
        public void GetAllPendingTickets_WhenCalledAndThereIsNoUnsolvedTickets_ShouldReturnEmptySequence()
        {
            _ticketOwner.AddTicket(_ticket);

            var notification = CreatedBookAccepted.Initialize()
                .WithAggregate(_ticketOwner.Guid)
                .WithTicketOwner(_ticketOwner.ReaderGuid)
                .WithTicket(_ticket.Guid, _fixture.Create<string>())
                .WithBook(_fixture.Create<Guid>(), _fixture.Create<string>());

            _ticketOwner.NotifyOwner(notification);

            var result = _ticketOwner.GetAllPendingTickets();

            result.Should().HaveCount(0);
        }

        [Test]
        public void GetAllPendingTickets_WhenCalledAndThereAreSomeUnsolvedTickets_ShouldReturnThem()
        {
            _ticketOwner.AddTicket(_ticket);

            var result = _ticketOwner.GetAllPendingTickets();

            result.Should().HaveCount(1);
        }

        [SetUp]
        public Task SetUp()
        {
            _fixture = new Fixture();
            _tickerConcernProviderMock = new Mock<ITicketConcernProvider>();
            _ticketOwnerRepositoryMock = new Mock<ITicketOwnerRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _concernCheckerMock = new Mock<ITicketConcernChecker>();

            var ticketOwnerGuid = _fixture.Create<Guid>();
            var readerId = _fixture.Create<int>();

            _fixture.Build<CreateTicketWriteModel>()
                .With(p => p.Description).With(p => p.Title)
                .With(p => p.CreatedAt)
                .With(p => p.TicketConcern, TicketConcern.Book.Value)
                .With(p => p.TicketData).With(p => p.TicketGuid)
                .Create();

            _tickerConcernProviderMock.Setup(p => p.GetTicketConcern(TicketConcern.Book.Value))
                .Returns(TicketConcern.Book);

            _ticketOwnerRepositoryMock.Setup(p => p.GetOwnerByReaderGuid(ticketOwnerGuid))
                .Returns(new TicketOwner(_fixture.Create<Guid>(), ticketOwnerGuid, readerId));

            _httpContextAccessorMock.Setup(s => s.UserGuid).Returns(ticketOwnerGuid);

            _concernCheckerMock.Setup(s => s.IsConcernValid(It.IsAny<int>()))
                .Returns(true);

            _ticketFactory =
                new TicketFactorySetup(new TicketFactory(), _ticketOwnerRepositoryMock.Object,
                        _tickerConcernProviderMock.Object, _httpContextAccessorMock.Object, _concernCheckerMock.Object)
                    .GetFactory(TicketFactoryData.Initialize()
                        .WithGuid(_fixture.Create<Guid>())
                        .WithTicketObject(_fixture.Create<Guid>())
                        .WithContent(new TicketContentData(_fixture.Create<string>(), _fixture.Create<string>()))
                        .WithDetails(new TicketDetailsData(_fixture.Create<DateTime>(), TicketConcern.Book.Value,
                            _fixture.Create<string>())));

            _ticket = _ticketFactory.Create();

            _ticketOwner = new TicketOwner(_fixture.Create<Guid>(), ticketOwnerGuid, readerId);

            return Task.CompletedTask;
        }
    }
}