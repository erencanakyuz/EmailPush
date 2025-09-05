using MediatR;
using EmailPush.Application.DTOs;
using EmailPush.Domain.Entities;

namespace EmailPush.Application.Queries;

public class GetCampaignsByStatusQuery : IRequest<List<CampaignDto>>
{
    public CampaignStatus Status { get; set; }
}