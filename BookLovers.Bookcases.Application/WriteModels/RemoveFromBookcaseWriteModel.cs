using System;

namespace BookLovers.Bookcases.Application.WriteModels
{
    public class RemoveFromBookcaseWriteModel
    {
        public Guid BookcaseGuid { get; set; }

        public Guid BookGuid { get; set; }
    }
}