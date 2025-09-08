using MediatR;
using AutoMapper;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Domain.Interfaces;
using EmailPush.Domain.Enums;

namespace EmailPush.Application.Handlers.Queries;

public class GetCampaignsQueryHandler : IRequestHandler<GetCampaignsQuery, PagedResponseDto<CampaignDto>>
{
    private readonly ICampaignRepository _repository;
    private readonly IMapper _mapper;

    public GetCampaignsQueryHandler(ICampaignRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResponseDto<CampaignDto>> Handle(GetCampaignsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<CampaignDto> campaignDtos;
        int totalCount;

        if (request.Status.HasValue)
        {
            // Filter by status
            var campaigns = await _repository.GetPagedByStatusAsync(request.Status.Value, request.PageNumber, request.PageSize);
            campaignDtos = _mapper.Map<List<CampaignDto>>(campaigns);
            totalCount = await _repository.GetCountByStatusAsync(request.Status.Value);
        }
        else
        {
            // Get all campaigns
            var campaigns = await _repository.GetPagedAsync(request.PageNumber, request.PageSize);
            campaignDtos = _mapper.Map<List<CampaignDto>>(campaigns);
            totalCount = await _repository.GetTotalCountAsync();
        }

        return new PagedResponseDto<CampaignDto>(campaignDtos, request.PageNumber, request.PageSize, totalCount);
    }
}