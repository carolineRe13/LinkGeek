using System.ComponentModel.DataAnnotations;
using LinkGeek.AppIdentity;

namespace LinkGeek.Models;

public class Comment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public string Text { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}