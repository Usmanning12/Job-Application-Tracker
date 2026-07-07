using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Application
{
    public int Id { get; set; }

    [Required]
    public string Company { get; set; } = string.Empty;

    [Required]
    public string Position { get; set; } = string.Empty;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

    public DateTime AppliedDate { get; set; } = DateTime.Now;
}