using System;

namespace BookLovers.Base.Infrastructure.Commands
{
    public interface IInternalCommand
    {
        Guid Guid { get; }
    }
}