using Microsoft.AspNetCore.Mvc;
using PokemonMVC.Services;

namespace PokemonMVC.Controllers
{
    /// <summary>
    /// Controlador de Pokémon - Maneja las peticiones HTTP
    /// </summary>
    public class PokemonController : Controller
    {
        private readonly IPokemonService _pokemonService;
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(IPokemonService pokemonService, ILogger<PokemonController> logger)
        {
            _pokemonService = pokemonService ?? throw new ArgumentNullException(nameof(pokemonService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Muestra la lista de pokemones con paginación
        /// </summary>
        public async Task<IActionResult> Index(int page = 1, int limit = 20)
        {
            try
            {
                var viewModel = await _pokemonService.GetPokemonListAsync(page, limit);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la lista de pokemones");
                return View("Error");
            }
        }

        /// <summary>
        /// Muestra los detalles de un pokémon específico
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest("ID inválido");
                }

                var pokemon = await _pokemonService.GetPokemonDetailsAsync(id);
                
                if (pokemon == null)
                {
                    return NotFound($"No se encontró el pokémon con ID: {id}");
                }

                return View(pokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar detalles del pokémon {Id}", id);
                return View("Error");
            }
        }


    }
}
