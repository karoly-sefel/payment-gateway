# Testing the Payment Gateway

## Quick start

- Go to the project root
- Run `docker-compose up`
- Open `http://localhost:5000/swagger`
- Click the `Authorize` button
- Copy and paste value to textbox and click `Authorize`
  ```text
  Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ikthcm9seSIsInN1YiI6Ikthcm9seSIsImp0aSI6ImQ5ZTFjZWI2Iiwic2NvcGUiOlsicGF5bWVudDpyZWFkIiwicGF5bWVudDpwcm9jZXNzIl0sIm1lcmNoYW50SWQiOiJtZXJjaGFudEEiLCJhdWQiOlsiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSJdLCJuYmYiOjE2OTM5MTE0NjUsImV4cCI6MTg5MzQ1NjAwMCwiaWF0IjoxNjkzOTExNDY2LCJpc3MiOiJkb3RuZXQtdXNlci1qd3RzIn0.x1BJ5EAzumJmY5jtqgUJxYvda_kgerQrDbptN3wUPHg
  ```
  For more access tokens and further details see the [Authorization](./authorization.md) page
- Select the first endpoint on the Swagger page
- Click `Try out` and enter `abc1234`
- You should see a success response
- Select the other endpoint on the Swagger page
- Click `Try out`, modify the request body as desired then press `Execute`
- You should see a success response when using the provided example payload

## Bank Simulator - supported scenarios


| Scenario                     | Card details                                           |
|------------------------------|--------------------------------------------------------|
| InsufficientFunds            | depends on currency and amount used, see section below |
| ExpiredCard                  | any card number with year and month in the past        |
| SecurityViolation            | use card number: 4556253752712245                      |
| LostCardPickUp               | use card number: 4941202060999329                      |
| AuthenticationRequired3DS    | use card number: 4500622868341387                      |
| ActivityAmountLimitExceeded  | use card number: 4556294593757189                      |
| Success                      | all other card numbers with valid expiry and amount    |


### InsufficientFunds error

The InsufficientFunds validation error is triggered for these currencies and an amount submitted
above the threshold.

| Currency | Threshold  | Amount (and above) |
|----------|------------|--------------------|
| DKK      | 100 DKK    | 10001              |
| EUR      | 1,0000 EUR | 100001             |
| EUR      | 10,000 GBP | 1000001            |

## Supported currencies

Only the following currency codes are accepted by the API

- EUR
- GBP
- USD
- DKK
- JPY
