using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Librarians.Application.WriteModels;

namespace BookLovers.Librarians.Application.Commands.Tickets
{
    public class NewTicketCommand : ICommand
    {
        public CreateTicketWriteModel WriteModel { get; }

        public NewTicketCommand(CreateTicketWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}