using MediatR;
using EmailPush.Application.DTOs;

namespace EmailPush.Application.Queries;

public class GetCampaignStatsQuery : IRequest<CampaignStatsDto>
{
}