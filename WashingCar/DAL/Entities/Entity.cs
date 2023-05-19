using System.ComponentModel.DataAnnotations;

namespace WashingCar.DAL.Entities
{
    public class Entity
    {
        #region Properties
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Fecha de entrada del vehículo")]
        public DateTime? CreationDate { get; set; }

        [Display(Name = "Fecha de entrega del vehículo")]
        public DateTime? DeliveryDate { get; set; }
        #endregion
    }
}
