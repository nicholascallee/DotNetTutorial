# Testing Overview

**Files to Test:**

-   `ApplicationDbContext.cs` - Uses moq to ensure the correct seeding of hardcoded values into the database for use in the application.
-   `UnitOfWork.cs` - Uses Gherkin and moq to test the usage of the UnitOfWork repository class that talks to the sql server. 
-   `DBInitializer.cs` - Uses moq to verify data initilization is performed correctly.

**Test Organization:**

-   1 test file for each application file needing tested.

**Naming Conventions:**

-   The test file will be the same as the implementation file but with a "Tests" suffix.

**Setting up the Tests:**

-   Use a mock database connection to ensure that tests are isolated and can run quickly without modifying any real data.
-   Use a library called **Moq** to mock the `ApplicationDbContext` when testing repositories. This ensures that calls to the database are simulated.


## Work Log
### Data Access Section.

**10/12/23:**
-   created test scaffolding for repository classes and all other data based classes. leaving off on the orderdetailrepositorytest on updatevalidorderdetail test. need to pick properties to update and update them.  i will need to look at how the order detail is used in the app to determine the best test aproach.
  
**10/25/23:**

-   I have finished the data access testing and have decided to go with Specflow for gherkin based testing.
-   The bulk of the work was working on a generic way to test unitOfWork. i spent a good deal of time talking back and forth with chatgpt to finally figure out how to use reflections and dynamically call variable parameters.


# Results:

Data Access section is completly tested.

![image](https://github.com/nicholascallee/DotNetTutorial/assets/141438641/92498c87-925c-42a9-ac52-8b7b02a4fd4d)
