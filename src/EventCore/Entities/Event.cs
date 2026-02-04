using EventCore.Exceptions;
namespace EventCore.Entities;

public class Event
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public string Location { get; private set; } = string.Empty;
    public int? MaxParticipants { get; private set; }

    private readonly List<Registration> _registrations = [];
    public IReadOnlyCollection<Registration> Registrations => _registrations;

    private Event() { } // Behövs för EF Core

    public Event(string name, string description, DateTime startDateTime, DateTime endDateTime, string location, int? maxParticipants = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty");
        }

        if (endDateTime <= startDateTime)
        {
            throw new ArgumentException("EndDateTime must be after StartDateTime");
        }

        if (string.IsNullOrWhiteSpace(location))
        {
            throw new ArgumentException("Location cannot be empty");
        }

        if (maxParticipants.HasValue && maxParticipants <= 0)
        {
            throw new ArgumentException("MaxParticipants must be greater than zero if specified");
        }

        Name = name;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        Location = location;
        MaxParticipants = maxParticipants;
    }

    public bool IsFull()
    {
        if (MaxParticipants.HasValue)
        {
            return _registrations.Count >= MaxParticipants.Value;
        }
        return false;
    }

    public Registration RegisterParticipant(string participantName, string participantEmail)
    {
        if (IsFull())
        {
            throw new EventIsFullException();
        }

        if (_registrations.Any(r => r.ParticipantEmail.Equals(participantEmail, StringComparison.OrdinalIgnoreCase)))
        {
            throw new DuplicateRegistrationException();
        }

        var registration = new Registration(this, participantName, participantEmail);
        _registrations.Add(registration);
        return registration;
    }
}

