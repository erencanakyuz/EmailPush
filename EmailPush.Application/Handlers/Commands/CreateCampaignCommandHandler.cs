using MediatR;
using EmailPush.Application.Commands;
using EmailPush.Application.DTOs;
using EmailPush.Application.Utils;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailPush.Application.Handlers.Commands;

public class CreateCampaignCommandHandler : IRequestHandler<CreateCampaignCommand, CampaignDto>
{
    private readonly ICampaignRepository _repository;
    private readonly ILogger<CreateCampaignCommandHandler> _logger;

    public CreateCampaignCommandHandler(
        ICampaignRepository repository,
        ILogger<CreateCampaignCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CampaignDto> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
    {
        var invalidEmails = EmailValidator.GetInvalidEmails(request.Recipients);
        if (invalidEmails.Any())
        {
            throw new ArgumentException($"Invalid email addresses: {string.Join(", ", invalidEmails)}");
        }

        var campaign = CampaignMapper.FromCreateCommand(request);

        var created = await _repository.AddAsync(campaign);
        _logger.LogInformation("Campaign created: {CampaignId} - {CampaignName}", created.Id, created.Name);
        
        return CampaignMapper.ToDto(created);
    }
}