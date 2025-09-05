using MediatR;

namespace EmailPush.Application.Commands;

public class StartCampaignCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}