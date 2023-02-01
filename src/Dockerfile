FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

ARG BUILDCONFIG=Release
ARG VERSION=1.0.0

WORKDIR /src
COPY . .
RUN dotnet publish Checkout.PaymentGateway.Api/Checkout.PaymentGateway.Api.csproj -c $BUILDCONFIG -o /app /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "Checkout.PaymentGateway.Api.dll"]