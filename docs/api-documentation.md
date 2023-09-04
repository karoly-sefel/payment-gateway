# Payment Gateway API Documentation

This page documents the public API endpoints to process payments and retrieve payment details.

Executable request examples can be found in [payment-gateway.http](../tools/payment-gateway.http)

## Retrieve Payment Details

Retrieve details of a previously made payment by providing the payment ID.

- **URL:** `/v1/payments/{paymentId}`
- **Method:** `GET`

### Request

- **Parameters:**
    - `paymentId` (string, required) - The unique identifier of the payment to retrieve.

### Response

- **Status Codes:**
    - `200 OK` - Successful retrieval. Returns payment details.
    - `404 Not Found` - Payment with the provided ID does not exist.
    - `400 Bad Request` - Invalid request format.

#### Successful Response (200 OK)

```json
{
  "paymentId": "1234567890",
  "maskedCardNumber": "**** **** **** 1234",
}
```

#### Not Found Response (404 Not Found)

```json
{
    "type": "https://httpstatuses.com/404",
    "title": "Payment not found",
    "status": 404,
    "detail": "No payment record can be found with the given id",
    "instance": "/v1/payments/123123",
    "paymentId": "123123"
}
```

#### Bad Request Response (400 Bad Request)

```json
{
    "type": "https://httpstatuses.com/400",
    "title": "Bad Request",
    "status": 400,
    "detail": "Invalid payment id",
    "instance": "/v1/payments/1-1-1-1",
    "paymentId": "1-1-1-1"
}
```

## Process Payment

Process a payment by providing payment details.

- **URL:** `/v1/payments`
- **Method:** `GET`

### Request

- **Headers:**
  - `Content-Type: application/json`

- **Body:** JSON object representing payment details.

#### Example Request Body:

```json
{
  "cardNumber": "1234 5678 9012 3456",
  "expiryMonth": "12",
  "expiryYear": "25",
  "amount": 100.00,
  "currency": "USD",
  "cvv": "123"
}
```

### Response

- **Status Codes:**
    - `201 Created` - Payment processing successful. Returns payment ID.
    - `400 Bad Request` - Invalid request format.
    - `500 Internal Server Error` - Unexpected server error.


#### Successful Response (200 OK)

```json
{
    "paymentId": "1234567890"
}
```

#### Bad Request Response (400 Bad Request)

```json
{
    "type": "https://httpstatuses.com/400",
    "title": "Bad Request",
    "status": 400,
    "detail": "Invalid request format. Please provide valid payment details."
}
```

#### Internal Server Error Response (500 Internal Server Error)

```json
{
    "type": "https://httpstatuses.com/500",
    "title": "Internal Server Error",
    "status": 500,
    "detail": "An unexpected error occurred during payment processing."
}
```
