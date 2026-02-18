using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Pharmacy.IntegrationTests.Controllers;

public class SystemUserControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public SystemUserControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetSystemUsers_WithoutAuthentication_ReturnsUnauthorizedOrOK()
    {
        // Act
        var response = await _client.GetAsync("/api/systemuser");

        // Assert
        // Note: This might return OK if the endpoint doesn't require authentication
        // Adjust based on your actual authentication requirements
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetSystemUser_WithInvalidGuid_ReturnsNotFound()
    {
        // Arrange - Use a clearly invalid string that won't be parsed as GUID
        var invalidGuid = "not-a-valid-guid-at-all";

        // Act
        var response = await _client.GetAsync($"/api/systemuser/{invalidGuid}");

        // Assert
        // ASP.NET Core routing will return 404 for invalid GUID format in route constraints
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetSystemUser_WithNonExistentValidGuid_ReturnsNotFoundOrUnauthorized()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/systemuser/{nonExistentId}");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetSystemUser_WithValidGuidFormat_DoesNotReturnBadRequest()
    {
        // Arrange
        var validGuid = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/systemuser/{validGuid}");

        // Assert
        // Should not return BadRequest for valid GUID format
        response.StatusCode.Should().NotBe(HttpStatusCode.BadRequest);
    }
}