using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WashingCar.DAL.Entities
{
    public class Service : Entity
    {
        #region Properties
        [Display(Name = "Servicio.")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }

        [Display(Name = "Vehiculos")]
        public ICollection<Vehicle> Vehicles { get; set; }

        [Display(Name = "Número de vehiculos")]
        public int VehicleNumber => Vehicles == null ? 0 : Vehicles.Count;
        #endregion
    }
}
