using BookLovers.Base.Domain.Rules;

namespace BookLovers.Auth.Domain.PasswordResets
{
    internal class PasswordResetTokenCannotBeExpired : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Password reset token has expired.";

        private readonly PasswordResetToken _passwordResetToken;

        public PasswordResetTokenCannotBeExpired(
            PasswordResetToken passwordResetToken)
        {
            _passwordResetToken = passwordResetToken;
        }

        public bool IsFulfilled() => !_passwordResetToken.IsExpired();

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}