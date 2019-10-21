using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public const string PrizesFile = "PrizeModels.csv"; //pascale case because const
        public const string PeopleFile = "PersonModels.csv";
        public const string TeamFile = "TeamModels.csv";
        public const string TournamentFile = "TournamentModels.csv";
        public const string MatchupFile = "MatchupModels.csv";
        public const string MatchupEntryFile = "MatchupEntryModels.csv";

        // we create the list to save both in database and txt file
        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections (DatabaseType db)
        {
            if (db == DatabaseType.Sql)
            {
                //TODO set up SQL connector properly
                SqlConnector sql = new SqlConnector();
                Connection = sql;
            }

            else if (db == DatabaseType.TextFile) //exactly one or another
            {
                //TODO text connection
                TextConnector text = new TextConnector();
                Connection = text;
            }
        }

        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
