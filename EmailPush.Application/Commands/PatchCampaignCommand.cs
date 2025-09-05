using MediatR;
using EmailPush.Application.DTOs;

namespace EmailPush.Application.Commands;

public class PatchCampaignCommand : IRequest<CampaignDto?>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Subject { get; set; }
    public string? Content { get; set; }
    public List<string>? Recipients { get; set; }
}