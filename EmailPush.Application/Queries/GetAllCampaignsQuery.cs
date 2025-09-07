using MediatR;
using EmailPush.Application.DTOs;

namespace EmailPush.Application.Queries;

public class GetAllCampaignsQuery : IRequest<PagedResponseDto<CampaignDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}