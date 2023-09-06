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
    "paymentId": "abc1234",
    "maskedCardNumber": "************1111",
    "amount": 50000,
    "currency": "GBP",
    "status": "Success"
}
```

#### Success response payload

| Field            | Description                                                | Values                      |
|------------------|------------------------------------------------------------|-----------------------------|
| paymentId        | transaction identifier                                     | string                      |
| maskedCardNumber | card number with the last 4 digits                         | string                      |
| amount           | payment amount in the smalles denomination of the currency | integer                     |
| currency         | 3 letter currency code                                     | string                      |
| status           | current status of the transaction                          | Approved, Declined, Pending |

#### Error response payload

**ProblemDetails object**

| Field     | Description                      | Values  |
|-----------|----------------------------------|---------|
| type      | link the the error description   | URI     |
| title     | error category title             | string  |
| detail    | specific details about the error | integer |
| instance  | URL path to identify the request | string  |
| paymentId | transaction identifier           | string  |


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
- **Method:** `POST`

### Request

- **Headers:**
  - `Content-Type: application/json`

- **Body:** JSON object representing payment details.

#### Example Request Body:

```json
{
    "cardNumber": "4242424242421234",
    "expiryMonth": 8,
    "expiryYear": 2025,
    "amount": 102400,
    "currency": "GBP",
    "cvv": "123"
}
```

| Field       | Description                                                               | Values                                  |
|-------------|---------------------------------------------------------------------------|-----------------------------------------|
| cardNumber  | payment card number                                                       | string, 8-19 digits, may include spaces |
| expiryMonth | card expiry month                                                         | integer, 1-12                           |
| expiryYear  | card expiry year                                                          | integer, 4 digits                       |
| amount      | payment amount in the currencies smallest denomination, i.e. 1 GBP -> 100 | integer                                 |
| currency    | 3 letter currency code                                                    | string                                  |
| cvv         | 3 digit CVV code                                                          | string, 3 digits                        |

### Response

- **Status Codes:**
    - `201 Created` - Payment processing successful.
    - `202 Accepted` - Payment processing pending.
    - `400 Bad Request` - Invalid request format.
    - `500 Internal Server Error` - Unexpected server error.


#### Successful Response (201 Created)

```json
{
    "paymentId": "mLVu3_dPLhM8",
    "status": "Success",
    "statusDescription": "Processed"
}
```
#### Further processing required (202 Accepted)


```json
{
    "paymentId": "mLVu3_dPLhM8",
    "status": "Pending",
    "statusDescription": "AuthenticationRequired3DS"
}
```


#### Success response payload

| Field             | Description            | Values                               |
|-------------------|------------------------|--------------------------------------|
| paymentId         | transaction identifier | string                               |
| status            | transaction status     | Success, Pending                     |
| statusDescription | error category title   | Processed, AuthenticationRequired3DS |

#### Bad Request Response (400 Bad Request)

```json
{
    "type": "https://httpstatuses.com/400",
    "title": "Bad Request",
    "status": 400,
    "detail": "Invalid request format. Please provide valid payment details."
}
```

#### Unauthorized (401 Unauthorized)

```json
{
    "type": "https://httpstatuses.com/401",
    "title": "Unauthorized",
    "status": 401
}
```

#### Payment error (422 Unprocessable Entity)

```json
{
    "type": "https://httpstatuses.com/422",
    "title": "Payment error",
    "status": 422,
    "detail": "Transaction failed",
    "instance": "/v1/payments",
    "errorCode": "ActivityAmountLimitExceeded",
    "paymentId": "9UNSBoMDsHEh"
}
```

**ProblemDetails object**

| Field     | Description                      | Values          |
|-----------|----------------------------------|-----------------|
| type      | link the the error description   | URI             |
| title     | error category title             | string          |
| status    | response status code             | integer         |
| detail    | specific details about the error | integer         |
| instance  | URL path to identify the request | string          |
| paymentId | transaction identifier           | string          |
| errorCode | transaction identifier           | see table below |

| ErrorCode                   | Description                                                |
|-----------------------------|------------------------------------------------------------|
| InsufficientFunds           |                                                            |
| ExpiredCard                 | card expiry date in the past                               |
| MerchantNotFound            | merchant with the merchantId claim not found in the system |
| MerchantAccountClosed       | merchant account is no longer active                       |
| InvalidOrMissingCardDetails | error from Acquiring Bank                                  |
| NotSupportedCurrency        | see supported currencies on [Testing](testing.md) page     |
| SecurityViolation           | error from Acquiring Bank                                  |
| LostCardPickUp              | error from Acquiring Bank                                  |
| AuthenticationRequired3DS   | further processing required, 3DSecre                       |
| ActivityAmountLimitExceeded | error from Acquiring Bank                                  |


#### Internal Server Error Response (500 Internal Server Error)

```json
{
    "type": "https://httpstatuses.com/500",
    "title": "Internal Server Error",
    "status": 500,
    "detail": "An unexpected error occurred during payment processing."
}
```

## Authentication

API calls require a JWT Bearer token that needs to be included in calls to the endpoints above.

The acquired token should be included as an HTTP request header in the following format with the `JWT_TOKEN` placeholder 
replaced with the actual token.

`Authorization: Bearer JWT_TOKEN`

Read [Authorization](authorization.md) for more details and see examples in [payment-gateway.http](../tools/payment-gateway.http)

