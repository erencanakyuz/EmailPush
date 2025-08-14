using EmailPush.Application.DTOs;
using EmailPush.Domain.Entities;

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
    /// Gets campaigns by status
    /// </summary>
    Task<List<CampaignDto>> GetCampaignsByStatusAsync(CampaignStatus status);

    /// <summary>
    /// Creates a new campaign
    /// </summary>
    Task<CampaignDto> CreateAsync(CreateCampaignDto dto);

    /// <summary>
    /// Updates campaign (draft only) - Full replacement
    /// </summary>
    Task<CampaignDto?> UpdateAsync(Guid id, CreateCampaignDto dto);

    /// <summary>
    /// Partially updates campaign (draft only) - PATCH semantics
    /// </summary>
    Task<CampaignDto?> PatchAsync(Guid id, UpdateCampaignDto dto);


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
