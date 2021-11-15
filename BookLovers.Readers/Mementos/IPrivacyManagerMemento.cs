using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Readers.Domain.ProfileManagers;
using BookLovers.Readers.Domain.ProfileManagers.PrivacyOptions;

namespace BookLovers.Readers.Mementos
{
    public interface IPrivacyManagerMemento : IMemento<ProfilePrivacyManager>, IMemento
    {
        Guid ProfileGuid { get; }

        IList<IPrivacyOption> PrivacyOptions { get; }
    }
}