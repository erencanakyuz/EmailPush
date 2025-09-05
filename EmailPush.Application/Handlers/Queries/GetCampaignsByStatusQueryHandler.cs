using MediatR;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Application.Utils;
using EmailPush.Domain.Interfaces;
using EmailPush.Domain.Entities;

namespace EmailPush.Application.Handlers.Queries;

public class GetCampaignsByStatusQueryHandler : IRequestHandler<GetCampaignsByStatusQuery, List<CampaignDto>>
{
    private readonly ICampaignRepository _repository;

    public GetCampaignsByStatusQueryHandler(ICampaignRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CampaignDto>> Handle(GetCampaignsByStatusQuery request, CancellationToken cancellationToken)
    {
        var campaigns = await _repository.GetByStatusAsync(request.Status);
        return CampaignMapper.ToDtoList(campaigns);
    }
}