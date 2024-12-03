namespace GameZoneApp.Services
{
    public class DevicesService(AppDbContext context) : IDevicesService
    {
        private readonly AppDbContext _context = context;
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _context.Devices.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            })
                .OrderBy(d => d.Text)
                .AsNoTracking()
                .ToList();
        }
    }
}
