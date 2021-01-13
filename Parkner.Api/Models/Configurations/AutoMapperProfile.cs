using AutoMapper;
using Parkner.Data.Dtos;
using Parkner.Data.Entities;

namespace Parkner.Api.Models.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Base
            this.CreateMap<BaseDto, Base>();
            //.ForMember(dest => dest.Version, opt => opt.MapFrom(src => Convert.FromBase64String(src.Version)));

            this.CreateMap<Base, BaseDto>();
            //    .ForMember(dest => dest.Version, opt => opt.MapFrom(src => Convert.ToBase64String(src.Version)));
            #endregion

            #region Responsable
            //this.CreateMap<Responsable, ResponsableDto>()
            //    //.BeforeMap((s, d) => d.Clave = String.IsNullOrWhiteSpace(s.Clave) ? d.Clave : s.Clave.Desencriptar())

            //    .ForMember(u => u.Estacionamientos, o => o.Ignore())
            //    .ForMember(u => u.Ganancias, o => o.Ignore());

            //this.CreateMap<ResponsableDto, Responsable>()
            //    //.BeforeMap((origen, destino) => destino.Clave = String.IsNullOrWhiteSpace(origen.Clave) ? destino.Clave : origen.Clave.Encriptar())
            //    //.ForMember(destino => destino.Version, o => o.MapFrom(origen => Convert.FromBase64String(origen.Version)))
            //    .ForMember(destino => destino.Estacionamientos, o => o.Ignore())
            //    .ForMember(destino => destino.Ganancias, o => o.Ignore());
            #endregion

            //this.CreateMap<UsuarioCrearPeticion, Usuario>()
            //    .ForMember(u => u.Id, o => o.MapFrom(cup => Guid.NewGuid().ToString()))
            //    .ForMember(u => u.Creacion, o => o.MapFrom(cup => DateTime.Now))
            //    .ForMember(u => u.Disponible, o => o.MapFrom(cup => true))
            //    .BeforeMap((s, d) => d.Disponible = true)
            //    .ForMember(u => u.Rol, o => o.MapFrom(cup => Roles.Administrador));

            //this.CreateMap<UsuarioCrearPeticion, Empleado>()
            //    .ForMember(u => u.Estacionamientos, o => o.Ignore())
            //    .ForMember(u => u.ReservasIniciadas, o => o.Ignore())
            //    .ForMember(u => u.ReservasFinalizadas, o => o.Ignore())
            //    .ForMember(u => u.Id, o => o.MapFrom(cup => Guid.NewGuid().ToString()))
            //    .ForMember(u => u.Creacion, o => o.MapFrom(cup => DateTime.Now))
            //    .ForMember(u => u.Disponible, o => o.MapFrom(cup => true))
            //    .ForMember(u => u.Rol, o => o.MapFrom(cup => Roles.Empleado));
        }
    }
}
