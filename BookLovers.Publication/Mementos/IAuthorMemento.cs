using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.Authors;

namespace BookLovers.Publication.Mementos
{
    public interface IAuthorMemento : IMemento<Author>, IMemento
    {
        string FirstName { get; }

        string SecondName { get; }

        DateTime? BirthDate { get; }

        DateTime? DeathDate { get; }

        int Sex { get; }

        string BirthPlace { get; }

        string AboutAuthor { get; }

        string AuthorWebsite { get; }

        string DescriptionSource { get; }

        IEnumerable<Guid> AuthorBooks { get; }

        IEnumerable<Guid> Followers { get; }

        IEnumerable<Guid> Quotes { get; }

        IEnumerable<int> Genres { get; }
    }
}