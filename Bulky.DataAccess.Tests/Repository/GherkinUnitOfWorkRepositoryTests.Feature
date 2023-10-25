Feature: Gherkin UnitOfWork Repository Tests

Scenario Outline: Updating and verifying entity values
Given I have an instance of <Class> at <Column> with a known value of <Value>
When I update the instance of <Class> at <Column> with value <NewValue>
Then the instance of <Class> at <Column> should have its value updated as <NewValue>

Examples:
|  Class           | Column        | Value          | NewValue  |
|  Product         | ISBN          | 0987654321     | 11111111  |
|  Category        | Name          | History        | Science   |
|  ApplicationUser | Name          | Nicholas Allee | Billy Bob |
