namespace PokemonMVC.Models
{
    /// <summary>
    /// ViewModel para la vista de listado de pokemones
    /// </summary>
    public class PokemonListViewModel
    {
        public List<PokemonSummary> Pokemons { get; set; } = new();
        public int CurrentOffset { get; set; }
        public int Limit { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public int CurrentPage => (CurrentOffset / Limit) + 1;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / Limit);
    }

    public class PokemonSummary
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
