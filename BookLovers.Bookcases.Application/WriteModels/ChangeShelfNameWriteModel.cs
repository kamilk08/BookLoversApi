using System;

namespace BookLovers.Bookcases.Application.WriteModels
{
    public class ChangeShelfNameWriteModel
    {
        public Guid BookcaseGuid { get; set; }

        public Guid ShelfGuid { get; set; }

        public string ShelfName { get; set; }
    }
}