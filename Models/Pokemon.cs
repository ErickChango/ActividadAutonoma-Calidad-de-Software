using System.ComponentModel.DataAnnotations;

namespace PokemonMVC.Models
{
    /// <summary>
    /// Modelo que representa un Pokémon
    /// </summary>
    public class Pokemon
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public int Height { get; set; }

        public int Weight { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public List<PokemonType> Types { get; set; } = new();

        public List<PokemonAbility> Abilities { get; set; } = new();

        public PokemonStats Stats { get; set; } = new();
    }

    public class PokemonType
    {
        public string Name { get; set; } = string.Empty;
    }

    public class PokemonAbility
    {
        public string Name { get; set; } = string.Empty;
    }

    public class PokemonStats
    {
        public int Hp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int Speed { get; set; }
    }
}
