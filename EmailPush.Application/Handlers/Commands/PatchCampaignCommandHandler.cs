using MediatR;
using EmailPush.Application.Commands;
using EmailPush.Application.DTOs;
using EmailPush.Application.Utils;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailPush.Application.Handlers.Commands;

public class PatchCampaignCommandHandler : IRequestHandler<PatchCampaignCommand, CampaignDto?>
{
    private readonly ICampaignRepository _repository;
    private readonly ILogger<PatchCampaignCommandHandler> _logger;

    public PatchCampaignCommandHandler(
        ICampaignRepository repository,
        ILogger<PatchCampaignCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CampaignDto?> Handle(PatchCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = await _repository.GetByIdAsync(request.Id);
        if (campaign == null)
            return null;

        if (campaign.Status != CampaignStatus.Draft)
        {
            throw new InvalidOperationException("Only draft campaigns can be updated");
        }

        if (request.Recipients != null && request.Recipients.Any())
        {
            var invalidEmails = EmailValidator.GetInvalidEmails(request.Recipients);
            if (invalidEmails.Any())
            {
                throw new ArgumentException($"Invalid email addresses: {string.Join(", ", invalidEmails)}");
            }
        }

        if (!string.IsNullOrEmpty(request.Name))
            campaign.Name = request.Name;
        
        if (!string.IsNullOrEmpty(request.Subject))
            campaign.Subject = request.Subject;
        
        if (!string.IsNullOrEmpty(request.Content))
            campaign.Content = request.Content;
        
        if (request.Recipients != null)
            campaign.Recipients = request.Recipients;

        await _repository.UpdateAsync(campaign);
        _logger.LogInformation("Campaign partially updated: {CampaignId} - {CampaignName}", campaign.Id, campaign.Name);

        return CampaignMapper.ToDto(campaign);
    }
}