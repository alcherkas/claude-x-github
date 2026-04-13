using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DemoApi.Tests;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetRoot_ReturnsSuccess()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetRoot_ReturnsExpectedJson()
    {
        var client = _factory.CreateClient();

        var response = await client.GetFromJsonAsync<RootResponse>("/");

        Assert.NotNull(response);
        Assert.Equal("Welcome to DemoApi", response.Message);
        Assert.Equal("1.0.0", response.Version);
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccess()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/weatherforecast");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsFiveItems()
    {
        var client = _factory.CreateClient();

        var forecasts = await client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        Assert.NotNull(forecasts);
        Assert.Equal(5, forecasts.Length);
    }

    [Fact]
    public async Task GetHealth_ReturnsHealthy()
    {
        var client = _factory.CreateClient();

        var response = await client.GetFromJsonAsync<HealthResponse>("/health");

        Assert.NotNull(response);
        Assert.Equal("healthy", response.Status);
    }

    [Fact]
    public async Task GetNonExistent_ReturnsNotFound()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/nonexistent");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private record RootResponse(string Message, string Version);
    private record HealthResponse(string Status);
}
