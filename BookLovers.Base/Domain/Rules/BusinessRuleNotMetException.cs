using System;

namespace BookLovers.Base.Domain.Rules
{
    public class BusinessRuleNotMetException : Exception
    {
        public BusinessRuleNotMetException(string message)
            : base(message)
        {
        }
    }
}