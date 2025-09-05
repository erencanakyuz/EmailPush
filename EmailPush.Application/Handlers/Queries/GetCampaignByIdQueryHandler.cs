using MediatR;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Application.Utils;
using EmailPush.Domain.Interfaces;

namespace EmailPush.Application.Handlers.Queries;

public class GetCampaignByIdQueryHandler : IRequestHandler<GetCampaignByIdQuery, CampaignDto?>
{
    private readonly ICampaignRepository _repository;

    public GetCampaignByIdQueryHandler(ICampaignRepository repository)
    {
        _repository = repository;
    }

    public async Task<CampaignDto?> Handle(GetCampaignByIdQuery request, CancellationToken cancellationToken)
    {
        var campaign = await _repository.GetByIdAsync(request.Id);
        return campaign != null ? CampaignMapper.ToDto(campaign) : null;
    }
}