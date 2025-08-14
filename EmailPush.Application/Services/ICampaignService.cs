using EmailPush.Application.DTOs;

namespace EmailPush.Application.Services;

/// <summary>
/// Service for managing email campaigns
/// </summary>
public interface ICampaignService
{
    /// <summary>
    /// Gets campaign by ID
    /// </summary>
    Task<CampaignDto?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all campaigns
    /// </summary>
    Task<List<CampaignDto>> GetAllAsync();

    /// <summary>
    /// Creates a new campaign
    /// </summary>
    Task<CampaignDto> CreateAsync(CreateCampaignDto dto);

    /// <summary>
    /// Updates campaign (draft only)
    /// </summary>
    Task<CampaignDto?> UpdateAsync(Guid id, CreateCampaignDto dto);

    /// <summary>
    /// Deletes campaign (draft only)
    /// </summary>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Starts campaign sending
    /// </summary>
    Task<bool> StartSendingAsync(Guid id);

    /// <summary>
    /// Gets campaign stats
    /// </summary>
    Task<CampaignStatsDto> GetStatsAsync();
}
