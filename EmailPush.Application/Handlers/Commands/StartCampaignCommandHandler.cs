using MediatR;
using EmailPush.Application.Commands;
using EmailPush.Application.Interfaces;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailPush.Application.Handlers.Commands;

public class StartCampaignCommandHandler : IRequestHandler<StartCampaignCommand, bool>
{
    private readonly ICampaignRepository _repository;
    private readonly IEmailCampaignPublisher _campaignPublisher;
    private readonly ILogger<StartCampaignCommandHandler> _logger;

    public StartCampaignCommandHandler(
        ICampaignRepository repository,
        IEmailCampaignPublisher campaignPublisher,
        ILogger<StartCampaignCommandHandler> logger)
    {
        _repository = repository;
        _campaignPublisher = campaignPublisher;
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

        await _campaignPublisher.PublishCampaignAsync(campaign, cancellationToken);
        _logger.LogInformation("Campaign message published to queue: {CampaignId} - {CampaignName}", 
            campaign.Id, campaign.Name);

        return true;
    }
}