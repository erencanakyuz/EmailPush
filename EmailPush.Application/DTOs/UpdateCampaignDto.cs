namespace EmailPush.Application.DTOs;

/// <summary>
/// DTO for partial campaign updates (PATCH operations)
/// </summary>
public class UpdateCampaignDto
{
    public string? Name { get; set; }
    public string? Subject { get; set; }
    public string? Content { get; set; }
    public List<string>? Recipients { get; set; }
}