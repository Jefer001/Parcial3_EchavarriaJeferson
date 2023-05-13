using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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
