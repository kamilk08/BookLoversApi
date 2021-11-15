using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Bookcases.Domain.Settings;

namespace BookLovers.Bookcases.Mementos
{
    public interface ISettingsManagerMemento : IMemento<SettingsManager>, IMemento
    {
        Guid BookcaseGuid { get; }

        List<IBookcaseOption> SelectedOptions { get; }
    }
}