using EmailPush.Application.DTOs;
using EmailPush.Domain.Entities;

namespace EmailPush.Application.Utils;

/// <summary>
/// Campaign mapping utility class
/// Handles conversion between Campaign entities and DTOs
/// </summary>
public static class CampaignMapper
{
    /// <summary>
    /// Maps Campaign entity to CampaignDto
    /// </summary>
    /// <param name="campaign">Campaign entity to map</param>
    /// <returns>Mapped CampaignDto</returns>
    public static CampaignDto ToDto(Campaign campaign)
    {
        ArgumentNullException.ThrowIfNull(campaign);

        return new CampaignDto
        {
            Id = campaign.Id,
            Name = campaign.Name,
            Subject = campaign.Subject,
            Content = campaign.Content,
            Recipients = campaign.Recipients,
            Status = campaign.Status.ToString(),
            CreatedAt = campaign.CreatedAt,
            StartedAt = campaign.StartedAt,
            SentCount = campaign.SentCount
        };
    }

    /// <summary>
    /// Maps multiple Campaign entities to CampaignDto list
    /// </summary>
    /// <param name="campaigns">Campaign entities to map</param>
    /// <returns>List of mapped CampaignDto objects</returns>
    public static List<CampaignDto> ToDtoList(IEnumerable<Campaign> campaigns)
    {
        ArgumentNullException.ThrowIfNull(campaigns);
        return campaigns.Select(ToDto).ToList();
    }

    /// <summary>
    /// Maps CreateCampaignDto to Campaign entity
    /// </summary>
    /// <param name="dto">CreateCampaignDto to map</param>
    /// <returns>Mapped Campaign entity</returns>
    public static Campaign FromCreateDto(CreateCampaignDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Campaign
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Subject = dto.Subject,
            Content = dto.Content,
            Recipients = dto.Recipients,
            Status = CampaignStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            SentCount = 0
        };
    }

    /// <summary>
    /// Updates existing Campaign entity with CreateCampaignDto data
    /// </summary>
    /// <param name="existingCampaign">Existing campaign to update</param>
    /// <param name="updateDto">Update data</param>
    public static void UpdateFromDto(Campaign existingCampaign, CreateCampaignDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(existingCampaign);
        ArgumentNullException.ThrowIfNull(updateDto);

        existingCampaign.Name = updateDto.Name;
        existingCampaign.Subject = updateDto.Subject;
        existingCampaign.Content = updateDto.Content;
        existingCampaign.Recipients = updateDto.Recipients;
    }
}