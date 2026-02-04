namespace EventHomepage.ViewModels;

public class EventDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public int? MaxParticipants { get; set; }
    public int RegistrationCount { get; set; }
    public bool IsFull { get; set; }
    public RegisterViewModel Registration { get; set; } = new();
}
