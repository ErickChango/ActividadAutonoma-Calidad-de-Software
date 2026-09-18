using PokemonMVC.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PokemonMVC.Services
{
    /// <summary>
    /// Servicio de Pokémon - Implementa la lógica de negocio y acceso a datos
    /// </summary>
    public class PokemonService : IPokemonService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PokemonService> _logger;

        public PokemonService(HttpClient httpClient, ILogger<PokemonService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PokemonListViewModel> GetPokemonListAsync(int page, int limit)
        {
            try
            {
                // Validación de parámetros
                page = Math.Max(1, page);
                limit = Math.Clamp(limit, 1, 50);

                var offset = (page - 1) * limit;
                
                // Validación de entrada para prevenir ataques
                if (offset < 0 || limit < 1 || limit > 100)
                {
                    _logger.LogWarning("Parámetros inválidos: offset={Offset}, limit={Limit}", offset, limit);
                    return new PokemonListViewModel();
                }

                var response = await _httpClient.GetAsync($"pokemon?offset={offset}&limit={limit}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);
                var root = json.RootElement;

                var listResponse = new PokemonListResponse
                {
                    Count = root.GetProperty("count").GetInt32(),
                    Next = root.TryGetProperty("next", out var next) ? next.GetString() : null,
                    Previous = root.TryGetProperty("previous", out var prev) ? prev.GetString() : null,
                    Results = root.GetProperty("results").EnumerateArray()
                        .Select(r => new PokemonListItem
                        {
                            Name = r.GetProperty("name").GetString() ?? "",
                            Url = r.GetProperty("url").GetString() ?? ""
                        }).ToList()
                };

                var pokemons = new List<PokemonSummary>();
                foreach (var item in listResponse.Results)
                {
                    // Extraer el ID de la URL
                    var segments = item.Url.TrimEnd('/').Split('/');
                    if (int.TryParse(segments[^1], out int id))
                    {
                        pokemons.Add(new PokemonSummary
                        {
                            Id = id,
                            Name = item.Name,
                            ImageUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/{id}.png"
                        });
                    }
                }

                return new PokemonListViewModel
                {
                    Pokemons = pokemons,
                    CurrentOffset = offset,
                    Limit = limit,
                    TotalCount = listResponse.Count,
                    HasPrevious = !string.IsNullOrEmpty(listResponse.Previous),
                    HasNext = !string.IsNullOrEmpty(listResponse.Next)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener lista de pokemones");
                return new PokemonListViewModel();
            }
        }

        public async Task<Pokemon?> GetPokemonDetailsAsync(int id)
        {
            try
            {
                // Validación de entrada
                if (id < 1 || id > 10000)
                {
                    _logger.LogWarning("ID de pokemon inválido: {Id}", id);
                    return null;
                }

                var response = await _httpClient.GetAsync($"pokemon/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return ParsePokemonJson(content);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al obtener pokemon con ID: {Id}", id);
                return null;
            }
        }


        private Pokemon ParsePokemonJson(string json)
        {
            var data = JsonDocument.Parse(json).RootElement;

            // Intentar obtener la imagen oficial, si no existe usar la por defecto
            string imageUrl = "";
            try
            {
                if (data.GetProperty("sprites").TryGetProperty("other", out var other) &&
                    other.TryGetProperty("official-artwork", out var artwork) &&
                    artwork.TryGetProperty("front_default", out var officialImage))
                {
                    imageUrl = officialImage.GetString() ?? "";
                }
            }
            catch
            {
                // Si falla, intentar con la imagen por defecto
                if (data.GetProperty("sprites").TryGetProperty("front_default", out var defaultImage))
                {
                    imageUrl = defaultImage.GetString() ?? "";
                }
            }

            // Si aún no hay imagen, usar un placeholder basado en el ID
            if (string.IsNullOrEmpty(imageUrl))
            {
                var id = data.GetProperty("id").GetInt32();
                imageUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{id}.png";
            }

            return new Pokemon
            {
                Id = data.GetProperty("id").GetInt32(),
                Name = data.GetProperty("name").GetString() ?? "",
                Height = data.GetProperty("height").GetInt32(),
                Weight = data.GetProperty("weight").GetInt32(),
                ImageUrl = imageUrl,
                Types = data.GetProperty("types").EnumerateArray()
                    .Select(t => new PokemonType
                    {
                        Name = t.GetProperty("type").GetProperty("name").GetString() ?? ""
                    }).ToList(),
                Abilities = data.GetProperty("abilities").EnumerateArray()
                    .Select(a => new PokemonAbility
                    {
                        Name = a.GetProperty("ability").GetProperty("name").GetString() ?? ""
                    }).ToList(),
                Stats = new PokemonStats
                {
                    Hp = data.GetProperty("stats")[0].GetProperty("base_stat").GetInt32(),
                    Attack = data.GetProperty("stats")[1].GetProperty("base_stat").GetInt32(),
                    Defense = data.GetProperty("stats")[2].GetProperty("base_stat").GetInt32(),
                    SpecialAttack = data.GetProperty("stats")[3].GetProperty("base_stat").GetInt32(),
                    SpecialDefense = data.GetProperty("stats")[4].GetProperty("base_stat").GetInt32(),
                    Speed = data.GetProperty("stats")[5].GetProperty("base_stat").GetInt32()
                }
            };
        }
    }
}
