Feature: Retrieve Payment
    As a merchant
    I want to retrieve details of a previous payment by ID

Scenario: Existing payment
    Given a merchant
    And a payment
    When the merchant requests the details of the payment
    Then the payment details are returned
    And the status code is Ok

Scenario: Missing payment
    Given a merchant
    When the merchant requests the details of the payment with a payment id that doesn't exist
    Then the status code is NotFound
    And a NotFound problem details response is returned

Scenario: Invalid request
    Given a merchant
    When the merchant requests the details of the payment with an invalid payment id
    Then the status code is BadRequest
    And a BadRequest problem details response is returned

Scenario: Requesting payment details that belong to another merchant
    Given a merchant
    And another merchant who processed a payment
    When the merchant requests the details of the payment of the other merchant
    Then the status code is NotFound
    And a NotFound problem details response is returned
