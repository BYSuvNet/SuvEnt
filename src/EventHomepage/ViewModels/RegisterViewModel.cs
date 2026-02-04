using System.ComponentModel.DataAnnotations;

namespace EventHomepage.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string ParticipantName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters")]
    public string ParticipantEmail { get; set; } = string.Empty;
}
