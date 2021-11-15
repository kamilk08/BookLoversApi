using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Readers.Domain.Profiles;

namespace BookLovers.Readers.Mementos
{
    public interface ISocialProfileMemento : IMemento<Profile>, IMemento
    {
        string About { get; }

        DateTime JoinedAt { get; }

        string WebSite { get; }

        DateTime BirthDate { get; }

        string FirstName { get; }

        string SecondName { get; }

        int Sex { get; }

        string City { get; }

        string Country { get; }

        string CurrentRole { get; }

        Guid ReaderGuid { get; }

        IList<IFavourite> Favourites { get; }
    }
}