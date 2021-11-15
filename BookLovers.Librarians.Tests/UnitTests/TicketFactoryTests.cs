using System;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Domain.TicketOwners;
using BookLovers.Librarians.Domain.Tickets;
using BookLovers.Librarians.Domain.Tickets.Services;
using BookLovers.Librarians.Domain.Tickets.Services.Factories;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookLovers.Librarians.Tests.UnitTests
{
    [TestFixture]
    public class TicketFactoryTests
    {
        private TicketFactory _ticketFactory;
        private TicketFactoryData _factoryData;
        private Mock<ITicketConcernProvider> _ticketConcernProviderMock;
        private Mock<ITicketOwnerRepository> _ticketOwnerRepositoryMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<ITicketConcernChecker> _concernCheckerMock;

        private Fixture _fixture;
        private Guid _readerGuid;

        [Test]
        public void Create_WhenCalled_ShouldCreateValidTicket()
        {
            var ticket = _ticketFactory.Create();

            ticket.Should().NotBeNull();

            ticket.IssuedBy.TicketOwnerGuid.Should().Be(_readerGuid);
            ticket.SolvedBy.LibrarianGuid.Should().BeEmpty();

            ticket.TicketContent.Content.Should().Be(_factoryData.TicketContentData.TicketData);
            ticket.TicketContent.TicketConcern.Should().Be(TicketConcern.Book);

            ticket.TicketDetails.Date.Should().Be(_factoryData.TicketDetailsData.CreatedAt);
            ticket.TicketDetails.Description.Should().Be(_factoryData.TicketDetailsData.Description);
            ticket.TicketDetails.Title.Should().Be(_factoryData.TicketContentData.Title);

            ticket.TicketState.Should().Be(TicketState.InProgress);
        }

        [Test]
        public void Create_WhenCalledAndTicketReasonIsNotValid_ShouldThrowBusinessRuleNotMeetException()
        {
            _factoryData.WithDetails(new TicketDetailsData(_fixture.Create<DateTime>(), _fixture.Create<int>(),
                _fixture.Create<string>()));

            Action act = () => _ticketFactory.Create();

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void Create_WhenCalledAndTicketDoesNotHaveOwner_ShouldThrowBusinessRuleNotMeetException()
        {
            _ticketOwnerRepositoryMock
                .Setup(p => p.GetOwnerByReaderGuid(_readerGuid))
                .Returns(Activator.CreateInstance(typeof(TicketOwner), true) as TicketOwner);

            Action act = () => _ticketFactory.Create();
            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Ticket must have his owner.");
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _ticketConcernProviderMock = new Mock<ITicketConcernProvider>();
            _ticketOwnerRepositoryMock = new Mock<ITicketOwnerRepository>();
            _concernCheckerMock = new Mock<ITicketConcernChecker>();

            _readerGuid = _fixture.Create<Guid>();
            var readerId = _fixture.Create<int>();

            _ticketConcernProviderMock.Setup(p => p.GetTicketConcern(TicketConcern.Book.Value))
                .Returns(TicketConcern.Book);

            _ticketOwnerRepositoryMock.Setup(p => p.GetOwnerByReaderGuid(_readerGuid))
                .Returns(new TicketOwner(_fixture.Create<Guid>(), _readerGuid, readerId));

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _httpContextAccessorMock.Setup(s => s.UserGuid).Returns(_readerGuid);

            _concernCheckerMock.Setup(s => s.IsConcernValid(It.IsAny<int>()))
                .Returns(true);

            _factoryData = TicketFactoryData.Initialize()
                .WithGuid(_fixture.Create<Guid>())
                .WithTicketObject(_fixture.Create<Guid>())
                .WithContent(new TicketContentData(_fixture.Create<string>(), _fixture.Create<string>()))
                .WithDetails(
                    new TicketDetailsData(_fixture.Create<DateTime>(), TicketConcern.Book.Value,
                        _fixture.Create<string>()));

            _ticketFactory = new TicketFactorySetup(new TicketFactory(), _ticketOwnerRepositoryMock.Object,
                    _ticketConcernProviderMock.Object, _httpContextAccessorMock.Object, _concernCheckerMock.Object)
                .GetFactory(_factoryData);
        }
    }
}