using MediatR;
using AutoMapper;
using EmailPush.Application.Commands;
using EmailPush.Application.DTOs;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Enums;
using EmailPush.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailPush.Application.Handlers.Commands;

public class PatchCampaignCommandHandler : IRequestHandler<PatchCampaignCommand, CampaignDto?>
{
    private readonly ICampaignRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<PatchCampaignCommandHandler> _logger;

    public PatchCampaignCommandHandler(
        ICampaignRepository repository,
        IMapper mapper,
        ILogger<PatchCampaignCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
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

        _mapper.Map(request, campaign);

        await _repository.UpdateAsync(campaign);
        _logger.LogInformation("Campaign partially updated: {CampaignId} - {CampaignName}", campaign.Id, campaign.Name);

        return _mapper.Map<CampaignDto>(campaign);
    }
}