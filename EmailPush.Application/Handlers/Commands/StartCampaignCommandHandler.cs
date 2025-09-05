using MediatR;
using MassTransit;
using EmailPush.Application.Commands;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;
using EmailPush.Domain.Messages;
using Microsoft.Extensions.Logging;

namespace EmailPush.Application.Handlers.Commands;

public class StartCampaignCommandHandler : IRequestHandler<StartCampaignCommand, bool>
{
    private readonly ICampaignRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<StartCampaignCommandHandler> _logger;

    public StartCampaignCommandHandler(
        ICampaignRepository repository,
        IPublishEndpoint publishEndpoint,
        ILogger<StartCampaignCommandHandler> logger)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<bool> Handle(StartCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = await _repository.GetByIdAsync(request.Id);
        if (campaign == null)
            return false;

        if (campaign.Status != CampaignStatus.Draft)
        {
            throw new InvalidOperationException("Only draft campaigns can be started");
        }

        campaign.Status = CampaignStatus.Ready;
        campaign.StartedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(campaign);

        _logger.LogInformation("Campaign started: {CampaignId} - {CampaignName}, Recipients: {RecipientCount}", 
            campaign.Id, campaign.Name, campaign.Recipients.Count);

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
        _logger.LogInformation("Campaign message published to queue: {CampaignId} - {CampaignName}", 
            campaign.Id, campaign.Name);

        return true;
    }
}