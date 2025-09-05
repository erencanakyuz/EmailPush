using MediatR;
using EmailPush.Application.DTOs;

namespace EmailPush.Application.Commands;

public class CreateCampaignCommand : IRequest<CampaignDto>
{
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Recipients { get; set; } = new();
}