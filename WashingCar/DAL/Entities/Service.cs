using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WashingCar.DAL.Entities
{
    public class Service : Entity
    {
        #region Properties
        [Display(Name = "Servicio.")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Name { get; set; }

        [Display(Name = "Precio.")]
        public decimal Price { get; set; }

        [Display(Name = "Vehicles")]
        public ICollection<Vehicle>? Vehicles { get; set; }

        [Display(Name = "Número de vehiculos")]
        public int VehicleNumber => Vehicles == null ? 0 : Vehicles.Count;
        #endregion
    }
}
