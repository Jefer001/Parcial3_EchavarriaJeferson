using System.ComponentModel.DataAnnotations;

namespace WashingCar.DAL.Entities
{
    public class Vehicle : Entity
    {
        #region Properties
        [Display(Name = "Vehiculo")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Name { get; set; }

        [Display(Name = "Propietario")]
        public string? Owner { get; set; }

        [Display(Name = "Número de Placa")]
        public string? NumberPlate { get; set; }

        [Display(Name = "Servicio.")]
        public Service? Service { get; set; }

        [Display(Name = "Detalles del vehículo")]
        public ICollection<VehicleDetail>? VehicleDetails { get; set; }
        #endregion
    }
}
