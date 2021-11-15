using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Validation;
using FluentValidation;

namespace BookLovers.Base.Infrastructure
{
    public abstract class BaseValidationModule
    {
        private readonly ValidatorFactoryBase _validatorFactory;

        protected BaseValidationModule(ValidatorFactoryBase validatorFactory) =>
            _validatorFactory = validatorFactory;

        protected ICommandValidationResult ValidateCommand<TCommand>(
            TCommand command)
        {
            if (command == null)
                return CommandValidationResult.FailureResult(
                    new List<FluentValidationError>
                    {
                        new FluentValidationError(nameof(command), "Command cannot be null.")
                    });

            var validationResult = _validatorFactory.GetValidator<TCommand>()
                .Validate(command);

            var errors = validationResult.Errors.Select(err =>
                new FluentValidationError(err.PropertyName, err.ErrorMessage));

            return !validationResult.IsValid
                ? CommandValidationResult.FailureResult(errors)
                : CommandValidationResult.SuccessResult();
        }

        protected QueryResult<TQuery, TResult> ValidateQuery<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            if (query == null)
                return QueryResult<TQuery, TResult>.InValidQuery(
                    default,
                    new List<FluentValidationError>
                    {
                        new FluentValidationError(nameof(TQuery), "Query cannot be null.")
                    });

            IValidator validator;

            try
            {
                validator = _validatorFactory.GetValidator<TQuery>();
            }
            catch (Exception ex)
            {
                return QueryResult<TQuery, TResult>.ValidatedQuery(query);
            }

            if (validator == null)
                return QueryResult<TQuery, TResult>.ValidatedQuery(query);

            var validationResult = validator.Validate(query);

            var errors = validationResult.Errors.Select(err =>
                new FluentValidationError(err.PropertyName, err.ErrorMessage));

            return !validationResult.IsValid
                ? QueryResult<TQuery, TResult>.InValidQuery(query, errors)
                : QueryResult<TQuery, TResult>.ValidatedQuery(query);
        }
    }
}