namespace EmailPush.Domain.Services;

public interface ICampaignDomainService
{
    Task<bool> CanCreateCampaignAsync(string name);
    Task<bool> IsNameUniqueAsync(string name, Guid? excludeId = null);
    bool HasValidEmailContent(string subject, string content);
    Task<TimeSpan> EstimatedSendingTimeAsync(int recipientCount);
}