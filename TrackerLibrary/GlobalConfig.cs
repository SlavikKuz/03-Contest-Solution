using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        // we create the list to save both in database and txt file
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

        public static void InitializeConnections (bool database, bool textFiles)
        {
            if (database)
            {
                //TODO set up SQL connector properly
                SqlConnector sql = new SqlConnector();
                Connections.Add(sql);
            }

            if (textFiles)
            {
                //TODO text connection
                TextConnection text = new TextConnection();
                Connections.Add(text);
            }
        }

    }
}
