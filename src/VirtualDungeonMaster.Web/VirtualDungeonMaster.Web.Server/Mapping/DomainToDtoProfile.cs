using AutoMapper;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.Adventures.Narratives;
using VirtualDungeonMaster.Domain.Characters;
using VirtualDungeonMaster.Web.Server.Dtos;

namespace VirtualDungeonMaster.Web.Server.Mapping
{
    public class DomainToDtoProfile : Profile
    {
        public DomainToDtoProfile()
        {
            CreateMap<Character, CharacterDto>().ReverseMap();
            CreateMap<NarrativeEvent, NarrativeEventDto>().ReverseMap();
            CreateMap<AdventureSession, AdventureSessionDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.AdventureSummary, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<AdventureSession, AdventureSessionSummaryDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap();
        }
    }
}
