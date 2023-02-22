using System.Collections.Generic;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using UrlShortener.Common.Requests;
using UrlShortener.Domain.Interfaces;
using UrlShortener.WebApplication.Controllers.V1;
using Xunit;

namespace UrlShortener.Domain.Tests.Unit.Fixtures;

[Trait("Category", "Unit")]
public class UrlShortenerControllerTestsSetup : TheoryData
{
    public bool? EnableUrlShortenerRepositoryMock { get; set; } = true;
    public bool? EnableUrlShortenerRepositoryValidatorMock { get; set; } = true;
    public bool? EnableTempDataDictionary { get; set; } = true;

    public IEnumerable<object[]> GetSetup()
    {
        var loggerMock = new Mock<ILogger<UrlShortenerController>>();
        var createShortUrlRequestValidator = new Mock<IValidator<CreateShortUrlRequest>>();
        var urlShortenerRepositoryMock = new Mock<IUrlShortenerRepository>();
        var tempDataDictionary = new Mock<ITempDataDictionary>();

        var mockCollection = new List<object>();

        var urlShortenerController =
            new UrlShortenerController(
                loggerMock.Object,
                createShortUrlRequestValidator.Object,
                urlShortenerRepositoryMock.Object
            )
            {
                TempData = tempDataDictionary.Object
            };

        if (EnableUrlShortenerRepositoryMock is true) mockCollection.Add(urlShortenerRepositoryMock);

        if (EnableUrlShortenerRepositoryValidatorMock is true) mockCollection.Add(createShortUrlRequestValidator);

        if (EnableTempDataDictionary is true) mockCollection.Add(tempDataDictionary);

        mockCollection.Add(urlShortenerController);

        AddRow(mockCollection.ToArray());

        return this;
    }
}