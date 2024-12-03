namespace GameZoneApp.Services
{
    public class CategoriesService(AppDbContext context) : ICategoriesService
    {
        private readonly AppDbContext _context = context;


        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
                .OrderBy(c => c.Text)
                .AsNoTracking()
                .ToList();
        }
    }
}
