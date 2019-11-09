#### Hi! This is a Contest Winner solution 
##### *C#, SQL, ASP.Net MVC5, Azure, Bootstrap4. Additionally UI WinForms*

<img src="/Readme/00_asp.png?raw=true" width="320"/>

1.0. [Solution specifics](#10)<br>
2.0. [Data Design](#20)
   - 2.1. [Database](#21)
   - 2.2. [TrackerLibrary](#22)
     - 2.2.1. [Models](#221)
     - 2.2.2. [DataAccess](#222)
   - 2.3.  [Tournament logic](#23)
   
3.0.  [Web App](#30)
  - 3.1. [Structure](#31)
  - 3.2. [Views](#32)

4.0.  [WinForms](#40)
   - 4.1.  [Tournament Dashbord](#41)
   - 4.2.  [Tournament Viewer](#42)
   - 4.3.  [Create Tournament](#43)
   - 4.4.  [Create Prize](#44)
   - 4.5.  [Create Team](#45)
<br>

#### <a href='#10' id='10' class='anchor' aria-hidden='true'>1.0. Solution specifics</a>
<br>
1.1. General:
- tracks who wins in series of matches and the game;
- multiple teams take part in the contest;
- creates and schedules matches;
- losing team is eliminated;
- the last standing wins.
<br>
1.2. Detailed:
- variable number of players in a contest;
- in case of non-usual number of players (like %4 != 0), some random players skip the first round as winners to join players in the second act;
- ordering of teams in games is random;
- games can played whenever teams are ready and want to play, no schedule;
- but, each round must be completed before the next one;
- a simple scoring systems (winner:loser, 1:0);
- desktop solution, a website as a future option;
- MS-SQL data storage, a text file as an option;
- entry fee to the contest;
- money as award to variable number of places (percentage-based system), total prize does not exceed the income from the competition;
- simple report by email who won, and prizes;
- anyone who uses the application is able to fill in scores;
- no varied access, no passwords;
- emailing players/teams about planned games.

1.3. Solution Concepts:
- ASP.NET Intranet solution; 
- Windows Forms and Class Library;
- save data to local machine .CSV file;
- one user at a time on one application;
- email and SMS option;
- custom events;
- error handling;
- interfaces;
- randomizing.

#### <a href='#20' id='20' class='anchor' aria-hidden='true'>2.0. Data Design</a>
<br>
The solution consists of three projects:
- `TrackerLibrary` includes data models, tourmanent logic, data access logic.
- `MVCUI` web application, models, views, controllers, scripts.
- `WinFormsUI` Windows application, that works with SQL and CSV databases.
<br>
    
##### <a href='#21' id='21' class='anchor' aria-hidden='true'>2.1. Database diagram</a>
<br>
<img src="/Readme/00_DBDiagram.png?raw=true" width="640"/>

The database was designed in Misrosoft SSMS, and later was moved to Azure SQL server.

The database consists following tables and stored procedures:

dbo.MatchupEntries
   - id (PK, int, not null)
   - Matchupld (FK, int, not null)
   - ParentMatchupld (FK, int, null)
   - TeamCompetingld (FK, int, null)
   - Score (float, null)

dbo.Matchups
   - id (PK, int, not null)
   - Tournamentld (int, not null)
   - Winnerld (FK, int, null)
   - MatchupRound (int, not null)

dbo.PeopIe

   - id (PK, int, not null)
   - FirstName (nvarchar(100), not null)
   - LastName (nvarchar(100), not null)
   - EmailAddress (nvarchar(100), not null)
   - CellphoneNumber (varchar(20), null)

dbo.Prizes
   - id (PK, int, not null)
   - PIaceNumber (int, not null)
   - PIaceName (nvarchar(50), not null)
   - PrizeAmount (money, not null)
   - PrizePercentage (float, not null)

dbo.Team
   - id (PK, int, not null)
   - TeamName (nvarchar(100), not null)

dbo.TeamMembers
   - id (PK, int, not null)
   - Teamld (FK, int, not null)
   - Personld (FK, int, not null)

dbo.TournamentEntries
   - id (PK, int, not null)
   - Tournamentld (FK, int, not null)
   - Teamld (FK, int, not null)

dbo.TournamentPrizes
  - id (PK, int, not null)
  - Tournamentld (FK, int, not null)
  - Prizeld (FK, int, not null)

dbo.Tournaments
   - id (PK, int, not null)
   - TournamentName (nvarchar(20), not null)
   - EntryFee (money, not null)
   - Active (bit, not null)

List of sotred procedures:

- dbo.spMatchupEntries_GetByMatchup
- dbo.spMatchupEntries_lnsert
- dbo.spMatchupEntries_Update
- dbo.spMatchups_GetByTournament
- dbo.spMatchups_lnsert
- dbo.spMatchups_Update
- dbo.spPeople_GetAl|
- dbo.spPeople_Insert
- dbo.spPrizes_GetAll
- dbo.spPrizes_GetByTournament
- dbo.spPrizes_lnsert
- dbo.spTeam_GetAll
- dbo.spTeam_GetByTournament
- dbo.spTeamMembers_GetByTeam
- dbo.spTeamMembers_lnsert
- dbo.spTeams_lnsert
- dbo.spTournamentEntries_lnsert
- dbo.spTournamentPrizes_lnsert
- dbo.spTournaments_Comp|ete
- dbo.spTournaments_GetAll
- dbo.spTournaments_lnsert

<br>   
#### <a href='#22' id='22' class='anchor' aria-hidden='true'> 2.2. TrackerLibrary structure </a>
<br>

##### <a href='#221' id='221' class='anchor' aria-hidden='true'>2.2.1. Models</a>
<br>
Models describe entities in the project.  *C# classes*  are orginized in forlder `Models`.
<br>
- Team, `TeamModel.cs`
  - id;
  - TeamName (string);
  - TeamMembers (`List<Person>`);

- Person, `PersonModel.cs`
  - id;
  - FirstName (string);
  - LastName (string);
  - EmailAddress (string);
  - CellphoneNumber (string);

- Tournament, `TournamentModel.cs`
  - `EventHandler` OnTournamentComplete; *// set tournament active: true or false*
  - id;
  - TournamentName (string); *// football, or PingPong;*
  - EntryFee (decimal);
  - EnteredTeams(`List<Team>`);* // registered teams in the contest;*
  - Prizes (`List<Prize>`);
  - Rounds (`List<List<Matchup>>`); *//list of pairs of teams for the round;*

- Prize, `PrizeModel.cs`
  - id;
  - PlaceNumber (int); // *1-2-3*;
  - PlaceName (string); *// champion - silver - bronze;*
  - PrizeAmount (decimal);
  - PrizePercentage (double); *// of total income fee;*

- Matchup, `MatchupModel.cs`
  - id;
  - Entries (`List<MatchupEntry>`);* //teams and wins as a list;*
  - WinnerId (int);
  - Winner (TeamModel);
  - MatchupRound (int);

- MatchupEntry, `MatchupEntryModel.cs`
  - id;
  - TeamCompetingId (int);
  - TeamCompeting (TeamModel);
  - Score (double);
  - ParentMatchupId(int); *//identifier for the parent matchup*
  - ParentMatchup (Matchup).
            
#### <a href='#222' id='222' class='anchor' aria-hidden='true'>2.2.2.  DataAccess</a>
<br>
`DataAccess` folder describes data acces logic. It contains:
- interface IDataConnection;
- class  SqlConnector : IDataConnection;
- class TextConnector : IDataConnection;
- static class TextConnectorProsessor.
*Additionaly:*
- GlobalConfig.cs, Enum.cs files in the root.

<br>
<img src="/Readme/DataConnectionDiagram.png?raw=true" width="640"/>
<br>
2.2.2.1. `IDataConnection interface` contains abstract methods and is implemented in two classes that write data to DB or .csv file.
- `PrizeModel CreatePrize(args)`,
- `PersonModel CreatePerson(args)`,
- `TeamModel CreateTeam(args)`,
- `List<PersonModel> GetPerson_All()`.

2.2.2.2. `Class GlobalConfig` is a public static class which defines SQL or .csv type connection in the program. 
- `Connection`property returns an instance of `IDataConnection` type (`SqlConnector` or `TextConnector`),
- Method `InitializeConnections (args)` takes an instance of `enum DatabaseType` with value Sql or TextFile. Depending on that value, the `Connection` property is a new instance of `SqlConnector` or `TextConnector` class.
- `CnnString(string name)` method returns the value of `connectionString` with an attribute `name` from the `connectionStrings` section in `App.config`.

2.2.2.3. `Enum.cs` stores all the enums that we will need for the current solution.

2.2.2.4. `Class SqlConnector : IDataConnection` saves data to the DB.
- `public PrizeModel CreatePrize(PrizeModel model)` method establishes connection to the DB `using SqlConnection(GlobalConfig.CnnString())`. Then it constructs a new dynamic parameter bag `var p` (syntax from `Assembly Dapper`) and adds objects (the properties of `model` instance) and names (according to the column names in `dbo.Prizes`), similar to Dictionary collection. 
`model.Id` property is received from the DB, because it is an autoincremented key. Therefore `@id` is added to the list with a `ParameterDirection.Output` parameter. It also has an `output` type in the stored Sql procedure. 
Then `CreatePrize(PrizeModel model)` executes on the `connection` a stored DB procedure `dbo.spPrizes_Insert` and inserts `p` to the database. The value of `Id` sends out to the method. `model.Id` receives the `Id` value from database, and the updated `model` is returned.

- `public PersonModel CreatePerson(PersonModel model)` is similar to the previous `CreatePrize(args)` method.

- `public TeamModel CreateTeam(TeamModel model)` has two steps. The first step is totally similar to the prevoius methods. On the second step the method processes instances of `PersonModel` that are in the `team`. Foreach `PersonModel` in the `team` the method constructs a dynamic parameter bag, adds values, and calls stored Sql procedure. The `model` is returned afterwards.

- `public List<PersonModel> GetPerson_All()` method establishes connection to the DB `using SqlConnection(GlobalConfig.CnnString())`, and fills a new `List<PersonModel> output` with result of executed query `dbo.spPeople_GetAll` in the database. List of all registered members is returned.

2.2.2.5. `Class TextConnector : IDataConnection` saves data to .csv file. File path is stored in the `App.config`.  `TextConnectorProsessor` contains all the methods for writing and reading CSV files.
<br>
#### <a href='#23' id='23' class='anchor' aria-hidden='true'>2.3. Tournament logic </a>
<br>
Tournament logic conatins in the files
   - `TournamentLogic.cs`, describes how the tournament starts, continues, and its changes,
   - `EmailLogic.cs`, emailing users.
   - `SMSLogic.cs`, sending sms.
         
<br>
#### <a href='#30' id='30' class='anchor' aria-hidden='true'>3.0. Web application </a>
<br>

The project `MVCUI` was built with ASP.NET MVC5 as an Intranet solution. The project is not optimized for publication in the Internet because of large number of database queries (often in loops) which slows down the performance dramatically. 

#### <a href='#31' id='31' class='anchor' aria-hidden='true'>3.1. Structure </a>
<br>

The project consists of the three main folders:
- `Models`, which are pretty much similar to `Models` in `TrackerLibrary`, cointain several additional filelds due to web appp specifics. New models were created not to interfere with existing solution, and build ASP.NET as an addon.
   - `MatchupMVCModel.cs`
   - `RoundMVCModel.cs`
   - `TeamMVCModel.cs`
   - `TournamentMVCCreateModel.cs`
   - `TournamentMVCDetailsModel.cs`
- `Controllers` which contains logic and connects models and views.
   - `HomeController.cs`, views `Contact`, `Index`;
   - `PeopleController.cs`, views `Create`, `Index`;
   - `PrizesController.cs`, views `Create`, `Index`;
   - `TeamsController.cs`, views  `Create`, `Index`;
   - `TournamentsController.cs`, views  `Create`, `Details`;
- `Views`, which contains HTML pages, incl. partial views.

#### <a href='#32' id='32' class='anchor' aria-hidden='true'>3.2. Web Application Views</a>
<br>

- Home
<img src="/Readme/00_asp.png?raw=true" width="320"/>

- Tournaments View
<img src="/Readme/01_asp.png?raw=true" width="320"/>

- Create New Tournament
<img src="/Readme/02_asp.png?raw=true" width="320"/>

- People
<img src="/Readme/03_asp.png?raw=true" width="320"/>

- Teams
<img src="/Readme/04_asp.png?raw=true" width="320"/>

- Prizes
<img src="/Readme/05_asp.png?raw=true" width="320"/>

#### <a href='#40' id='40' class='anchor' aria-hidden='true'>4.0. WinForms</a>
<br>

The project contains forms for creating every model in the solution.
<br>
##### <a href='#41' id='41' class='anchor' aria-hidden='true'>4.1. Tournament Dashbord</a>
<br>
`TournamentDashbordForm.cs`

The form is to open en existing tournament from the list or create new.

<img src="/Readme/03_Dashboard.png?raw=true" width="320"/>

Opening an existing tournament leads to Viewer form.
<br>
##### <a href='#42' id='42' class='anchor' aria-hidden='true'>4.2. Tournament Viewer</a>
<br>
`TournamentViewerForm.cs`

<img src="/Readme/04_ContestView.png?raw=true" width="320"/>

On the form user can see all the teams that plays each round, set scores after the match and see matchups pairs.
<br>
##### <a href='#43' id='43' class='anchor' aria-hidden='true'>4.3. Create Tournament</a>
<br>
`CreateTournamentForm.cs`

<img src="/Readme/05_CreateContest.png?raw=true" width="320"/>

For reference, let's show mechanics behind forms for `Prize` and `Team`.
<br>
##### <a href='#44' id='44' class='anchor' aria-hidden='true'>4.4. Create Prize</a>
<br>
`CreatePrizeForm.cs`

The form creates rewards for winners in the contest.

<img src="/Readme/01_CreatePrize.png?raw=true" width="320"/>

Event handlers in form `CreatePrizeForm.cs`:

#### `Create` click -> `ValidateForm()` -> `new PrizeModel(args)` -> `CreatePrize(model)` -> Clear form

<img src="/Readme/CreatePrizeDiagram.png?raw=true" width="640"/>

A private method `ValidateForm()` inspects the fullfilling of the form, and shows message *`Please, check the form!`* if any problem appears. If the validation is passed, a new `PrizeModel` instance is created. `PrizeModel class` contains a user's constructor that parses string values from the form to the fields.

A weak reference calls a static property `GlobalConfig.Connection` that returns `IDataConnection` interface containing an abstract method `CreatePrize(PrizeModel model)`. The `GlobalConfig.Connection` property can return instance of `SqlConnector` or `TextConnector` class that both implement `IDataConnection` interface. `CreatePrize(model)` method stores the instance to Database or .csv file according to `DatabaseType` from `GlobalConfig.Connections`. After that the form is cleared.
<br>
##### <a href='#45' id='45' class='anchor' aria-hidden='true'>4.5. Create Team</a>
<br>
`CreateTeamForm.cs`<br/>
The form is to create a new team player, select existing member, make a team, add a new team, and remove selected members from the team.

<img src="/Readme/02_CreateTeam.png?raw=true" width="320"/>

`CreateTeamForm.cs` structure:
- `List<PersonModel> availableTeamMembers` keeps the list of all available players to make teams. It is recieved from `GlobalConfig.Connection` property that returns instance (`SqlConnector` or `TextConnector`) which implements method `GetPerson_All()` from `IDataConnection` interface. `GetPerson_All()` returns `List<PersonModel>`. 
  - in `SqlConnector class`, the method connects to the database, executes stored procedure `dbo.spPeople_GetAll`, and grabs all the records from the table `People`.
  - in `TextConnector class`, it READS FROM FILE.
 
-  `List<PersonModel> selectedTeamMembers` is a new list of `PersonModels`, that is to keep a new list of selected players (from `availableTeamMembers`) to make teams.

- `private bool ValidateForm()` checks if fileds `First Name`, `Last Name`, `Email`, `Cellphone` contain values.

- `private void WireUpLists()` is to refresh DropDownList `Select Team Member` (which contains list of available members), and TextBox (with selected members for making a team). The method is called after every change in both lists.

<img src="/Readme/CreateTeamDiagram.png?raw=true" width="640"/>

Event handlers in form `CreatePrizeForm.cs`:

#### `Create Member` click -> `ValidateForm()` -> `new PersonModel(args)` -> `CreatePerson(p)` -> `selectedTeamMembers.Add(p)` -> `WireUpLists()` -> Clear form

`ValidateForm()` method inspects the form, and shows message *`Please check the input!`* if any problem appears. If the validation is passed, a new `PersonModel` instance is created, and values are assigned to its properties.

A weak reference calls a static property `GlobalConfig.Connection` that returns `IDataConnection` interface containing an abstract method `CreatePerson(args)`. The `GlobalConfig.Connection` property can return instance of `SqlConnector` or `TextConnector` class that both implement `IDataConnection` interface. `CreatePerson(p)` method stores the instance to the Database or .csv file according to `DatabaseType` from `GlobalConfig.Connections`.

`PersonModel p` is added then to the list of `selectedTeamMembers`, which may already contain a list of members, if our data files were not empty. `WireUpLists()` method refreshes view of the list. After that the form is cleared.

#### `Add Member` click -> `new PersonModel()` from `availableTeamMembers` -> `availableTeamMembers.Remove(p)` -> `selectedTeamMembers.Add(p)` -> `WireUpLists()`

When the member is selected in `Select Team Member` field (`List<PersonModel> availableTeamMembers`), a new `PersonModel` instance captures selected value from the list and upcasts it to the `PersonModel` type. Then, the created `PersonModel` instance is removed from the list of availableTeamMembers, and is added to ListBox (`List<PersonModel> selectedTeamMembers`). The form is refreshed to show the updated lists.

#### `Remove Selected` click -> `new PersonModel()` from `selectedTeamMembers` -> `selectedTeamMembers.Remove(p)` -> `availableTeamMembers.Add(p)` -> `WireUpLists()`

This set of actions is opposite to `Add Member` click when an insatce is removed from `selectedTeamMembers` and is added to `availableTeamMembers`.

#### `Create Team` click -> `new TeamModel` -> `CreateTeam(t)` -> ...

A new instance of `TeamModel` is created. Property `TeamName` is taken form `Team Name` field, and `TeamMembers` is a `List<PersonModel> selectedTeamMembers`.

A weak reference calls a static property `GlobalConfig.Connection` that returns `IDataConnection` interface containing an abstract method `CreateTeam(args)`. The `GlobalConfig.Connection` property can return instance of `SqlConnector` or `TextConnector` class that both implement `IDataConnection` interface. `CreateTeam(t)` method stores the instance to the Database or .csv file according to `DatabaseType` from `GlobalConfig.Connections`.


