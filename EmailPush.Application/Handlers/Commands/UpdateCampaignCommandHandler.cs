using MediatR;
using EmailPush.Application.Commands;
using EmailPush.Application.DTOs;
using EmailPush.Application.Utils;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailPush.Application.Handlers.Commands;

public class UpdateCampaignCommandHandler : IRequestHandler<UpdateCampaignCommand, CampaignDto?>
{
    private readonly ICampaignRepository _repository;
    private readonly ILogger<UpdateCampaignCommandHandler> _logger;

    public UpdateCampaignCommandHandler(
        ICampaignRepository repository,
        ILogger<UpdateCampaignCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CampaignDto?> Handle(UpdateCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = await _repository.GetByIdAsync(request.Id);
        if (campaign == null)
            return null;

        if (campaign.Status != CampaignStatus.Draft)
        {
            throw new InvalidOperationException("Only draft campaigns can be updated");
        }

        var invalidEmails = EmailValidator.GetInvalidEmails(request.Recipients);
        if (invalidEmails.Any())
        {
            throw new ArgumentException($"Invalid email addresses: {string.Join(", ", invalidEmails)}");
        }

        campaign.Name = request.Name;
        campaign.Subject = request.Subject;
        campaign.Content = request.Content;
        campaign.Recipients = request.Recipients;

        await _repository.UpdateAsync(campaign);
        _logger.LogInformation("Campaign updated: {CampaignId} - {CampaignName}", campaign.Id, campaign.Name);

        return CampaignMapper.ToDto(campaign);
    }
}