using FluentValidation;
using UrlShortener.Common.Requests;

namespace UrlShortener.WebApplication.Validators;

public class CreateShortUrlValidator : AbstractValidator<CreateShortUrlRequest>
{
    public CreateShortUrlValidator()
    {
        RuleFor(payLoad =>
                payLoad.LongUrl).Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(payLoad => !string.IsNullOrEmpty(payLoad.LongUrl)).NotEmpty();
    }
}