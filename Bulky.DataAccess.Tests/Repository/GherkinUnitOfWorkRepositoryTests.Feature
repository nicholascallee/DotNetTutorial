Feature: Gherkin UnitOfWork Repository Tests

Scenario Outline: UnitOfWork Repository Is Updated With New Data
Given I have an instance of <Class> at <Column> with a known value of <Value>
When I update that instance with value <NewValue>
Then requerying the instance should return <NewValue>

Examples:
|  Class           | Column        | Value          | NewValue  |
|  Product         | ISBN          | 0987654321     | 11111111  |
|  Category        | Name          | History        | Science   |
|  ApplicationUser | Name          | Nicholas Allee | Billy Bob |
