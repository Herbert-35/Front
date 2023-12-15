using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace FrontAPI.Data.Models
{
    [Table("States")]
    [Index(nameof(Name))]
    [Index(nameof(RestaurantName))]
    [Index(nameof(PhoneNumber))]
    [Index(nameof(Cuisine))]
    public class State
    {
        #region Properties
        /// <summary>   
        /// The unique id and primary key for this State
        /// </summary>

        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// State Name
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Restaurant Name
        /// </summary>
        public string RestaurantName { get; set; } = null!;

        /// <summary>
        /// Restaurants Phone Number
        /// </summary>
        public double PhoneNumber { get; set; }

        /// <summary>
        /// The type of cuisine served
        /// </summary>
        public string Cuisine { get; set; } = null!;
        #endregion

        #region Navigation Properties
        /// <summary>
        /// A collection of all the locations related to this State.
        /// </summary>
        public ICollection<Location>? Locations { get; set; } = null!;
        #endregion
    }
}