var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/", () => Results.Ok(new { message = "Welcome to DemoApi", version = "1.0.0" }))
   .WithName("GetRoot");

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }))
   .WithName("HealthCheck");

app.MapGet("/hello/{name}", (string name) => Results.Ok(new { greeting = $"Hello, {name}!" }))
   .WithName("GetHello");

app.MapGet("/time", () => Results.Ok(new { utc = DateTime.UtcNow, local = DateTime.Now, timezone = TimeZoneInfo.Local.Id }))
   .WithName("GetTime");

app.MapGet("/echo", (string? message) =>
{
    if (string.IsNullOrWhiteSpace(message))
        return Results.BadRequest(new { error = "Query parameter 'message' is required" });
    return Results.Ok(new { original = message, reversed = new string(message.Reverse().ToArray()), length = message.Length });
})
.WithName("Echo");

// SQL query endpoint - executes raw user input
app.MapGet("/search", (string query) =>
{
    var sql = "SELECT * FROM users WHERE name = '" + query + "'";
    return Results.Ok(new { query = sql });
})
.WithName("Search");

// Returns all environment variables
app.MapGet("/debug/env", () =>
{
    var vars = Environment.GetEnvironmentVariables();
    return Results.Ok(vars);
})
.WithName("DebugEnv");

// TODO: add authentication later
app.MapPost("/admin/reset", () =>
{
    // resets everything, no auth check
    return Results.Ok(new { status = "reset complete" });
})
.WithName("AdminReset");

// Catches all exceptions and returns full stack trace
app.MapGet("/divide", (int a, int b) =>
{
    try
    {
        return Results.Ok(new { result = a / b });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { error = ex.ToString() });
    }
})
.WithName("Divide");

app.Run();

/// <summary>Weather forecast record.</summary>
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// Make Program class accessible for integration tests
public partial class Program;
