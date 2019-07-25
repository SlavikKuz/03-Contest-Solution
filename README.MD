#### Hi! This is a Contest Winner solution (.Net, WinForms, MSSQL, Dapper) 

### *(document in progress)*

1.0. [Solution requirements](#01)<br/>
	1.1. General specifics.<br/>
	1.2. Detailed specifics.<br/>
	1.3. Solution Concepts.<br/>
2.0. [Data Design](#02)<br/>
	2.1. Classes.<br/>
3.0. [Database diagram
](#03)<br/>
4.0. [Planning solution](#04)<br/>
5.0. [  ](#05)<br/>

### <a href='#01' id='01' class='anchor' aria-hidden='true'>1.0. Solution requirements</a>

1.1. General specifics:
- tracks who wins in series of games;
- multiple teams take part in the contest;
- creates a contest plan;
- schedules games;
- losing team is eliminated;
- the last standing wins.

1.2. Detailed specifics:
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
- Windows Forms and Class Library;
- SQL/CSV file;
- one user at a time on one application;
- email sending;
- custom events;
- error handling;
- interfaces;
- randomizing.

### <a href='#02' id='02' class='anchor' aria-hidden='true'>2.0. Data Design</a>

2.1. Classes:

- Team
  - TeamMembers (List<Person>);
  - TeamName (string);

- Person
  - FirstName (string);
  - LastName (string);
  - EmailAddress (string);
  - CellphoneNumber (string);

- Tournament
  - TournamentName (string); //football, or PingPong;
  - EntryFee (decimal);
  - EnteredTeams(List<Team>); // registered teams in the contest;
  - Prizes (List<Prize>);
  - Rounds (List<List<Matchup>>); //list of pairs of teams for the round;

- PrizeModel class
  - PlaceNumber (int); // 1-2-3;
  - PlaceName (string); // champion - silver - bronze;
  - PrizeAmount (decimal);
  - PrizePercentage (double); // of total income;

- Matchup
  - Entries (List<MatchupEntry>); //teams and wins as a list;
  - Winner (Team);
  - MatchupRound (int);

- MatchupEntry
  - TeamCompeting (Team);
  - Score (double);
  - ParentMatchup (Matchup).

Orginized in forlders

### <a href='#03' id='03' class='anchor' aria-hidden='true'>3.0. Database diagram
</a>

<img src="/Readme/00_DBDiagram.png?raw=true" width="640"/>

### <a href='#04' id='03' class='anchor' aria-hidden='true'>4.0. Data Connection</a>

<img src="/Readme/DataConnectionDiagram.png?raw=true" width="640"/>

4.1. `IDataConnection interface` contains abstract methods and is implemented in two classes that write data to DB or .csv file.
- `PrizeModel CreatePrize(args)`,
- `PersonModel CreatePerson(args)`,
- `TeamModel CreateTeam(args)`,
- `List<PersonModel> GetPerson_All()`.

4.2. `Class GlobalConfig` is a public static class which defines SQL or .csv type connection in the program. 
- `Connection`property returns an instance of `IDataConnection` type (`SqlConnector` or `TextConnector`),
- Method `InitializeConnections (args)` takes an instance of `enum DatabaseType` with value Sql or TextFile. Depending on that value, the `Connection` property is a new instance of `SqlConnector` or `TextConnector` class.
- `CnnString(string name)` method returns the value of `connectionString` with an attribute `name` from the `connectionStrings` section in `App.config`.

4.3. `Enum.cs` stores all the enums that we will need for the current solution.

4.4. `Class SqlConnector : IDataConnection` saves data to DB.
- `public PrizeModel CreatePrize(PrizeModel model)` method establishes connection to the DB `using SqlConnection(GlobalConfig.CnnString())`. Then it constructs a new dynamic parameter bag `var p` (syntax from `Assembly Dapper`) and adds objects (the properties of `model` instance) and names (according to the column names in `dbo.Prizes`), similar to Dictionary collection. 
`model.Id` property is received from the DB, because it is an autoincremented key. Therefore `@id` is added to the list with a `ParameterDirection.Output` parameter. It also has an `output` type in the stored Sql procedure. 
Then `CreatePrize(PrizeModel model)` executes on the `connection` a stored DB procedure `dbo.spPrizes_Insert` and inserts `p` to the database. The value of `Id` sends out to the method. `model.Id` receives the `Id` value from database, and the updated `model` is returned.

- `public PersonModel CreatePerson(PersonModel model)` is similar to the previous `CreatePrize(args)` method.

- `public TeamModel CreateTeam(TeamModel model)` has two steps. The first step is totally similar to the prevoius methods. On the second step the method processes instances of `PersonModel` that are in the `team`. Foreach `PersonModel` in the `team` the method constructs a dynamic parameter bag, adds values, and calls stored Sql procedure. The `model` is returned afterwards.

- `public List<PersonModel> GetPerson_All()` method establishes connection to the DB `using SqlConnection(GlobalConfig.CnnString())`, and fills a new `List<PersonModel> output` with result of executed query `dbo.spPeople_GetAll` in the database. List of all registered members is returned.
 
***

4.5. `Class TextConnector : IDataConnection` saves data to .csv file. File path is stored in the `App.config`. The class contains `const PrizesFile` that stores file name *`PrizeModels.csv`*. Method `CreatePrize` loads the file, reads lines into the list, and convert strings to `PrizeModel` instances. The method also receives last `id` from the file, and adds +1 to create id for the new record *(SQL has autoincrement option)*. Is the file is empty, `id = 1`. Then the method creates a new instance with the id, makes a string of the instance `PrizeModel`, and writes the string down to the end of the file. The method returns curren `model` instance that can be used by other models.

4.6. Methods that `TextConnector` uses are implemented in `class TestConnectorProsessor`. The class has own `TextHelpers` namespace. Method `FullFilePath` returns full path to the file. Extention method `LoadFile` checks if the file already exits and reads all the lines into list of strings. `ConvertToPrizeModels` extention method splits every string into array of strings, and array members are assigned to propeties of a new `PrizeModel` instance. The method returns the list of `PrizeModel` instances. Extention method `SaveToPrizeFile` receives list of instances `PrizeModel`, turn them in strings, and write down the whole list to the file. 

5. Forms inthe solution

5.1. Prize form<br/>

The form creates rewards for winners in the contest.

<img src="/Readme/01_CreatePrize.png?raw=true" width="320"/>

Event handlers in form `CreatePrizeForm.cs`:

#### `Create` click -> `ValidateForm()` -> `new PrizeModel(args)` -> `CreatePrize(model)` -> Clear form

<img src="/Readme/CreatePrizeDiagram.png?raw=true" width="640"/>

A private method `ValidateForm()` inspects the fullfilling of the form, and shows message *`Please, check the form!`* if any problem appears. If the validation is passed, a new `PrizeModel` instance is created. `PrizeModel class` contains a user's constructor that parses string values from the form to the fields.

A weak reference calls a static property `GlobalConfig.Connection` that returns `IDataConnection` interface containing an abstract method `CreatePrize(PrizeModel model)`. The `GlobalConfig.Connection` property can return instance of `SqlConnector` or `TextConnector` class that both implement `IDataConnection` interface. `CreatePrize(model)` method stores the instance to Database or .csv file according to `DatabaseType` from `GlobalConfig.Connections`. After that the form is cleared.

5.2. Create Team form<br/>
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

#### `Create Team` click -> `new TeamModel` -> `CreateTeam(t)` -> NOT FINISHED

A new instance of `TeamModel` is created. Property `TeamName` is taken form `Team Name` field, and `TeamMembers` is a `List<PersonModel> selectedTeamMembers`.

A weak reference calls a static property `GlobalConfig.Connection` that returns `IDataConnection` interface containing an abstract method `CreateTeam(args)`. The `GlobalConfig.Connection` property can return instance of `SqlConnector` or `TextConnector` class that both implement `IDataConnection` interface. `CreateTeam(t)` method stores the instance to the Database or .csv file according to `DatabaseType` from `GlobalConfig.Connections`.
