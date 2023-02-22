using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using UrlShortener.Common.Requests;
using UrlShortener.Domain.Interfaces;
using UrlShortener.Domain.Tests.Unit.Fixtures;
using UrlShortener.WebApplication.Controllers.V1;
using Xunit;

namespace UrlShortener.Domain.Tests.Unit.Controller.V1;

public class UrlShortenerControllerTests
{
    public static IEnumerable<object[]> GetUrlShortenerControllerSetup(
        bool enableUrlShortenerRepositoryMock, bool enableUrlShortenerRepositoryValidatorMock,
        bool enableTempDataDictionaryMock)
    {
        return new UrlShortenerControllerTestsSetup
        {
            EnableUrlShortenerRepositoryMock = enableUrlShortenerRepositoryMock,
            EnableUrlShortenerRepositoryValidatorMock = enableUrlShortenerRepositoryValidatorMock,
            EnableTempDataDictionary = enableTempDataDictionaryMock
        }.GetSetup();
    }

    [Theory]
    [MemberData(nameof(GetUrlShortenerControllerSetup), true, true, true)]
    public async Task Create_UrlShortener_AliasAlreadyExist_ShouldReturnInvalidOperationException_TestAsync(
        Mock<IUrlShortenerRepository> urlShortenerRepositoryMock,
        Mock<IValidator<CreateShortUrlRequest>> createShortUrlRequestValidator, Mock<ITempDataDictionary> tempDataMock,
        UrlShortenerController urlShortenerController)
    {
        urlShortenerRepositoryMock.Setup(_ =>
                _.AddShortUrlAsync(It.IsAny<CreateShortUrlRequest>()))
            .ThrowsAsync(new InvalidOperationException());

        createShortUrlRequestValidator
            .Setup(_ => _.ValidateAsync(It.IsAny<CreateShortUrlRequest>(), default))
            .ReturnsAsync(new ValidationResult());

        var result = await urlShortenerController.Create(It.IsAny<CreateShortUrlRequest>());

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Create", ((RedirectToActionResult)result).ActionName);
        tempDataMock.VerifySet(x => x["ErrorMessage"] = "Sorry, that alias is already taken.");

        urlShortenerRepositoryMock
            .Verify(
                _ => _.AddShortUrlAsync(It.IsAny<CreateShortUrlRequest>()), Times.Once());
    }

    [Theory]
    [MemberData(nameof(GetUrlShortenerControllerConstructorParameterTestFeed))]
    public void UrlShortenerControllerConstructor_UseDefaultsForArguments_ShouldThrowNullException(
        ILogger<UrlShortenerController> logger, IValidator<CreateShortUrlRequest> createShortUrlRequestValidator,
        IUrlShortenerRepository urlShortenerRepositoryMock
    )
    {
        Assert.Throws<ArgumentNullException>(() =>
            new UrlShortenerController(logger, createShortUrlRequestValidator,
                urlShortenerRepositoryMock));
    }

    public static IEnumerable<object[]> GetUrlShortenerControllerConstructorParameterTestFeed()
    {
        var loggerMock = Mock.Of<ILogger<UrlShortenerController>>();
        var urlShortenerRepositoryMock
            = Mock.Of<IUrlShortenerRepository>();
        var createShortUrlRequestValidatorMock = Mock.Of<IValidator<CreateShortUrlRequest>>();

        yield return new object[]
        {
            default!, createShortUrlRequestValidatorMock, urlShortenerRepositoryMock
        };
        yield return new object[]
        {
            loggerMock, default!, urlShortenerRepositoryMock
        };
        yield return new object[]
        {
            loggerMock, createShortUrlRequestValidatorMock, default!
        };
    }
}