using EmailPush.Application.DTOs;
using EmailPush.Application.Commands;
using EmailPush.Domain.Entities;

namespace EmailPush.Application.Utils;

/// <summary>
/// Converts campaigns to/from DTOs
/// </summary>
public static class CampaignMapper
{
    /// <summary>
    /// Converts campaign to DTO
    /// </summary>
    public static CampaignDto ToDto(Campaign campaign)
    {

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
        return campaigns.Select(ToDto).ToList();
    }

    /// <summary>
    /// Maps CreateCampaignDto to Campaign entity
    /// </summary>
    /// <param name="dto">CreateCampaignDto to map</param>
    /// <returns>Mapped Campaign entity</returns>
    public static Campaign FromCreateDto(CreateCampaignDto dto)
    {

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
    /// Maps CreateCampaignCommand to Campaign entity
    /// </summary>
    /// <param name="command">CreateCampaignCommand to map</param>
    /// <returns>Mapped Campaign entity</returns>
    public static Campaign FromCreateCommand(CreateCampaignCommand command)
    {
        return new Campaign
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Subject = command.Subject,
            Content = command.Content,
            Recipients = command.Recipients,
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

        existingCampaign.Name = updateDto.Name;
        existingCampaign.Subject = updateDto.Subject;
        existingCampaign.Content = updateDto.Content;
        existingCampaign.Recipients = updateDto.Recipients;
    }

    /// <summary>
    /// Updates existing Campaign entity with UpdateCampaignCommand data
    /// </summary>
    /// <param name="existingCampaign">Existing campaign to update</param>
    /// <param name="updateCommand">Update command data</param>
    public static void UpdateFromCommand(Campaign existingCampaign, UpdateCampaignCommand updateCommand)
    {
        existingCampaign.Name = updateCommand.Name;
        existingCampaign.Subject = updateCommand.Subject;
        existingCampaign.Content = updateCommand.Content;
        existingCampaign.Recipients = updateCommand.Recipients;
    }

    /// <summary>
    /// Updates existing Campaign entity with UpdateCampaignDto data (partial update - PATCH)
    /// </summary>
    /// <param name="existingCampaign">Existing campaign to update</param>
    /// <param name="updateDto">Partial update data</param>
    public static void PatchFromDto(Campaign existingCampaign, UpdateCampaignDto updateDto)
    {
        if (!string.IsNullOrEmpty(updateDto.Name))
            existingCampaign.Name = updateDto.Name;
            
        if (!string.IsNullOrEmpty(updateDto.Subject))
            existingCampaign.Subject = updateDto.Subject;
            
        if (!string.IsNullOrEmpty(updateDto.Content))
            existingCampaign.Content = updateDto.Content;
            
        if (updateDto.Recipients != null)
            existingCampaign.Recipients = updateDto.Recipients;
    }

    /// <summary>
    /// Updates existing Campaign entity with PatchCampaignCommand data (partial update - PATCH)
    /// </summary>
    /// <param name="existingCampaign">Existing campaign to update</param>
    /// <param name="patchCommand">Partial update command data</param>
    public static void PatchFromCommand(Campaign existingCampaign, PatchCampaignCommand patchCommand)
    {
        if (!string.IsNullOrEmpty(patchCommand.Name))
            existingCampaign.Name = patchCommand.Name;
            
        if (!string.IsNullOrEmpty(patchCommand.Subject))
            existingCampaign.Subject = patchCommand.Subject;
            
        if (!string.IsNullOrEmpty(patchCommand.Content))
            existingCampaign.Content = patchCommand.Content;
            
        if (patchCommand.Recipients != null)
            existingCampaign.Recipients = patchCommand.Recipients;
    }
}