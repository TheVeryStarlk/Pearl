using System.ComponentModel.DataAnnotations;

namespace Pearl.Database.Models;

public sealed class User
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public byte[] Hash { get; set; } = null!;

    [Required]
    public byte[] Salt { get; set; } = null!;

    [Required]
    public List<Group> Groups { get; set; } = null!;
}