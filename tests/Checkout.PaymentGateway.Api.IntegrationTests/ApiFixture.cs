using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Checkout.PaymentGateway.Api.IntegrationTests;

public class ApiFixture : IDisposable
{
    public WebApplicationFactory<Program> Factory;

    public ApiFixture()
    {
        var configuration = GetIConfigurationRoot();

        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, conf) => { conf.AddConfiguration(configuration); });
        });
    }

    public void Dispose()
    {
        Factory.Dispose();
    }

    private static IConfigurationRoot GetIConfigurationRoot()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
    }
}