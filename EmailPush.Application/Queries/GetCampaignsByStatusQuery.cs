using MediatR;
using EmailPush.Application.DTOs;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Enums;

namespace EmailPush.Application.Queries;

public class GetCampaignsByStatusQuery : IRequest<PagedResponseDto<CampaignDto>>
{
    public CampaignStatus Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}