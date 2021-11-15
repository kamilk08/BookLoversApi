using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Ninject.Activation;

namespace BookLovers.Photos.Models.Services
{
    public class FilePathProvider : IProvider<IDictionary<ProviderType, IFilePathFactory>>, IProvider
    {
        public Type Type => typeof(IDictionary<ProviderType, IFilePathFactory>);

        public object Create(IContext context)
        {
            var f= context.Kernel
                .GetAll<IFilePathFactory>()
                .ToList()
                .ToDictionary(k => k.ProviderType, v => v);

            return f;
        }
    }
}