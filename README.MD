#### Hi! This is a Contest Winner solution (.Net, WinForms, SQL) 

1.0. [Solution requirements.](#01)<br/>
	1.1. General specifics.<br/>
	1.2. Detailed specifics.<br/>
	1.3. Solution Concepts.<br/>
2.0. [Data Design.](#02)<br/>
	2.1. Classes.<br/>
3.0. [  ](#03)<br/>
4.0. [  ](#04)<br/>
5.0. [  ](#05)<br/>

#### <a href='#01' id='01' class='anchor' aria-hidden='true'>1.0. Solution requirements.</a>

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

#### <a href='#02' id='02' class='anchor' aria-hidden='true'>2.0. Data Design.</a>

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

- Prize
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

#### 3.0. 1:50





#### <a href='#03' id='03' class='anchor' aria-hidden='true'>3.0. </a>
#### <a href='#04' id='04' class='anchor' aria-hidden='true'>4.0. </a>
#### <a href='#05' id='05' class='anchor' aria-hidden='true'>5.0. </a>
#### <a href='#06' id='06' class='anchor' aria-hidden='true'>6.0. </a>