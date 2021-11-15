using System;

namespace BookLovers.Bookcases.Application.WriteModels
{
    public class AddShelfWriteModel
    {
        public Guid ShelfGuid { get; set; }

        public Guid BookcaseGuid { get; set; }

        public string ShelfName { get; set; }

        public int ShelfId { get; set; }
    }
}