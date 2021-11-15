using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Audiences
{
    public class AddAudienceCommand : ICommand
    {
        public AddAudienceWriteModel WriteModel { get; }

        public AddAudienceCommand(AddAudienceWriteModel writeModel)
        {
            this.WriteModel = writeModel;
        }
    }
}