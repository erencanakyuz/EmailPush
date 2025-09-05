using MediatR;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Application.Utils;
using EmailPush.Domain.Interfaces;

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
        var campaigns = await _repository.GetAllAsync();
        var filteredCampaigns = campaigns.Where(c => c.Status == request.Status);
        return CampaignMapper.ToDtoList(filteredCampaigns);
    }
}