using MassTransit;
using EmailPush.Application.Interfaces;
using EmailPush.Domain.Entities;
using EmailPush.Infrastructure.Messages;

namespace EmailPush.Infrastructure.Services;

public class EmailCampaignPublisher : IEmailCampaignPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EmailCampaignPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishCampaignAsync(Campaign campaign, CancellationToken cancellationToken = default)
    {
        var emailMessage = new EmailCampaignMessage
        {
            CampaignId = campaign.Id,
            CampaignName = campaign.Name,
            Subject = campaign.Subject,
            Content = campaign.Content,
            Recipients = campaign.Recipients.ToList(),
            CreatedAt = DateTime.UtcNow
        };

        await _publishEndpoint.Publish(emailMessage, cancellationToken);
    }
}