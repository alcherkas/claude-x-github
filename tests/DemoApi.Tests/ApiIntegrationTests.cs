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
    public async Task GetHello_ReturnsGreeting()
    {
        var client = _factory.CreateClient();

        var response = await client.GetFromJsonAsync<HelloResponse>("/hello/World");

        Assert.NotNull(response);
        Assert.Equal("Hello, World!", response.Greeting);
    }

    [Fact]
    public async Task GetTime_ReturnsTimeInfo()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/time");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<TimeResponse>();
        Assert.NotNull(content);
        Assert.False(string.IsNullOrEmpty(content.Timezone));
    }

    [Fact]
    public async Task GetEcho_ReturnsReversedMessage()
    {
        var client = _factory.CreateClient();

        var response = await client.GetFromJsonAsync<EchoResponse>("/echo?message=hello");

        Assert.NotNull(response);
        Assert.Equal("hello", response.Original);
        Assert.Equal("olleh", response.Reversed);
        Assert.Equal(5, response.Length);
    }

    [Fact]
    public async Task GetEcho_WithoutMessage_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/echo");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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
    private record HelloResponse(string Greeting);
    private record TimeResponse(DateTime Utc, DateTime Local, string Timezone);
    private record EchoResponse(string Original, string Reversed, int Length);
}
