using MediatR;
using AutoMapper;
using EmailPush.Application.Queries;
using EmailPush.Application.DTOs;
using EmailPush.Domain.Interfaces;

namespace EmailPush.Application.Handlers.Queries;

public class GetCampaignByIdQueryHandler : IRequestHandler<GetCampaignByIdQuery, CampaignDto?>
{
    private readonly ICampaignRepository _repository;
    private readonly IMapper _mapper;

    public GetCampaignByIdQueryHandler(ICampaignRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CampaignDto?> Handle(GetCampaignByIdQuery request, CancellationToken cancellationToken)
    {
        var campaign = await _repository.GetByIdAsync(request.Id);
        return campaign != null ? _mapper.Map<CampaignDto>(campaign) : null;
    }
}