using System.ComponentModel.DataAnnotations;

namespace WashingCar.DAL.Entities
{
    public class Entity
    {
        #region Properties
        [Key]
        public Guid Id { get; set; }
        #endregion
    }
}
