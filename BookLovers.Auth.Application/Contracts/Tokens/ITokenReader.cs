using System.Collections.Generic;
using System.Security.Claims;

namespace BookLovers.Auth.Application.Contracts.Tokens
{
    public interface ITokenReader
    {
        ITokenReader AddProtectedToken(string protectedToken);

        ITokenReader AddIssuer(string issuer);

        ITokenReader AddAudienceWithKey(string validAudience, string audienceKey);

        ITokenReader AddAudiencesWithKeys(IDictionary<string, string> dictionary);

        ITokenReader AddSigningKey(string secretKey);

        ITokenReader ValidateAudience();

        ITokenReader ValidateIssuer();

        ClaimsPrincipal ReadToken();
    }
}