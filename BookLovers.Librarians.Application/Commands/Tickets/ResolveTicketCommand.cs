using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.WriteModels;

namespace BookLovers.Librarians.Application.Commands.Tickets
{
    public class ResolveTicketCommand : ICommand
    {
        public ResolveTicketWriteModel WriteModel { get; }

        public ResolveTicketCommand(ResolveTicketWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}