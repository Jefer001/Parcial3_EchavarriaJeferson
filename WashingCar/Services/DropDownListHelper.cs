using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WashingCar.DAL;
using WashingCar.Helpers;

namespace WashingCar.Services
{
    public class DropDownListHelper : IDropDownListHelper
    {
        #region Constants
        private readonly DataBaseContext _context;
        #endregion

        #region Builder
        public DropDownListHelper(DataBaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Public methods
        public async Task<IEnumerable<SelectListItem>> GetDDLServicesAsync()
        {
            List<SelectListItem> listServices = await _context.Services
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            listServices.Insert(0, new SelectListItem
            {
                Text = "Seleccione un servicio...",
                Value = Guid.Empty.ToString(),
                Selected = true
            });

            return listServices;
        }
        #endregion
    }
}
