# BookLoversApi

## Purpose
  Main purpose of this library was to get better understanding of concepts behind so called modular monolith,
  get grasp how to design particular parts of application to be more resilient to changes and actually familiarize
  myself with a project that is sllightly bigger than simple TO_DO_LIST app with `POST`,`PUT`,`GET`,`DELETE` action.
  
  Now instead of making long description of what actually modular monlith is and the whole point of it i will point you to the repository 
  that i have been using as a guidline while trying to develop the whole application.
  
 <ul>
  <li>modular-monolith-with-ddd (https://github.com/kgrzybek/modular-monolith-with-ddd)</li>
</ul>

## Quick overview
  Project is based on popular books site https://www.goodreads.com/. Main features of the project are:
 <ul>
  <li>Adding,editing and removing books as a librarian.</li>
  <li>Creating tickets with new books or authors as a regular user.</li>
  <li>Managing,sorting and viewing other users bookcases.</li>
  <li>Adding,editing,liking and reporting reviews</li>
  <li>Managing profile,following other users and timeline where user can see what has happend with his profile recently</li>
</ul>
 
 -  Authorization - Module used to authenticate and authorize user in the system. Users are differentiated to two main groups which are `Readers` and `Librarians`.
    Based on that during authentication process, appropriate token is being created and then user can access resources based on that token or perform certain
    actions in the system if he has permission to do that.
    
  - Bookcases - One of the main modules of our application where user can keep his books collection. User can put book on shelf,change shelf or remove book from certain shelf.
    There are two types of shelves `core` and `custom`.The first one is not editable,every user is going to have set of `Read`,`Now reading`,`Want to read` shelves where they
    can add as many books as they want but they cannot remove the shelf or edit its name. Second `custom` group of shelves are editable. User can change the name of the shelf,
    remove shelf from the bookcase and ofcourse add books to it but the amount of books that can be put on certian custom shelf is limited.
    
   - Books - Main module where all available books in the system are.User can check basics information about the book like ISBN, authors,publisher or publication date.
    Module also knows how to differentiate valid book from the invalid ones and that is being done by validating incoming ISBN number of the book, validating authors of the book
    (by checking if they actually exists in the system),validating publisher of the book (same as authors) or series if the book have one.
    
   - Librarians- Main purpose of the module is to resolve `tickets` that are being created by users. If `reader` wants to add book that is not available in the system, then    unless his role is `librarian` he cannot do that directly. Firstly he needs to create a ticket with a certain book, and then if the book will fullfill all the requirments then
    the book is going to be accepted and added to the system. `Librarian` also can promote other user to be librarian or degrade to default reader, then if user was degraded   once he cannot be promoted to `librarian` role again.
    
   - Readers - Each user after registration is going to have his own profile,timeline and notification wall. He can update profile by adding necessary infromation 
    like firstname,secondname or date of birth. Timeline serves as history of what sort of actions user performed in the past. Notification wall as it name sugests
    informs user of actions that affected him, for example: someone decied to follow him, his book was accepted by the librarian or review that he added just has been liked by       someone else.
    
   - Ratings - User while adding review also can give a rating to a book. Ratings are aggregated and then `Book`,`Author`,`Publisher` average is recalculated.

## How modules work internally ?
   Every module has public api that looks like:
   ```csharp
    public interface IModule<T> : IModule
    {
        Task<QueryResult<TQuery, TResult>> ExecuteQueryAsync<TQuery, TResult>(
            TQuery query)
            where TQuery : class, IQuery<TResult>;

        Task<ICommandValidationResult> SendCommandAsync<TCommand>(
            TCommand command)
            where TCommand : class, ICommand;
    }
   ```
   Now to change the state of our application we have to send a `command`.`Command` has a corresponding `command handler` that will perform certian action on 
   our domain object.This action may produce one,many or none `domain events`.If there is a need then we have to react to these `domain events` and send `internal commands`
   that will change state of our other domain objects (in boundary of the specific module).After whole process (or transcation) is done then we can send a special type of
   event which is `integration event` that will force other modules to react in certain way.
   
   To cast a little bit more light to what has been said i will briefly present, how the process of `blocking user` works from the beginnning till very end.
    
   * User sends a `POST` request with writemodel like:
   ```csharp 
   public class BlockAccountWriteModel
   {
        public Guid BlockedReaderGuid { get; set; }
   }
   ```
   * That request is being initialy validated by library (https://fluentvalidation.net/)
   ```csharp
   public BlockAccountValidator()
   {
            RuleFor(p => p.WriteModel).NotNull().WithMessage("Dto cannot be null");

            When(p => p.WriteModel != null, () =>
                RuleFor(p => p.WriteModel.BlockedReaderGuid)
                    .NotEmpty()
                    .WithMessage("Invalid user guid."));
    }
  ```
  * If the request is valid then the command handler that is linked with given command is going to be invoked.
  ```csharp
  public async Task HandleAsync(BlockAccountCommand command)
  {
       var user = await this._unitOfWork.GetAsync<User>(command.WriteModel.BlockedReaderGuid);

       this._accountBlocker.BlockUser(user);

       await this._unitOfWork.CommitAsync(user);

       var @event = new UserBlockedIntegrationEvent(user.Guid, user.IsInRole(Role.Librarian.Name));

       await this._inMemoryEventBus.Publish(@event);
   }
  ```
  * Now if the domain rules were validated successfully then after `await this._inMemoryEventBus.Publish(@event)` other modules should react accordingly to was has
  happend in our command handler. So `Bookcases module` will send a command of type `ArchiveBookcaseOwnerInternalCommand` to keep `Bookcases module` in sync, the same goes
  for `Readers module` (ofcourse it will send a different type of command or commands i.e `ArchiveProfileInternalCommand` and `ArchiveNotificationWallInternalCommand` ) and
  any other module that has to respond to certain `integration event`.

## Technology used in project
  Few loose thoughts about technology used in project.
  - ASP.NET - Someone may ask why not ASP.NET Core ? The answer is quite simple. I had an old project which lasted from times when i was starting to learn programming
  and i decided that it would be nice to refactor the **lasagne** that i left there.
  - Event sourcing - it is a simple event store that i had written by myself based on theory that i found on internet. Ofcourse someone my say that i reinventing the wheel
  and that is propably true. But the main purpose of it was to learn some rudimentary concepts about it and i think i would not be able to do it if i would use some well
  developed and tested in battle event store like (http://neventstore.org/). Ofcourse there is an argument that i could have written it in a wrong way and learn some bad 
  practices because of it and that maybe is true aswell...
  - JWT - Nothing extraordinary, popular standard used for authentication,authorization purposes. One thing worth metioning here is that token are signed with secret key
  (with the HMAC alghoritm) which may create some security issues especially when we cannot provide security for the secret that we are using to sign our tokens. Alternative is 
  to sign tokens with asymetric RSA key (actually two different keys) especially when we would like to exchange tokens with our client app (Angular,React,Vue).
  - MS SQL - Standard, used mainly becasuse of the fact that it can be easily integrated with Entity Framework, and beceause at this moment and time i have little to no
  knowledge about NOSQL databases.
  - CQRS - application is separated into two different parts which are `readside` and `writeside`. `Writeside` is where the business logic and validation resides and `readside`   is where user can query available data. This pattern is often used to ensure that `single-responsibility` principle is present in the project that we are working on and 
  to scale in terms of performance `readside` and `writeside` of the application. For example in most cases `readside` has much higher load then `writeside` so we can adjust to 
  it by using document database on `readside` and relation database on `writeside`.In my case on both sides i used relational database mostly becasue of the simplicity
  and to not complicate the project by mixing different type of technologies.
  
  ## How to run
   * Install Visual Studio 2019 Community.
   * Install MS SQL Express.
   * Rebuild application using Visual Studio.
   * Unzip seed file in Seed project (optional)
   * Run start.cmd script
      - Script will start application on localhost:64892
      - With the first start of the application, all necessary databases will be created.
      - If you decided to unzip seed file you will have to wait a few seconds till databases, seeding process will finish.
      
      - To actually make api calls, you will have to obtain JWT token. To do that, make an `POST` request on `http://localhost:64892/auth/token`
        with params:
        > - username:user11@gmail.com
        > - password:Babcia123
        > - grant_type:password
        > - client_id:00f80a32-0205-4aff-94d9-46635d8c431c
        > - client_secret:DUPA 
       
      - Response that you are going to obtain will look like: `"access_token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjZjYyMmM1OS0yMzdjLTRkYzctYmQ0OC02ZjQ4YWMyNjIzOWEiLCJqdGkiOiJhYmEzNDRjOC1hYzFlLTQ5YTQtYmY3N"`
      
      - Now you can for example create a publisher by sending a request on url `http://localhost:64892/api/publishers` that will contain `access_token` in authorization header.
      
  ## Libraries
   - ASP.NET Framework - https://dotnet.microsoft.com/apps/aspnet
   - Microsoft Owin - https://github.com/aspnet/AspNetKatana/
   - MS SQL - https://www.microsoft.com/pl-pl/sql-server/sql-server-downloads
   - Automapper - https://automapper.org/
   - Dapper - https://github.com/DapperLib/Dapper
   - Fluent Assertions - https://fluentassertions.com/
   - FluentScheduler - https://github.com/fluentscheduler/FluentScheduler
   - FluentValidation - https://fluentvalidation.net/
   - Moq - https://github.com/moq/moq4
   - Nunit - https://nunit.org/
   - Newtonsoft json - https://www.newtonsoft.com/json
   - Serilog - https://serilog.net/
   - System.Identitymodel.Tokens.Jwt - https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/
   - Entity Framework 6 - https://github.com/dotnet/ef6
   - Z.EntityFramework.Extensions - https://entityframework-extensions.net/
   - Ninject - http://www.ninject.org/
   - NitoAsync - https://www.nuget.org/packages/Nito.AsyncEx
   - Nuke.build - https://nuke.build/
   - StyleCop.Analyzers - https://github.com/DotNetAnalyzers/StyleCopAnalyzers
   - DbUp - https://dbup.readthedocs.io/en/latest/
  ## Resources
   - https://github.com/kgrzybek/modular-monolith-with-ddd
   - https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs
   - https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html
   - https://bitoftech.net/2014/10/27/json-web-token-asp-net-web-api-2-jwt-owin-authorization-server/
   - http://www.kamilgrzybek.com/design/domain-model-validation/
   - https://www.infoq.com/minibooks/domain-driven-design-quickly/
