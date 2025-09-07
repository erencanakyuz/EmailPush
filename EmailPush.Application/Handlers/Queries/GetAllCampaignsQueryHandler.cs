using MediatR;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Application.Utils;
using EmailPush.Domain.Interfaces;

namespace EmailPush.Application.Handlers.Queries;

public class GetAllCampaignsQueryHandler : IRequestHandler<GetAllCampaignsQuery, PagedResponseDto<CampaignDto>>
{
    private readonly ICampaignRepository _repository;

    public GetAllCampaignsQueryHandler(ICampaignRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponseDto<CampaignDto>> Handle(GetAllCampaignsQuery request, CancellationToken cancellationToken)
    {
        var campaigns = await _repository.GetPagedAsync(request.PageNumber, request.PageSize);
        var campaignDtos = CampaignMapper.ToDtoList(campaigns);
        var totalCount = await _repository.GetTotalCountAsync();

        return new PagedResponseDto<CampaignDto>(campaignDtos, request.PageNumber, request.PageSize, totalCount);
    }
}