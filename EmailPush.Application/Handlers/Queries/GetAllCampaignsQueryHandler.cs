using MediatR;
using AutoMapper;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Domain.Interfaces;

namespace EmailPush.Application.Handlers.Queries;

public class GetAllCampaignsQueryHandler : IRequestHandler<GetAllCampaignsQuery, PagedResponseDto<CampaignDto>>
{
    private readonly ICampaignRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCampaignsQueryHandler(ICampaignRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResponseDto<CampaignDto>> Handle(GetAllCampaignsQuery request, CancellationToken cancellationToken)
    {
        var campaigns = await _repository.GetPagedAsync(request.PageNumber, request.PageSize);
        var campaignDtos = _mapper.Map<List<CampaignDto>>(campaigns);
        var totalCount = await _repository.GetTotalCountAsync();

        return new PagedResponseDto<CampaignDto>(campaignDtos, request.PageNumber, request.PageSize, totalCount);
    }
}