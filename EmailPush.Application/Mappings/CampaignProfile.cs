using AutoMapper;
using EmailPush.Application.DTOs;
using EmailPush.Application.Commands;
using EmailPush.Domain.Entities;
using EmailPush.Domain.Enums;

namespace EmailPush.Application.Mappings;

public class CampaignProfile : Profile
{
    public CampaignProfile()
    {
        CreateMap<Campaign, CampaignDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.TotalRecipients, opt => opt.MapFrom(src => src.Recipients.Count));

        CreateMap<CreateCampaignDto, Campaign>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CampaignStatus.Draft))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.SentCount, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.StartedAt, opt => opt.Ignore());

        CreateMap<CreateCampaignCommand, Campaign>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CampaignStatus.Draft))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.SentCount, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.StartedAt, opt => opt.Ignore());

        CreateMap<UpdateCampaignCommand, Campaign>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.SentCount, opt => opt.Ignore())
            .ForMember(dest => dest.StartedAt, opt => opt.Ignore());

        CreateMap<CreateCampaignDto, Campaign>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.SentCount, opt => opt.Ignore())
            .ForMember(dest => dest.StartedAt, opt => opt.Ignore());

        CreateMap<PatchCampaignCommand, Campaign>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}