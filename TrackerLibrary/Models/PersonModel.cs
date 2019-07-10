using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represents one person
    /// </summary>
    public class PersonModel
    {
        /// <summary>
        /// Unique id for the person
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Person's first name
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Person's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Person's email
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Person's cell number
        /// </summary>
        public string CellPhoneNumber { get; set; }
    }
}
