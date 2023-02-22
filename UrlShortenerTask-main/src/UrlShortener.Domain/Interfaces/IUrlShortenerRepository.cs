using UrlShortener.Common.Requests;
using UrlShortener.Domain.Models;

namespace UrlShortener.Domain.Interfaces;

public interface IUrlShortenerRepository
{
    Task AddShortUrlAsync(CreateShortUrlRequest request);
    Task<IEnumerable<ShortUrl>?> GetShortUrlsAsync();
}