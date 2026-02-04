namespace EventHomepage.ViewModels;

public class EventListViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public int RegistrationCount { get; set; }
    public int? MaxParticipants { get; set; }
    public bool IsFull { get; set; }
}
