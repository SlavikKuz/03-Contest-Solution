using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "First Name")]
        [StringLength(20, MinimumLength = 2)]
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Person's last name
        /// </summary>
        [Display(Name = "Last Name")]
        [StringLength(20, MinimumLength = 2)]
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Person's email
        /// </summary>
        [Display(Name = "Email")]
        ///regex possibly
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Person's cell number
        /// </summary>
        [Display(Name = "Tel.")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(14, MinimumLength = 8)]
        [Required]
        public string CellPhoneNumber { get; set; }

        /// <summary>
        /// to avoid name duplicates
        /// </summary>
        public string FullName
        {
            get { return $"{ FirstName } { LastName }"; }
        }

    }
}
