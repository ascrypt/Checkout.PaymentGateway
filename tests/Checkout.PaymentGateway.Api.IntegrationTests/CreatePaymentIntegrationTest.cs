using System.Net;
using System.Text;
using Checkout.PaymentGateway.Application.Features.Payment.Commands;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Checkout.PaymentGateway.Api.IntegrationTests;

public class CreatePaymentIntegrationTest : IClassFixture<ApiFixture>
{
    private readonly ApiFixture _fixture;
    private const string ApiPaymentsUrlPath = "/api/payments/";

    public CreatePaymentIntegrationTest(ApiFixture fixture)
    {
        _fixture = fixture;
    }
    
    private HttpClient CreateAuthenticatedHttpClient()
    {
        var client = _fixture.Factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        return client;
    }

    [Fact]
    public async Task Create_Payment_BadRequest_ExpirationDate_Expired()
    {
        // Arrange
        var client = CreateAuthenticatedHttpClient();

        var paymentId = Guid.NewGuid();

        // Act
        var paymentCommand = new CreatePaymentCommand
        {
            Id = paymentId,
            Amount = 50,
            Currency = "USD",
            CardNumber = "7592140428574371",
            MerchantId = "123456789",
            CvvCode = "765",
            ExpirationMonth = 12,
            ExpirationYear = 17,
            CardHolderName = "John Doe"
        };

        var payload = JsonConvert.SerializeObject(paymentCommand);
        var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(ApiPaymentsUrlPath, postContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Payment_BadRequest_Currency_Incorrect()
    {
        // Arrange
        var client = CreateAuthenticatedHttpClient();

        var paymentId = Guid.NewGuid();

        // Act
        var paymentCommand = new CreatePaymentCommand
        {
            Id = paymentId,
            Amount = 20,
            Currency = "USDT",
            CardNumber = "7592140428574371",
            MerchantId = "123456789",
            CvvCode = "665",
            ExpirationMonth = 12,
            ExpirationYear = 25,
            CardHolderName = "John Doe"
        };

        var payload = JsonConvert.SerializeObject(paymentCommand);
        var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(ApiPaymentsUrlPath, postContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Payment_BadRequest_Amount_InValid()
    {
        // Arrange
        var client = CreateAuthenticatedHttpClient();

        var paymentId = Guid.NewGuid();

        // Act
        var paymentCommand = new CreatePaymentCommand
        {
            Id = paymentId,
            Amount = -5,
            Currency = "USD",
            CardNumber = "7592140428574371",
            CvvCode = "665",
            MerchantId = "123456789",
            ExpirationMonth = 12,
            ExpirationYear = 25,
            CardHolderName = "John Doe"
        };

        var payload = JsonConvert.SerializeObject(paymentCommand);
        var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(ApiPaymentsUrlPath, postContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Payment_BadRequest_CardNumber_InValid()
    {
        // Arrange
        var client = CreateAuthenticatedHttpClient();

        var paymentId = Guid.NewGuid();

        // Act
        var paymentCommand = new CreatePaymentCommand
        {
            Id = paymentId,
            Amount = 50,
            Currency = "USD",
            CardNumber = "379212",
            CvvCode = "665",
            MerchantId = "123456789",
            ExpirationMonth = 12,
            ExpirationYear = 25,
            CardHolderName = "John Doe"
        };

        var payload = JsonConvert.SerializeObject(paymentCommand);
        var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var result = await client.PostAsync(ApiPaymentsUrlPath, postContent);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Create_Payment_Created_Valid()
    {
        // Arrange
        var client = CreateAuthenticatedHttpClient();

        var paymentId = Guid.NewGuid();

        // Act
        var paymentCommand = new CreatePaymentCommand
        {
            Id = paymentId,
            Amount = 500,
            Currency = "JPY",
            CardNumber = "6037997540752384",
            CvvCode = "665",
            MerchantId = "123456789",
            ExpirationMonth = 02,
            ExpirationYear = 28,
            CardHolderName = "John Doe"
        };

        var payload = JsonConvert.SerializeObject(paymentCommand);
        var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var result = await client.PostAsync(ApiPaymentsUrlPath, postContent);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task Create_Payment_BadRequest_CVVCODE_InValid()
    {
        // Arrange
        var client = CreateAuthenticatedHttpClient();

        var paymentId = Guid.NewGuid();

        // Act
        var paymentCommand = new CreatePaymentCommand
        {
            Id = paymentId,
            Amount = 50,
            Currency = "USD",
            MerchantId = "123456789",
            CardNumber = "7592140428574371",
            CvvCode = "66556",
            ExpirationMonth = 12,
            ExpirationYear = 25,
            CardHolderName = "John Doe"
        };

        var payload = JsonConvert.SerializeObject(paymentCommand);
        var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var result = await client.PostAsync(ApiPaymentsUrlPath, postContent);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Payment_BadRequest_ExpirationDate_InValid()
    {
        // Arrange
        var client = CreateAuthenticatedHttpClient();

        var paymentId = Guid.NewGuid();

        // Act
        var paymentCommand = new CreatePaymentCommand
        {
            Id = paymentId,

            Amount = 50,
            Currency = "USD",
            CardNumber = "7592140428574371",
            MerchantId = "123456789",
            CvvCode = "615",
            ExpirationMonth = 15,
            ExpirationYear = 45,
            CardHolderName = "John Doe"
        };

        var payload = JsonConvert.SerializeObject(paymentCommand);
        var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var result = await client.PostAsync(ApiPaymentsUrlPath, postContent);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Payment_BadRequest_Existing_Payment()
    {
        // Arrange
        var client = CreateAuthenticatedHttpClient();

        var paymentId = Guid.NewGuid();

        // Act
        var paymentCommand = new CreatePaymentCommand
        {
            Id = paymentId,
            Amount = 50,
            Currency = "USD",
            CardNumber = "7592140428574371",
            CvvCode = "665",
            MerchantId = "123456789",
            ExpirationMonth = 12,
            ExpirationYear = 25,
            CardHolderName = "John Doe"
        };

        var payload = JsonConvert.SerializeObject(paymentCommand);
        var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
        
        await client.PostAsync(ApiPaymentsUrlPath, postContent);
        var secondAttemptResult = await client.PostAsync(ApiPaymentsUrlPath, postContent);

        // Assert
        secondAttemptResult.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}