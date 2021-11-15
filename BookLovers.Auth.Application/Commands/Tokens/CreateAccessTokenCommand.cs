using System;
using BookLovers.Auth.Application.Contracts.Tokens;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Auth.Application.Commands.Tokens
{
    public class CreateAccessTokenCommand : ICommand
    {
        public Guid TokenGuid { get; }

        public AccessTokenProperties Dto { get; }

        public CreateAccessTokenCommand(AccessTokenProperties dto)
        {
            this.TokenGuid = Guid.NewGuid();
            this.Dto = dto;
        }
    }
}