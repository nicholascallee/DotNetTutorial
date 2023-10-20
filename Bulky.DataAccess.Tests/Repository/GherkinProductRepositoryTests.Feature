Feature: Gherkin Product Repository Tests

Scenario Outline: Retrieving all products
   When I retrieve all products
   Then the number of products should be <ExpectedCount>

Examples:
| ExpectedCount |
| 6            |

Scenario Outline: Updating a product's title
   Given I have a product with ID <ProductId>
   When I update a product with ID <ProductId>
   Then the product with ID <ProductId> should have its title updated

Examples:
| ProductId |
| 1         |
| 2         |
| 3         |
