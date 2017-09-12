# ioc

Requirements:
 
  1.  Write an IoC (Inversion of Control) container, also known as a Dependency Injection container.
  2.  The container must allow you to register types.
     *   Example: container.Register<ICalculator, Calculator>()
  3.  The container must be aware and control object lifecycle for transient objects (a new instance is created for each request), and singleton objects (the same instance is returned for each request).
     *   Example: container.Register<ICalculator, Calculator>(LifecycleType.Transient), or container.Register<ICalculator, Calculator>(LifecycleType.Singleton)
  4.  The default lifecycle for an object should be transient
  5.  You must be able to resolve registered types through the container
     *   Example: container.Resolve<ICalculator>()
  6.  If you try to resolve a type that the container is unaware of it should throw an informative exception
  7.  When resolving from the container for a registered type, if that type has constructor arguments which are also registered types the container should inject the instances into the constructor appropriately (this is where dependency injection applies).
     *   Example Constructor: public UsersController(ICalculator calculator,  IEmailService emailService). If all 3 types for the controller, calculator, and email service are registered you should be able to resolve an instance of the UsersController.
  8.  You must write tests for all of this behavior using xUnit.
  9.  You must use git for source control and push your code to github.com<http://github.com/>. Please send me the link to your repository at least 1 day before the second interview.
  10. General Suggestion: Don't let the overall tasks overwhelm you. Break everything into smaller pieces that build up to the larger solution.
  11. Be prepared to answer a question along the lines of… How would your code change if given the requirement to add a new lifecycle (ThreadStatic for instance)? Would you be required to add new code, or modify existing code?
