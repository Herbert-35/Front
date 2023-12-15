using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FrontAPI.Data.Models
{
    [Table("Locations")]
    [Index(nameof(Name))]
    [Index(nameof(ZipCode))]
    [Index(nameof(StreetAddress))]
    public class Location
    {
        #region Properties
        /// <summary>
        /// The unique id and primary key for this Location
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// City Name
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Zip Code
        /// </summary>
        public int ZipCode { get; set; }

        /// <summary>
        /// Street Address
        /// </summary>
        public string StreetAddress { get; set; } = null!;

        ///<summary>
        ///State Id (foreign key)
        /// </summary>
        [ForeignKey(nameof(State))]
        public int StateId {  get; set; }
        #endregion

        #region Navigation Properties
        /// <summary>
        /// The State related to this location.
        /// </summary>
        public State? State { get; set; } = null!;
        #endregion
    }
}
