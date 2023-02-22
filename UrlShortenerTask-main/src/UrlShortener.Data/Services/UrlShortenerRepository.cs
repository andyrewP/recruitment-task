using System.Text;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Common.Requests;
using UrlShortener.Data.Data;
using UrlShortener.Domain.Interfaces;
using UrlShortener.Domain.Literals;
using UrlShortener.Domain.Models;

namespace UrlShortener.Data.Services;

public class UrlShortenerRepository : IUrlShortenerRepository
{
    private readonly DataContext _context;
    private const int DefaultShortCodeLength = 8;

    public UrlShortenerRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddShortUrlAsync(CreateShortUrlRequest request)
    {
        request.Alias ??= GetShortCode(DefaultShortCodeLength);

        if (await IsAliasClaimed(request.Alias))
            throw new InvalidOperationException(nameof(request.Alias));

        var shortUrl = new ShortUrl
        {
            CreatedTimeStamp = DateTime.Now,
            LongUrl = request.LongUrl,
            Alias = request.Alias,
            ShortedUrl = $"{Literals.ApplicationSetupConstants.ServiceUrl}?u={request.Alias}"
        };
        await _context.AddAsync(shortUrl);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ShortUrl>?> GetShortUrlsAsync()
    {
        return await _context.ShortUrls
            .OrderBy(s => s.CreatedTimeStamp).ToListAsync();
    }

    private static string GetShortCode(int maxShortCodeLength)
    {
        var shortCodeRandomNumber = new Random((int)DateTime.Now.Ticks);
        var stringBuilder = new StringBuilder();

        Enumerable.Range(1, maxShortCodeLength).ToList().ForEach(_ =>
        {
            stringBuilder.Append(
                Convert.ToChar(
                    Convert.ToInt32(Math.Floor(26 * shortCodeRandomNumber.NextDouble() + 65)
                    )));
        });

        return stringBuilder.ToString();
    }

    private async Task<bool> IsAliasClaimed(string alias) => await _context.ShortUrls.AnyAsync(s => s.Alias == alias);
}