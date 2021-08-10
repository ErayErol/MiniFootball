﻿namespace MiniFootball.Infrastructure
{
    using AutoMapper;
    using Data.Models;
    using Models.Games;
    using Services.Fields;
    using Services.Games.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Game, GameListingServiceModel>();
            this.CreateMap<Game, GameIdUserIdServiceModel>();
            this.CreateMap<GameDetailsServiceModel, GameFormModel>();

            this.CreateMap<Game, GameDetailsServiceModel>()
                .ForMember(gDSM => gDSM.UserId, cfg => cfg.MapFrom(g => g.Admin.UserId));

            this.CreateMap<Game, GameIdUserIdServiceModel>()
                .ForMember(gDSM => gDSM.UserId, cfg => cfg.MapFrom(g => g.Admin.UserId));

            this.CreateMap<Field, FieldServiceModel>();
            this.CreateMap<Field, FieldListingServiceModel>();
            this.CreateMap<Field, FieldDetailServiceModel>();
        }
    }
}
