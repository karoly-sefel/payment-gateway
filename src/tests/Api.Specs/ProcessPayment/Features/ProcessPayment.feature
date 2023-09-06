Feature: Process Payment
    As a merchant
    I want to be able to take card payments from my customers
    So that I can sell my products and services

Scenario: Successful payment with a payment id returned
    Given a merchant
    And a customer
    And the customer has sufficient balance on their credit card for the payment
    When a request is made to the ProcessPayment endpoint
    Then the status code is Created
    And the response includes the payment id for the current transaction with a Processed status

Scenario: Payment rejected by the acquiring bank
    Given a merchant
    And a customer
    And the customer has insufficient balance on their credit card for the payment
    When a request is made to the ProcessPayment endpoint
    Then the status code is UnprocessableEntity

Scenario: Unauthorized request
    Given a merchant
    When a request is made to the ProcessPayment endpoint without a bearer token
    Then the status code is Unauthorized

Scenario: Customer tries to pay with a stolen credit card
    Given a merchant
    And a customer
    And the customer uses a stolen credit card for the payment
    When a request is made to the ProcessPayment endpoint
    Then the status code is UnprocessableEntity
    And the payment is failed with a LostCardPickUp error code

Scenario: Expired card
    Given a merchant
    And a customer
    And the customer uses an expired credit card for the payment
    When a request is made to the ProcessPayment endpoint
    Then the status code is UnprocessableEntity
    And the payment is failed with a ExpiredCard error code

Scenario: Cards with 3DS require further processing
    Given a merchant
    And a customer
    And the customer uses a credit cards that's enrolled in 3DS for the payment
    When a request is made to the ProcessPayment endpoint
    Then the status code is Accepted
    And the response includes the payment id for the current transaction with a AuthenticationRequired3DS status
