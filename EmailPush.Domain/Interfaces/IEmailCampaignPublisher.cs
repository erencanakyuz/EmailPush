using EmailPush.Domain.Entities;

namespace EmailPush.Domain.Interfaces;

public interface IEmailCampaignPublisher
{
    Task PublishCampaignAsync(Campaign campaign, CancellationToken cancellationToken = default);
}