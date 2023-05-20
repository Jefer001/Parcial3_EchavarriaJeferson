using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WashingCar.DAL.Entities;
using WashingCar.Utilities;

namespace WashingCar.Models
{
    public class AddVehicleViewModel : Vehicle
    {
        #region Properties
        [Display(Name = "Servicio")]
        [NonEmptyGuid(ErrorMessage = "Debes de seleccionar un servicio.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid ServiceId { get; set; }

        public IEnumerable<SelectListItem> Services { get; set; }
        #endregion
    }
}
