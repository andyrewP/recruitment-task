using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Common.Requests;

public record CreateShortUrlRequest
{
    [RegularExpression(
        @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$",
        ErrorMessage = "Incorrect URL - must be a valid format")]
    public string? LongUrl { get; set; }
    public string? Alias { get; set; }
}