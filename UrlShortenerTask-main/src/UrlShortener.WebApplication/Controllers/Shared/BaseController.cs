using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.WebApplication.Controllers.Shared;

public abstract class BaseApiController<T> : Controller
{
    /// <summary>
    ///     <see cref="ILogger"/> logging
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    ///     Validator for fluent validation
    /// </summary>
    protected readonly IValidator<T> Validator;

    /// <summary>
    ///     Base controller constructor
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> logging service</param>
    /// <param name="validator">fluent validation for generic model</param>
    protected BaseApiController(ILogger logger, IValidator<T> validator)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }
}