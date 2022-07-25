using System.ComponentModel.DataAnnotations;

namespace Pearl.Database.Models;

public sealed class Message
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Content { get; set; } = null!;

    [Required]
    public Group Group { get; set; } = null!;

    [Required]
    public User User { get; set; } = null!;
}