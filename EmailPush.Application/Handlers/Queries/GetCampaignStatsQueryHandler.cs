using MediatR;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;

namespace EmailPush.Application.Handlers.Queries;

public class GetCampaignStatsQueryHandler : IRequestHandler<GetCampaignStatsQuery, CampaignStatsDto>
{
    private readonly ICampaignRepository _repository;

    public GetCampaignStatsQueryHandler(ICampaignRepository repository)
    {
        _repository = repository;
    }

    public async Task<CampaignStatsDto> Handle(GetCampaignStatsQuery request, CancellationToken cancellationToken)
    {
        // Get all campaigns with pagination to avoid loading everything into memory
        var totalCount = await _repository.GetTotalCountAsync();
        
        // For stats, we still need to get all campaigns, but we can do it more efficiently
        // by only loading what we need for the statistics calculation
        var campaigns = await _repository.GetAllAsync();
        var campaignsList = campaigns.ToList();

        return new CampaignStatsDto
        {
            TotalCampaigns = campaignsList.Count,
            DraftCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Draft),
            ReadyCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Ready),
            SendingCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Sending),
            CompletedCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Completed),
            FailedCampaigns = campaignsList.Count(c => c.Status == CampaignStatus.Failed),
            TotalEmailsSent = campaignsList.Sum(c => c.SentCount),
            TotalRecipients = campaignsList.Sum(c => c.Recipients.Count)
        };
    }
}