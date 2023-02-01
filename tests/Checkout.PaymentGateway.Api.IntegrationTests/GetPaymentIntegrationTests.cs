using System.Net;
using System.Text;
using Checkout.PaymentGateway.Application.Features.Payment.Commands;
using Checkout.PaymentGateway.Application.Features.Payment.Queries;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Checkout.PaymentGateway.Api.IntegrationTests;

public class GetPaymentIntegrationTest : IClassFixture<ApiFixture>
{
    private readonly ApiFixture _fixture;
    private const string ApiPaymentsUrlPath = "/api/payments/";

    public GetPaymentIntegrationTest(ApiFixture fixture)
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
    public async Task Get_Payment_Correct_Data_Valid()
    {
        // Arrange
        var client = CreateAuthenticatedHttpClient();

        var paymentId = Guid.NewGuid();
        var paymentCommand = new CreatePaymentCommand
        {
            Id = paymentId,
            Amount = 50,
            MerchantId = "123456789",
            Currency = "USD",
            CardNumber = "6037997540752384",
            CvvCode = "665",
            ExpirationMonth = 12,
            ExpirationYear = 25,
            CardHolderName = "John Doe"
        };

     var res=   await client.PostAsync(ApiPaymentsUrlPath,
            new StringContent(JsonConvert.SerializeObject(paymentCommand), Encoding.UTF8, "application/json"));

        // Act
        var response = await client.GetAsync(ApiPaymentsUrlPath + paymentId);
        var json = await response.Content.ReadAsStringAsync();
        var paymentResponse = JsonConvert.DeserializeObject<GetPaymentQueryResult>(json);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        paymentResponse.Id.Should().Be(paymentId);
        paymentResponse.Amount.Should().Be(paymentCommand.Amount);
        paymentResponse.Currency.Should().Be(paymentCommand.Currency);
        paymentResponse.ExpirationYear.Should().Be(paymentCommand.ExpirationYear);
        paymentResponse.ExpirationMonth.Should().Be(paymentCommand.ExpirationMonth);
    }

    [Fact]
    public async Task Get_Payment_NotFound_PaymentId()
    {
        // Arrange
        var client = CreateAuthenticatedHttpClient();
        var paymentId = Guid.NewGuid();

        // Act
        var response = await client.GetAsync(ApiPaymentsUrlPath + paymentId);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}