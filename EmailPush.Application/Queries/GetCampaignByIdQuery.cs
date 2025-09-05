using MediatR;
using EmailPush.Application.DTOs;

namespace EmailPush.Application.Queries;

public class GetCampaignByIdQuery : IRequest<CampaignDto?>
{
    public Guid Id { get; set; }
}