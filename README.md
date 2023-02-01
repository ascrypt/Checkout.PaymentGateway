# Checkout Payment Gateway Challenge
A RestAPI that allows a merchant to process a payment through a payment gateway and retrieve details of a previously made payment.
# Deliverables
1. Build an API that allows a merchant:
 -  a. To process a payment through your payment gateway.
 -  b. To retrieve details of a previously made payment.
2. Build a bank simulator to test your payment gateway API.

# Assumptions
1. MerchantId and PaymentId are coming from the Merchant
2. MerchantId considered to be valid if it is not empty and has min 5 and max 50 character length
3. Bank payment processing takes 10-15 seconds (simulated)
4. Rate limiting is applied to the API by default configuration of : Only allow 1 request every 3 seconds and only queue 3 requests when we go over that limit (configuration can be change in program.cs)

# Technologies and techniques used:

`C#`
`.Net 7`
`Clean Architecture`
`MediatR`
`CQRS`
`Rate limiting`
`Serilog`
`XUNIT`
`FluentValidation`
`Unit Tests`
`Integration Tests`
`InMemory Database`
`Docker`
`Swagger`
`Postman`

# Project Architecture

Clean Architecture used for this project to separate concerns and make the code more testable and maintainable.
## Command Query Responsibility Segregation Pattern (CQRS)

CQRS used to separate the responsibilities and separate the read and write intention processes. 

# Running the application

## Pre-requisites

- .NET Core 7.0 SDK (runtime for only running the application)

### Running the application using IDE
- Run the app through the visual studio or the Rider and make `CheckOut.PaymentGateway.Api` as the default project. Default port for the application is `7115` and startup launch url will be `https://localhost:7115/swagger/index.html`

### Running the application using Docker
Go to src folder and run the following commands:
```
docker-compose build
docker-compose up
```
Default port for the application is `2600` and swagger will be available on `https://localhost:2600/swagger/index.html`

### Using Postman
Postman collection file in the solution is available and the file name is : `Payment Gateway.postman_collection.json` and can be imported to Postman to test the endpoints.

# Project Dependencies diagram
![img](http://i.imgur.com/yB7GEyT.png)


# API endpoints sample requests and responses

### POST /api/payments 
Create a payment request

### Curl
```
curl -X 'POST' \
'https://localhost:7115/api/payments' \
-H 'accept: application/json' \
-H 'X-Api-Key: pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp' \
-H 'Content-Type: application/json' \
-d '{
"id": "3fa85f64-5717-4562-b3fc-2c963f66afa2",
"cardHolderName": "John Doe",
"cardNumber": "6037997540752384",
"merchantId": "asd232",
"cvvCode": "221",
"amount": 110,
"currency": "USD",
"expirationMonth": 10,
"expirationYear": 2030
}'
```
**Sample Json Body**:

```json
{
   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa2",
   "cardHolderName": "John Doe",
   "cardNumber": "6037997540752384",
   "merchantId": "asd232",
   "cvvCode": "221",
   "amount": 110,
   "currency": "USD",
   "expirationMonth": 10,
   "expirationYear": 2030
}
```

**Sample Response**:

**Success (201)**:

```json
{
  "paymentId": "3fa85f64-5717-4562-b3fc-2c963f66afa2",
  "resultCode": 0,
  "result": "Processing"
}
```

**Bad Request (400)**:

```json
{
  "paymentId": "00000000-0000-0000-0000-000000000000",
  "resultCode": 1,
  "result": "Invalid Request"
}
```

**Too many requests (429)**:

This is occurs when you are being rate limited based on your client.

### GET /Payment/{id}

### Curl
Endpoint to get previous transaction details

```
curl -X 'GET' \
'https://localhost:7115/api/payments/3fa85f64-5717-4562-b3fc-2c963f66afa2' \
-H 'accept: application/json' \
-H 'X-Api-Key: pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp'
```



**Sample Response**:

**Success (200)**:

```json
{
  "paymentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Processing",
  "isApproved": false,
  "card": {
    "cardNumber": "**** **** **** 2230",
    "expiryMonth": 6,
    "expiryYear": 22
  },
  "amount": {
    "currency": "USD",
    "amount": 50.00
  },
  "description": "Payment to amazon"
}
```

**Not Found (404)**:
There is no payment with the given id

**Too many requests (429)**:

This is occurs when you are being rate limited based on your client.

# Bonus points
## Authentication
`X-Api-Key` should be added to the header of the requests. The key is defined in the `appsettings.json` file.

The key is :

`pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp`

To use this in Swagger, simply click the "Authorize" button and insert it before calling endpoints.

## API Client Rate Limiting
I have implemented client based rate limiting into the app using the `AspNetCoreRateLimiting` package and used the Api Keys as clients.

## Logging
I have added `SeriLog` for structured file logging.
The settings can be changed through the appsettings file.

By default the log files can be found in the /Logs folder

## Containerization (Docker)

The project contains containerization support using Docker. 

Run `docker-compose build` and `docker-compose up` to start the gateway.

# Improvements
#### everything is implemented basically, but there are many improvements that can be done:
- Implement persistence database for storing payment details and merchant details
- Add distributed locking for payment creation protection using something like Redis
- Add rate limiting data to in-memory db like Redis
- Add a message broker like RabbitMQ or Kafka for asynchronous processing of payment requests
- Add resilience policies for accessing bank external endpoints
- Add cloud based logging
- Add better Authentication and Authorization using IdentityServer or cloud based authentication
- Add more unit tests and integration tests
