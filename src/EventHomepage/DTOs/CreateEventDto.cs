namespace EventHomepage.DTOs;

public record CreateEventDto(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Location,
    int? MaxParticipants
);
