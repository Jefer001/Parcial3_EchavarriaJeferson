using System.ComponentModel.DataAnnotations;

namespace WashingCar.DAL.Entities
{
    public class Vehicle : Entity
    {
        #region Properties
        [Display(Name = "Vehiculo")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Propietario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Owner { get; set; }

        [Display(Name = "Número de Placa")]
        [MaxLength (6)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string NumberPlate { get; set; }

        [Display(Name = "Servicio.")]
        public Service Service { get; set; }
        #endregion
    }
}
