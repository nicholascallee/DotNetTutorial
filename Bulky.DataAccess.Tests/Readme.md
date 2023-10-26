**Files to Test:**

-   `ApplicationDbContext.cs` - Test its usage within repositories.
-   `UnitOfWork.cs` - Containing all the **Repository** files, e.g., `ProductRepository.cs`, `CategoryRepository.cs`, etc.
-   `DBInitializer.cs` - Test to verify that initialization is correct.

**Test Organization:**

-   Create a corresponding test file for each implementation file.

**Naming Conventions:**

-   The test file will be the same as the implementation file but with a "Tests" suffix.

**Setting up the Tests:**

-   Use a mock database connection to ensure that tests are isolated and can run quickly without modifying any real data.
-   Use a library called **Moq** to mock the `ApplicationDbContext` when testing repositories. This ensures that calls to the database are simulated.

# Work Log
**10/12/23:**
-   created test scaffolding for repository classes and all other data based classes. leaving off on the orderdetailrepositorytest on updatevalidorderdetail test. need to pick properties to update and update them.  i will need to look at how the order detail is used in the app to determine the best test aproach.
  
**10/25/23**
    - I have finished the data access testing and have decided to go with Specflow for gherkin based testing.
    - The bulk of the work was working on a generic way to test unitOfWork. i spent a good deal of time talking back and forth with chatgpt to finally figure out how to use reflections and dynamically call variable parameters.
