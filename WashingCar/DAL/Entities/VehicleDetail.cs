using System.ComponentModel.DataAnnotations;

namespace WashingCar.DAL.Entities
{
    public class VehicleDetail : Entity
    {
        #region Properties
        [Display(Name = "Detalles del vehículo")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Name { get; set; }

        [Display(Name = "Fecha de entrada del vehículo")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Fecha de Entrega del vehículo")]
        public DateTime DeliveryDat { get; set; }

        [Display(Name = "Vehiculo")]
        public Vehicle? Vehicle { get; set; }
        #endregion
    }
}
