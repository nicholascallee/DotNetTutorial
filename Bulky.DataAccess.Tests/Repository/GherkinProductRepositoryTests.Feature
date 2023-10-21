Feature: Gherkin Product Repository Tests


Examples:
| ExpectedCount |
| 6            |





Scenario Outline: Updating a product's values
	Given I have a product with ID <ProductId>
	When I update the <ColumnName> of a product with value <ColumnValue>
	Then the product with <ColumnName> should have its value updated as <ColumnValue2>

Examples:
| ProductId | ColumnName  | ColumnValue           | ColumnValue2          |
| 1         | Title       | New Product Title     | New Product Title     |
| 1         | Description | Updated Description   | Updated Description   |
| 1         | ISBN        | 1234567890            | 1234567890            |
| 1         | Author      | John Doe              | John Doe              |
| 1         | ListPrice   | 25.99                 | 25.99                 |
| 1         | Price       | 20.99                 | 20.99                 |
| 1         | Price50     | 19.99                 | 19.99                 |
| 1         | Price100    | 18.99                 | 18.99                 |
| 2         | Title       | Another Title         | Another Title         |
| 2         | Description | Another Description   | Another Description   |
| 2         | ISBN        | 0987654321            | 0987654321            |
| 2         | Author      | Jane Smith            | Jane Smith            |
| 2         | ListPrice   | 30.99                 | 30.99                 |
| 2         | Price       | 28.99                 | 28.99                 |
| 2         | Price50     | 27.99                 | 27.99                 |
| 2         | Price100    | 26.99                 | 26.99                 |
| 3         | Title       | Third Product         | Third Product         |
| 3         | Description | Third Description     | Third Description     |
| 3         | ISBN        | 1122334455            | 1122334455            |
| 3         | Author      | Alice Cooper          | Alice Cooper          |
| 3         | ListPrice   | 35.99                 | 35.99                 |
| 3         | Price       | 33.99                 | 33.99                 |
| 3         | Price50     | 32.99                 | 32.99                 |
| 3         | Price100    | 31.99                 | 31.99                 |




