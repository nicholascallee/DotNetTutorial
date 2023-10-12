

### Related Course:
https://www.udemy.com/course/complete-aspnet-core-21-course/

### Course Github:
 https://github.com/bhrugen/Bulky_MVC/tree/master


# Test Plan

## Location of Test Projects:
- BulkyBookWeb.Tests for the web solution.
- BulkyBookUtility.Tests for the utility solution. 
- BulkyBookModel.Tests for the model solution.
- BulkyBookDataAccess.Tests for the data access solution.
- **Why:** 
	- easier to manage dependencies, and the separation ensures that test code doesn't accidentally get bundled with or impact production code. 

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
   	    
## Setting Up the Test Projects:
- Each test project will reference the project it's testing. This means BulkyBookWeb.Tests should have a reference to the BulkyBookWeb project, and so on.    
- Install necessary packages: A testing framework (xUnit, NUnit, or MSTest). xUnit is quite popular for .NET Core projects. Moq for mocking in unit tests. Other necessary libraries specific to what is being tested.

## Priority and Reason for testing:
   - **Data Access Layer (DAL):** 
	    - **Why:** 
	    The data is the backbone of most
	    applications. Ensuring that your CRUD operations (Create, Read,
	    Update, Delete) are working correctly is crucial. Any bugs at this
	    layer can have cascading effects on higher layers. 
	    **What to Test:** 
	    Test database connections, CRUD operations, and any complex queries
	    or data transformations. 

   - **Business Logic or Services:**    
	   - **Why:** 
	    This is where the core logic of your application resides.
	    Errors here can lead to incorrect data processing or invalid
	    application states. 
	    **What to Test:** 
	    Test methods that have complex
	    logic, calculations, or data manipulations. Make sure all expected
	    paths (including edge cases) are covered. 
   - **Web Controllers & Endpoints:**
	    - **Why:** 
	    These serve as the gatekeepers to your application's
	    functionality. Incorrect routing, data binding, or response handling
	    can make parts of your app inaccessible or malfunction.
	    **What to Test:** 
	    Test routing, data validation/binding, and expected responses.
	    Ensure that the controller actions return the correct view or data
	    and handle errors gracefully. 
    
   - **Models:**
	   - **Why:** 
	    While models might seem straightforward, ensuring their
	    correctness is vital, especially if there's any logic associated
	    with setting or getting properties. 
	    **What to Test:** 
	    Any validation
	    logic or business rules tied to the models. If they're simple POCOs
	    (Plain Old CLR Objects) without any logic, they might not need
	    extensive testing. 
    
   - **Utility:**   
	   - **Why:** 
	    Utilities might be used across multiple parts of the
	    application, so their correctness is crucial. However, they're often
	    less tied to core business functionality, which is why they're a bit
	    lower on the list. 
	    **What to Test:** 
	     Any method or function within the
	    utility. Focus on those that are complex or widely used throughout
	    the application.
    
  - **Frontend/UI:**
	  -  **Why:** 
	    While crucial for user experience, UI issues are often less
	    critical than backend ones in terms of data integrity or application
	    functionality (unless you're building a heavily client-side
	    processed app). 
	    **What to Test:** 
	     Test key user flows, form submissions,
	    and any dynamic or interactive parts of the UI. 

 - **Integration & End-to-End Testing**
	 - **Why:** 
	     After testing individual components, it's essential to test
	    them working together. This captures issues that unit tests might
	    miss. 
	    **What to Test:** 
	     Key user journeys from start to finish,
	    including database updates and UI interactions. 

- **Performance & Load Testing:**
  -  **Why:** 
    Ensuring that your application works under load is essential
    for production readiness, especially if you expect high traffic.
    **What to Test:** 
     Test how the system behaves under expected and peak
    loads. Monitor response times, error rates, and system resources.

 - **Security Testing:**
	  - **Why:** 
	    Security is paramount, but the reason it's lower on this list
	    is that you'd typically want to ensure functionality first, then
	    ensure that functionality is secure. However, if you're in a domain
	    with high security needs (like finance), you might prioritize this
	    higher.
	    **What to Test:** 
	     Test for vulnerabilities like SQL injection,
	    XSS, CSRF, etc. Ensure that authentication and authorization
	    mechanisms are robust.
