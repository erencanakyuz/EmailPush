using System.ComponentModel.DataAnnotations;

namespace EmailPush.Application.DTOs;

/// <summary>
/// Campaign Data Transfer Object
/// Campaign information returned from API
/// </summary>

public class CampaignDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Recipients { get; set; } = new();
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public int TotalRecipients => Recipients.Count;
    public int SentCount { get; set; }
}


/// <summary>
/// DTO for creating new campaigns
/// </summary>

public class CreateCampaignDto
{

    /// <summary>
    /// Campaign name (maximum 200 characters)
    /// </summary>
    /// <example>Welcome Campaign</example>
    [Required(ErrorMessage = "Campaign name is required")]
    [StringLength(200, ErrorMessage = "Campaign name can be maximum 200 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email subject (maximum 500 characters)
    /// </summary>
    /// <example>Welcome to our platform!</example>
    [Required(ErrorMessage = "Email subject is required")]
    [StringLength(500, ErrorMessage = "Email subject can be maximum 500 characters")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Email content
    /// </summary>
    /// <example>Hello! Welcome to our platform. This is a test email campaign.</example>
    [Required(ErrorMessage = "Email content is required")]
    [MinLength(10, ErrorMessage = "Email content must be at least 10 characters")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// List of recipient email addresses (maximum 1000)
    /// </summary>
    /// <example>["test@example.com", "user@example.com"]</example>
    [Required(ErrorMessage = "At least one recipient is required")]
    [MinLength(1, ErrorMessage = "At least one recipient is required")]
    public List<string> Recipients { get; set; } = new();
}


/// <summary>
/// Campaign statistics
/// </summary>
public class CampaignStatsDto
{
    /// <summary>
    /// Total number of campaigns
    /// </summary>
    /// <example>25</example>
    public int TotalCampaigns { get; set; }
    
    /// <summary>
    /// Number of draft campaigns
    /// </summary>
    /// <example>5</example>
    public int DraftCampaigns { get; set; }
    
    /// <summary>
    /// Number of ready campaigns
    /// </summary>
    /// <example>3</example>
    public int ReadyCampaigns { get; set; }
    
    /// <summary>
    /// Number of campaigns currently sending
    /// </summary>
    /// <example>2</example>
    public int SendingCampaigns { get; set; }
    
    /// <summary>
    /// Number of completed campaigns
    /// </summary>
    /// <example>12</example>
    public int CompletedCampaigns { get; set; }
    
    /// <summary>
    /// Number of failed campaigns
    /// </summary>
    /// <example>3</example>
    public int FailedCampaigns { get; set; }
    
    /// <summary>
    /// Total emails sent across all campaigns
    /// </summary>
    /// <example>15420</example>
    public int TotalEmailsSent { get; set; }
    
    /// <summary>
    /// Total recipients across all campaigns
    /// </summary>
    /// <example>18500</example>
    public int TotalRecipients { get; set; }
}
