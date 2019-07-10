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
- SQL/TXT file;
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
  - ParentMatchup (Matchup);

### <a href='#03' id='03' class='anchor' aria-hidden='true'>3.0. Database diagram
</a>

<img src="/Readme/DBDiagram.PNG?raw=true" width="640"/>

### <a href='#04' id='03' class='anchor' aria-hidden='true'>4.0. Planning solution</a>

4.1. Data Connection<br/>

We create `IDataConnection interface` which contains an abstract method that accept and return `PrizeModel class`. 
The interface will be implemented in two classes that write data to DB or .txt file.

`Class GlobalConfig` is a public static class for storing data sources info. It has the `IDataConnection` static property for data connection. Static method `InitializeConnections` creates connections to SQL DB or .txt file, depends on the privious choice. `CnnString` method goes to `App.config` and get previously added `connectionStrings`. Choice options are from `Enum.cs` which will store all the enums for the current solution. 

`Class SqlConnector : IDataConnection` saves data to DB.
It establishes connection. Then using `Dapper lib`executes the store procedure in the DB with the set of parameters. Query returns output value for `@id`.

`Class TextConnector : IDataConnection` saves data to .csv file. File path is stored in the `App.config`. The class contains `const PrizesFile` that stores file name *`PrizeModels.csv`*. Method `CreatePrize` loads the file, reads lines into the least, and convert strings to `PrizeModel` instances. The method also receives last `id` from the file, and adds +1 to create id for the new record *(SQL has autoincrement option)*. Is the file is empty, `id = 1`. Then the method creates a new instance with the id, makes a string of the instance `PrizeModel`, and writes the string down to the end of the file. The method returns curren `model` instance that can be used by other models.

Methods that `TextConnector` uses are implemented in `class TestConnectorProsessor`. The class has own `TextHelpers` namespace. Method `FullFilePath` returns full path to the file. Extention method `LoadFile` checks if the file already exits and reads all the lines into list of strings. `ConvertToPrizeModels` extention method splits every string into array of strings, and array members are assigned to propeties of a new `PrizeModel` instance. The method returns the list of `PrizeModel` instances. Extention method `SaveToPrizeFile` receives list of instances `PrizeModel`, turn them in strings, and write down the whole list to the file. 

4.2. Prize form<br/>

We create a reward for winning places in the contest. The program checks the correct input in every fields, and then saves data to both database and txt file.

<br/>bilde prize form<br/>

This form contains fields `Place Number`, `Place Name`, `Prize Amount`, `Prize Percentage` and `Create Prize` button.

`PrizeModel class` contains user's constructor that uses these field to initiate properties of the instance of the class. The constructor parses string values in the fileds, that will be saved later.

`CreatePrizeForm` form has a private method `ValidateForm()` that checks the correct input, and shows message *`Please, check the form!`* otherwise. Method `createPrizeButton_Click`, if the validation is succesfull, invokes user's constructor of `PrizeModel class` and creates an instance of a class. Then the instance is stored in according to `DatabaseType` from `GlobalConfig.Connections`. After successfull adding data, the method resets fields values.

4.3. Create form<br/>
The form creates new team player, select existing member, make a team, add a new team, delete selected.

<br/>bilde prize form<br/>

Form contains fields 


in Idataconnection add createPerson
implement it im sql and txt





### <a href='#04' id='04' class='anchor' aria-hidden='true'>4.0. </a>
### <a href='#05' id='05' class='anchor' aria-hidden='true'>5.0. </a>
### <a href='#06' id='06' class='anchor' aria-hidden='true'>6.0. </a>