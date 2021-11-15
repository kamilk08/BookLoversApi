namespace BookLovers.Librarians.Domain.Tickets.Services
{
    public interface ITicketConcernProvider
    {
        TicketConcern GetTicketConcern(int concernType);
    }
}