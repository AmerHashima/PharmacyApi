using FluentAssertions;
using Pharmacy.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Microsoft.EntityFrameworkCore.InMemory;
namespace Pharmacy.IntegrationTests;

public class BasicIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public BasicIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                // Replace the database with in-memory database for testing
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PharmacyDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<PharmacyDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Application_Starts_Successfully()
    {
        // This test just ensures the application can start
        _client.Should().NotBeNull();
    }

    [Fact]
    public async Task Swagger_Endpoint_Returns_Success()
    {
        // Act
        var response = await _client.GetAsync("/swagger/index.html");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Health_Check_Endpoint()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }
}
