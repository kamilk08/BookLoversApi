using BookLovers.Librarians.Application.WriteModels;
using BookLovers.Librarians.Domain.Tickets.Services.Factories;

namespace BookLovers.Librarians.Application.Extensions
{
    internal static class ConversionExtensions
    {
        internal static TicketFactoryData ConvertToTicketData(
            this CreateTicketWriteModel writeModel)
        {
            return TicketFactoryData.Initialize()
                .WithGuid(writeModel.TicketGuid)
                .WithTicketObject(writeModel.TicketObjectGuid)
                .WithContent(new TicketContentData(writeModel.Title, writeModel.TicketData))
                .WithDetails(
                    new TicketDetailsData(writeModel.CreatedAt, writeModel.TicketConcern, writeModel.Description));
        }
    }
}