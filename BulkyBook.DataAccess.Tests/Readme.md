**Files to Test:**

-   `ApplicationDbContext.cs` - Test its usage within repositories.
-   All the **Repository** files, e.g., `ProductRepository.cs`, `CategoryRepository.cs`, etc.
-   `UnitOfWork.cs`
-   `DBInitializer.cs` - Test to verify that initialization is correct.

**Test Organization:**

-   Create a corresponding test file for each implementation file.

**Naming Conventions:**

-   The test file will be the same as the implementation file but with a "Tests" suffix.

**Setting up the Tests:**

-   Use an in-memory database or a mock database connection to ensure that tests are isolated and can run quickly without modifying any real data.
-   Use a library called **Moq** to mock the `ApplicationDbContext` when testing repositories. This ensures that calls to the database are simulated.
-   Always start with a fresh database state for each test. You can achieve this by re-initializing the in-memory database or resetting mock setups before each test.	


**Findings**
-	I think we should skip dbinitializer because it only happens if the db is not present, which in turn checks for pending migrations. Not sure how to emulate pending migrations.

# Work Log
**10/12/23:**
-   created test scaffolding for repository classes and all other data based classes. leaving off on the orderdetailrepositorytest on updatevalidorderdetail test. need to pick properties to update and update them.  i will need to look at how the order detail is used in the app to determine the best test aproach.
