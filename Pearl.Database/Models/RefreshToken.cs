using System.ComponentModel.DataAnnotations;

namespace Pearl.Database.Models;

public sealed class RefreshToken
{
    [Required]
    public int Id { get; set; }

    [Required]
    public User User { get; set; } = null!;

    [Required]
    public string Value { get; set; } = null!;

    [Required]
    public DateTime ExpiryDate { get; set; }
}