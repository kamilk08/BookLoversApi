using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class SexReadModel : IReadModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}