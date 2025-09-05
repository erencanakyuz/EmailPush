using MediatR;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Application.Utils;
using EmailPush.Domain.Interfaces;

namespace EmailPush.Application.Handlers.Queries;

public class GetAllCampaignsQueryHandler : IRequestHandler<GetAllCampaignsQuery, List<CampaignDto>>
{
    private readonly ICampaignRepository _repository;

    public GetAllCampaignsQueryHandler(ICampaignRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CampaignDto>> Handle(GetAllCampaignsQuery request, CancellationToken cancellationToken)
    {
        var campaigns = await _repository.GetAllAsync();
        return CampaignMapper.ToDtoList(campaigns);
    }
}