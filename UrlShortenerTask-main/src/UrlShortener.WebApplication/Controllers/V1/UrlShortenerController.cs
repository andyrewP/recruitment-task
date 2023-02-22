using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Common.Requests;
using UrlShortener.Domain.Interfaces;
using UrlShortener.Domain.Models;
using UrlShortener.WebApplication.Controllers.Shared;
using X.PagedList;

namespace UrlShortener.WebApplication.Controllers.V1;

public class UrlShortenerController : BaseApiController<CreateShortUrlRequest>
{
    private readonly IUrlShortenerRepository _urlShortenerRepository;

    public UrlShortenerController(ILogger<UrlShortenerController> logger, IValidator<CreateShortUrlRequest> validator,
        IUrlShortenerRepository urlShortenerRepository) : base(logger, validator)
    {
        _urlShortenerRepository =
            urlShortenerRepository ?? throw new ArgumentNullException(nameof(urlShortenerRepository));
    }

    /// <summary>
    /// Default view
    /// </summary>
    /// <returns>View with list of shortened urls.</returns>
    public async Task<IActionResult> Index(int? page)
    {
        try
        {
            var shortUrls
                = await _urlShortenerRepository.GetShortUrlsAsync() ?? new List<ShortUrl>();

            var model = await shortUrls.ToPagedListAsync(page ?? 1, 3);
            return View(model);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred: {Message}", ex.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    /// <summary>
    /// Default view
    /// </summary>
    /// <returns>Return default view from Create</returns>
    public IActionResult Create()
    {
        return View(new CreateShortUrlRequest());
    }

    /// <summary>
    /// Inserts a short url to the DB.
    /// </summary>
    /// <param name="request">Short url request payload.</param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateShortUrlRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                Logger.LogError("Invalid model state: {Errors}",
                    string.Join("; ", ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)));
                return RedirectToAction(nameof(Index));
            }

            var validationResponse =
                await Validator.ValidateAsync(request);

            if (!validationResponse.IsValid)
            {
                var errorMessages = 
                    string.Join(" ", validationResponse.Errors.Select(e => e.ErrorMessage));

                Logger.LogError(
                    $"Validation error in {nameof(Create)} -> " +
                    $"{validationResponse.Errors.Select(e => $"{e.PropertyName} {e.ErrorMessage}")}");
                TempData["ErrorMessage"] =
                    $"Template validation failed: {errorMessages}";
                return RedirectToAction("Create");
            }

            await _urlShortenerRepository.AddShortUrlAsync(request);
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = "Sorry, that alias is already taken.";
            Logger.LogError(ex, "An exception occurred: {Message}", ex.Message);
            return RedirectToAction("Create");
        }

        return RedirectToAction("Index", "UrlShortener");
    }
}