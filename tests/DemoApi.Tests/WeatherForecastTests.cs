namespace DemoApi.Tests;

public class WeatherForecastTests
{
    [Fact]
    public void TemperatureF_ConvertsCorrectly()
    {
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 0, "Cold");
        Assert.Equal(32, forecast.TemperatureF);
    }

    [Theory]
    [InlineData(0, 32)]
    [InlineData(100, 211)]  // 32 + (int)(100 / 0.5556) = 32 + 179 = 211 (integer truncation)
    [InlineData(-40, -39)]  // 32 + (int)(-40 / 0.5556) = 32 + -71 = -39
    [InlineData(25, 76)]
    public void TemperatureF_VariousValues(int celsius, int expectedFahrenheit)
    {
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), celsius, "Test");
        Assert.Equal(expectedFahrenheit, forecast.TemperatureF);
    }

    [Fact]
    public void WeatherForecast_StoresProperties()
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var forecast = new WeatherForecast(date, 25, "Warm");

        Assert.Equal(date, forecast.Date);
        Assert.Equal(25, forecast.TemperatureC);
        Assert.Equal("Warm", forecast.Summary);
    }

    [Fact]
    public void WeatherForecast_AllowsNullSummary()
    {
        var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Now), 20, null);
        Assert.Null(forecast.Summary);
    }
}
