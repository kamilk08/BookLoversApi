using System;

namespace BookLovers.Bookcases.Application.WriteModels
{
    public class RemoveShelfWriteModel
    {
        public Guid BookcaseGuid { get; set; }

        public Guid ShelfGuid { get; set; }
    }
}