using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        /// <summary>
        /// saves a new prize to the DB
        /// </summary>
        /// <param name="model">prize info</param>
        /// <returns>The prize info, including the unique identifier</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // new connection which relizes interface, in which we can have code behind
            // is a MS sql connection
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("ContestSolution")))
            {
                var p = new DynamicParameters(); //dapper
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                //do stored procedure with ... and get back id
                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id"); //looks at p, finds id, gets id int value

                return model;
            }
        }
    }
}
