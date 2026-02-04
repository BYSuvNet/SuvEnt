namespace EventHomepage.DTOs;

public record EventDto(
    int Id,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Location,
    int? MaxParticipants,
    int RegistrationCount,
    bool IsFull
);
