using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Auth.Application.Commands.PasswordResets;
using BookLovers.Auth.Application.Commands.Registrations;
using BookLovers.Auth.Application.Commands.Tokens;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Application.WriteModels;
using BookLovers.Auth.Infrastructure.Queries;
using BookLovers.Auth.Infrastructure.Queries.Registrations;
using BookLovers.Auth.Infrastructure.Queries.Users;
using BookLovers.Auth.Infrastructure.Root;
using BookLovers.Base.Infrastructure;
using BookLovers.Filters;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class AuthController : ApiController
    {
        private readonly IModule<AuthModule> _module;

        public AuthController(IModule<AuthModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/auth/sign_up")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(SignUpWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new SignUpCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/auth/password")]
        [Authorize]
        [CheckCredentials]
        public async Task<IHttpActionResult> ChangePassword(
            ChangePasswordWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new ChangePasswordCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/auth/password/token")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GenerateChangePasswordToken(
            GenerateResetPasswordTokenWriteModel writeModel)
        {
            var command = new GenerateResetTokenPasswordCommand(writeModel);

            var validationResult = await _module.SendCommandAsync(command);

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/auth/password/reset")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ResetPassword(
            ResetPasswordWriteModel writeModel)
        {
            var validationResult =
                await _module.SendCommandAsync(new ResetPasswordCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/auth/email")]
        [Authorize]
        [CheckCredentials]
        public async Task<IHttpActionResult> ChangeEmail(
            ChangeEmailWriteModel writeModel)
        {
            var validationResult =
                await _module.SendCommandAsync(new ChangeEmailCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/auth/registration/{email}/{token}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CompleteRegistration(
            string email,
            string token)
        {
            var validationResult = await _module.SendCommandAsync(
                new CompleteRegistrationCommand(email, token));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/auth/token")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> RevokeToken(
            RevokeTokenWriteModel writeModel)
        {
            var validationResult =
                await _module.SendCommandAsync(new RevokeTokenCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/auth/user")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> BlockAccount(
            BlockAccountWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new BlockAccountCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpGet]
        [Route("api/auth/email/{email}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> IsEmailUnique(string email)
        {
            var queryResult = await _module.ExecuteQueryAsync<IsEmailUniqueQuery, bool>(new IsEmailUniqueQuery(email));

            return Ok(queryResult.Value);
        }

        [HttpGet]
        [Route("api/auth/username/{userName}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> IsUserNameUnique(string userName)
        {
            var queryResult = await _module.ExecuteQueryAsync<IsUserNameUniqueQuery, bool>(
                new IsUserNameUniqueQuery(userName));

            return Ok(queryResult.Value);
        }

        [HttpGet]
        [Route("api/auth/registration/{email}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetVerificationToken(string email)
        {
            var queryResult = await _module.ExecuteQueryAsync<GetRegistrationSummaryTokenQuery, string>(
                new GetRegistrationSummaryTokenQuery(email));

            return Ok(queryResult.Value);
        }

        [HttpGet]
        [Route("api/auth/password/token/{email}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetResetPasswordToken(string email)
        {
            var queryResult = await _module.ExecuteQueryAsync<GetResetPasswordTokenQuery, string>(
                new GetResetPasswordTokenQuery(email));

            return Ok(queryResult.Value);
        }
    }
}