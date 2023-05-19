using WashingCar.DAL.Entities;

namespace WashingCar.Models
{
    public class VehicleViewModel : Vehicle
    {
        #region Properties
        public Guid ServiceId { get; set; }
        #endregion
    }
}
