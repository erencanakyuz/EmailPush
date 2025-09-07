using MediatR;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Application.Utils;
using EmailPush.Domain.Interfaces;
using EmailPush.Domain.Entities;

namespace EmailPush.Application.Handlers.Queries;

public class GetCampaignsByStatusQueryHandler : IRequestHandler<GetCampaignsByStatusQuery, PagedResponseDto<CampaignDto>>
{
    private readonly ICampaignRepository _repository;

    public GetCampaignsByStatusQueryHandler(ICampaignRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponseDto<CampaignDto>> Handle(GetCampaignsByStatusQuery request, CancellationToken cancellationToken)
    {
        var campaigns = await _repository.GetPagedByStatusAsync(request.Status, request.PageNumber, request.PageSize);
        var campaignDtos = CampaignMapper.ToDtoList(campaigns);
        
        // Get total count for the specific status
        var allCampaignsWithStatus = await _repository.GetByStatusAsync(request.Status);
        var totalCount = allCampaignsWithStatus.Count();

        return new PagedResponseDto<CampaignDto>(campaignDtos, request.PageNumber, request.PageSize, totalCount);
    }
}