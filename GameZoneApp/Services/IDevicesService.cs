namespace GameZoneApp.Services
{
    public interface IDevicesService
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}
