using System.ComponentModel.DataAnnotations;

namespace Pearl.Database.Models;

public sealed class Group
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
}