namespace GameZoneApp.Services
{
    public interface IGamesService
    {
        IEnumerable<Game> GetAll();
        Game? GetById(int id);
        Task Create(CreateGameFormViewModel viewModel);
        Task<Game?> Update(UpdateGameFormViewModel viewModel);
        bool Delete(int id);


    }
}
