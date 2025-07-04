using AutoMapper;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.Adventures.Narratives;
using VirtualDungeonMaster.Domain.Characters;
using VirtualDungeonMaster.Infrastructure.Entities.Adventures;
using VirtualDungeonMaster.Infrastructure.Entities.Characters;
using VirtualDungeonMaster.Infrastructure.Entities.Narratives;

namespace VirtualDungeonMaster.Infrastructure.Mapping
{
    public class EntityToDomainProfile : Profile
    {
        public EntityToDomainProfile()
        {
            CreateMap<CharacterEntity, Character>().ReverseMap();
            CreateMap<NarrativeEventEntity, NarrativeEvent>()
                .ConstructUsing(e => new NarrativeEvent(e.TurnNumber, e.PlayerInput, e.AIResponse))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp))
                .ReverseMap();
            CreateMap<AdventureSessionEntity, AdventureSession>()
                .ConstructUsing(e => new AdventureSession(e.CharacterId, e.Title))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<AdventureStatus>(src.Status)))
                .ForMember(dest => dest.StartedAt, opt => opt.MapFrom(src => src.StartedAt))
                .ForMember(dest => dest.EndedAt, opt => opt.MapFrom(src => src.EndedAt))
                .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src.Events))
                .ForMember(dest => dest.CurrentTurnNumber, opt => opt.MapFrom(src => src.CurrentTurnNumber))
                .ForMember(dest => dest.CurrentPrompt, opt => opt.MapFrom(src => src.CurrentPrompt))
                .ReverseMap();
        }
    }
}
