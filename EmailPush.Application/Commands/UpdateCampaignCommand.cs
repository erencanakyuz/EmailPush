using MediatR;
using EmailPush.Application.DTOs;

namespace EmailPush.Application.Commands;

public class UpdateCampaignCommand : IRequest<CampaignDto?>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Recipients { get; set; } = new();
}