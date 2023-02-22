using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Domain.Models;

public record ShortUrl
{
    public int Id { get; set; }
    public DateTime CreatedTimeStamp { get; set; }
    public string? LongUrl { get; set; }
    public string? ShortedUrl { get; set; }
    public string? Alias { get; set; }
}