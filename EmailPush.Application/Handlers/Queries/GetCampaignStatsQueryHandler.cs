using MediatR;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Enums;
using EmailPush.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        // Get IQueryable to perform database-level aggregations - NO memory loading!
        var campaignsQuery = _repository.GetAll();

        return new CampaignStatsDto
        {
            // Each query executes as optimized SQL COUNT/SUM in database
            TotalCampaigns = await campaignsQuery.CountAsync(cancellationToken),
            DraftCampaigns = await campaignsQuery.CountAsync(c => c.Status == CampaignStatus.Draft, cancellationToken),
            ReadyCampaigns = await campaignsQuery.CountAsync(c => c.Status == CampaignStatus.Ready, cancellationToken),
            SendingCampaigns = await campaignsQuery.CountAsync(c => c.Status == CampaignStatus.Sending, cancellationToken),
            CompletedCampaigns = await campaignsQuery.CountAsync(c => c.Status == CampaignStatus.Completed, cancellationToken),
            FailedCampaigns = await campaignsQuery.CountAsync(c => c.Status == CampaignStatus.Failed, cancellationToken),
            TotalEmailsSent = await campaignsQuery.SumAsync(c => c.SentCount, cancellationToken),
            
            // For Recipients.Count, we need to load the data since it's a CSV parsing operation
            // But we only load the Recipients column, not the entire entities
            TotalRecipients = await campaignsQuery
                .Select(c => c.Recipients)
                .ToListAsync(cancellationToken)
                .ContinueWith(task => task.Result.Sum(recipients => recipients.Count), cancellationToken)
        };
    }
}