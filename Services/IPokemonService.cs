using PokemonMVC.Models;

namespace PokemonMVC.Services
{
    /// <summary>
    /// Interface del servicio de Pokémon - Capa de lógica de negocio
    /// </summary>
    public interface IPokemonService
    {
        Task<PokemonListViewModel> GetPokemonListAsync(int page, int limit);
        Task<Pokemon?> GetPokemonDetailsAsync(int id);
    }
}
