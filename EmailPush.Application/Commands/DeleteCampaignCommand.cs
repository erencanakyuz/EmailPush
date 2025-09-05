using MediatR;

namespace EmailPush.Application.Commands;

public class DeleteCampaignCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}