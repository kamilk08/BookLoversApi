using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class AddedResourceCannotBeDuplicated : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Resource already added.";

        private readonly Reader _reader;
        private readonly IAddedResource _addedResource;

        public AddedResourceCannotBeDuplicated(Reader reader, IAddedResource addedResource)
        {
            _reader = reader;
            _addedResource = addedResource;
        }

        public bool IsFulfilled()
        {
            return !_reader.AddedResources.Any(a =>
                a.ResourceGuid == _addedResource.ResourceGuid
                && a.AddedAt == _addedResource.AddedAt);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}