Feature: Update and verify entity values in a repository

  In order to ensure that values in the repository can be updated and verified
  As a developer
  I want to update an entity's value and verify the changes

  Scenario Outline: Updating and verifying entity values
    Given I have an instance of <Class> at <Column> with a known value of <Value>
    When I update the instance of <Class> at <Column> with value <Value>
    Then the instance of <Class> at <Column> should have its value updated as <Value>

    Examples:
      | Class       | Column        | Value       |
      | Product     | ProductName   | NewName     |
      | Category    | CategoryName  | NewCategory |
      | User        | Username      | NewUsername |
