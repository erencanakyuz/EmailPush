using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;

namespace EmailPush.Domain.Services;

public interface ICampaignDomainService
{
    Task<bool> CanCreateCampaignAsync(string name);
    Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null);
    bool HasValidEmailContent(string subject, string content);
    Task<TimeSpan> EstimatedSendingTimeAsync(int recipientCount);
}

public class CampaignDomainService : ICampaignDomainService
{
    private readonly ICampaignRepository _campaignRepository;

    public CampaignDomainService(ICampaignRepository campaignRepository)
    {
        _campaignRepository = campaignRepository;
    }

    public async Task<bool> CanCreateCampaignAsync(string name)
    {
        return await IsNameUniqueAsync(name);
    }

    public async Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null)
    {
        var campaigns = await _campaignRepository.GetAllAsync();
        return !campaigns.Any(c => 
            c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
            (!excludeId.HasValue || c.Id != excludeId.Value));
    }

    public bool HasValidEmailContent(string subject, string content)
    {
        // Business rule: Content should not be empty and should have reasonable length
        if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(content))
            return false;

        // Business rule: Subject should be concise
        if (subject.Length > 500)
            return false;

        // Business rule: Content should have meaningful content (more than just whitespace)
        if (content.Trim().Length < 10)
            return false;

        return true;
    }

    public async Task<TimeSpan> EstimatedSendingTimeAsync(int recipientCount)
    {
        // Business rule: Estimate 1 second per 10 emails (rate limiting)
        var estimatedSeconds = Math.Ceiling(recipientCount / 10.0);
        return TimeSpan.FromSeconds(estimatedSeconds);
    }
}