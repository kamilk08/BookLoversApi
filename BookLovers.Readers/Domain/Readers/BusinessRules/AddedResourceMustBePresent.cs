using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class AddedResourceMustBePresent : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Added resource is not available.";

        private readonly Reader _reader;
        private readonly IAddedResource _addedResource;

        public AddedResourceMustBePresent(Reader reader, IAddedResource addedResource)
        {
            _reader = reader;
            _addedResource = addedResource;
        }

        public bool IsFulfilled() => _reader.AddedResources.Contains(_addedResource);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}