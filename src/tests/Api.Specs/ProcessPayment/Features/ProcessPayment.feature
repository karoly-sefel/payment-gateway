Feature: Process Payment
    As a merchant
    I want to be able to take card payments from my customers
    So that I can sell my products and services

Scenario: Successful payment with a payment id returned
    Given a merchant
    When a request is made to the ProcessPayment endpoint
    Then the status code is Created
    And the response includes the payment id for the current transaction
