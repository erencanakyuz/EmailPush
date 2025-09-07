using EmailPush.Domain.Entities;

namespace EmailPush.Application.Interfaces;

public interface IEmailCampaignPublisher
{
    Task PublishCampaignAsync(Campaign campaign, CancellationToken cancellationToken = default);
}