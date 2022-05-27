using AutoMapper;
using ServerLoginApp.Modelos;
using ServerLoginApp.Modelos.Dto;

namespace ServerLoginApp
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<UsuarioDto, Usuario>();
                config.CreateMap<Usuario, UsuarioDto>();
            });

            return mappingConfig;
        }
    }
}
