using MediatR;
using EmailPush.Application.DTOs;

namespace EmailPush.Application.Queries;

public class GetAllCampaignsQuery : IRequest<List<CampaignDto>>
{
}