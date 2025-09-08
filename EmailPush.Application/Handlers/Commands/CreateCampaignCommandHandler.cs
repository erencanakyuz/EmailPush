using MediatR;
using AutoMapper;
using EmailPush.Application.Commands;
using EmailPush.Application.DTOs;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailPush.Application.Handlers.Commands;

public class CreateCampaignCommandHandler : IRequestHandler<CreateCampaignCommand, CampaignDto>
{
    private readonly ICampaignRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCampaignCommandHandler> _logger;

    public CreateCampaignCommandHandler(
        ICampaignRepository repository,
        IMapper mapper,
        ILogger<CreateCampaignCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CampaignDto> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
    {
        var campaign = _mapper.Map<Campaign>(request);

        var created = await _repository.AddAsync(campaign);
        _logger.LogInformation("Campaign created: {CampaignId} - {CampaignName}", created.Id, created.Name);
        
        return _mapper.Map<CampaignDto>(created);
    }
}