using MediatR;
using EmailPush.Application.Commands;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Enums;
using EmailPush.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailPush.Application.Handlers.Commands;

public class DeleteCampaignCommandHandler : IRequestHandler<DeleteCampaignCommand, bool>
{
    private readonly ICampaignRepository _repository;
    private readonly ILogger<DeleteCampaignCommandHandler> _logger;

    public DeleteCampaignCommandHandler(
        ICampaignRepository repository,
        ILogger<DeleteCampaignCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = await _repository.GetByIdAsync(request.Id);
        if (campaign == null)
            return false;

        if (campaign.Status != CampaignStatus.Draft)
        {
            throw new InvalidOperationException("Only draft campaigns can be deleted");
        }

        await _repository.DeleteAsync(campaign);
        _logger.LogInformation("Campaign deleted: {CampaignId} - {CampaignName}", campaign.Id, campaign.Name);

        return true;
    }
}