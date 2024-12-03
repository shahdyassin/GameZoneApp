using Microsoft.AspNetCore.Mvc;

namespace GameZoneApp.Controllers
{
    public class GamesController(ICategoriesService categoriesService, 
        IDevicesService devicesService, 
        IGamesService gamesService) : Controller
    {
        private readonly ICategoriesService _categoriesService = categoriesService;
        private readonly IDevicesService _devicesService = devicesService;
        private readonly IGamesService _gamesService = gamesService;
        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }
        public IActionResult Details(int id)
        {
            var game = _gamesService.GetById(id);
            if(game == null)
            {
                return NotFound();
            }
            return View(game);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewModel = new()
            {
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList()

            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _categoriesService.GetSelectList();
                viewModel.Devices = _devicesService.GetSelectList();
                return View(viewModel);
            }
            await _gamesService.Create(viewModel);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {

            var game = _gamesService.GetById(id);
            if (game == null)
            {
                return NotFound();
            }
            UpdateGameFormViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList(),
                CurrentCover = game.Cover,
            };
            return View(viewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateGameFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _categoriesService.GetSelectList();
                viewModel.Devices = _devicesService.GetSelectList();
                return View(viewModel);
            }
            var game = await _gamesService.Update(viewModel);

            if(game == null)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
           // return BadRequest();
            var isDeleted = _gamesService.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }
    }
}
