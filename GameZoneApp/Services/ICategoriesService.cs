namespace GameZoneApp.Services
{
    public interface ICategoriesService
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}
