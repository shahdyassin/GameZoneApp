
namespace GameZoneApp.Services
{
    public class GamesService : IGamesService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        public GamesService(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";

        }
        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(d => d.Device)
                .AsNoTracking()
                .ToList();
        }
        public Game? GetById(int id)
        {
            return _context.Games
               .Include(g => g.Category)
               .Include(g => g.Devices)
               .ThenInclude(d => d.Device)
               .AsNoTracking()
               .SingleOrDefault(g => g.Id == id);
              
        }

        public async Task Create(CreateGameFormViewModel viewModel)
        {
            var coverName = await SaveCover(viewModel.Cover);
            //stream.Dispose();

            Game game = new()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId,
                Cover = coverName,
                Devices = viewModel.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList(),
            };
            _context.Add(game);
            _context.SaveChanges();
        }

        public async Task<Game?> Update(UpdateGameFormViewModel viewModel)
        {
            var game = _context.Games
                .Include(g => g.Devices)
                .SingleOrDefault(g => g.Id == viewModel.Id);
            if (game == null)
            {
                return null;
            }
            var hasNewCover = viewModel.Cover != null;
            var oldCover = game.Cover;

            game.Name = viewModel.Name;
            game.Description = viewModel.Description;
            game.CategoryId = viewModel.CategoryId;
            game.Devices = viewModel.SelectedDevices.Select(d => new GameDevice {  DeviceId = d }).ToList();

            if(hasNewCover)
            {
                game.Cover = await SaveCover(viewModel.Cover!);  
            }
            var effectedRows = _context.SaveChanges();

            if(effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }
                return game;
            }
            else
            {
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);

                return null;
            }

        }
        public bool Delete(int id)
        {
            var isDeleted =false;

            var game = _context.Games.Find(id);

            if (game == null)
            {
                return isDeleted;
            }
            _context.Games.Remove(game);
            var effectedRows = _context.SaveChanges();
            if (effectedRows > 0)
            {
                isDeleted = true;
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
            }

            return isDeleted;
        }
        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);

            return coverName;
        }

       
    }
}
