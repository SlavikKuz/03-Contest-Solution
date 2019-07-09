using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represents what the prize is for the given place
    /// </summary>
    public class PrizeModel
    {
        /// <summary>
        /// Unique id for thr prize
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// the numeric identifier for the place (2 - second, etc)
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// a good name for the place (champion, first, etc)
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// fixed amount of prize or 0, if not used
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// Percentage of prize, fraction 50% is 0.5, or 0 if not used
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;

            int placeNumberValue = 0;
            int.TryParse(placeNumber, out placeNumberValue);
            PlaceNumber = placeNumberValue;

            decimal prizeAmountValue = 0;
            decimal.TryParse(prizeAmount, out prizeAmountValue);
            PrizeAmount = prizeAmountValue;

            double prizePercentageValue = 0;
            double.TryParse(prizePercentage, out prizePercentageValue);
            PrizePercentage = prizePercentageValue;
        }
    }
}
