
namespace FrontAPI.Data
{
    public class StateDTO
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string RestaurantName { get; set; } = null!;
        public double PhoneNumber { get; set; }
        public string Cuisine { get; set; } = null!;
        public int? totLocations { get; set; } = null!;
        #endregion
    }
}
