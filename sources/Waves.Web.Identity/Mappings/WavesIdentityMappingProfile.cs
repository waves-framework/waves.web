using AutoMapper;
using Waves.Web.Identity.Entities.ApiEntities;
using Waves.Web.Identity.Entities.DbEntities;
using Waves.Web.Identity.Entities.DtoEntities;

namespace Waves.Web.Identity.Mappings
{
    /// <summary>
    /// Default mapping profile.
    /// </summary>
    public class WavesIdentityMappingProfile : Profile
    {
        /// <summary>
        /// Creates new instance of <see cref="WavesIdentityMappingProfile"/>.
        /// </summary>
        public WavesIdentityMappingProfile()
        {
            CreateUserMaps();
        }

        /// <summary>
        /// Creates user maps.
        /// </summary>
        private void CreateUserMaps()
        {
            CreateMap<WavesUserRegistrationInputApiEntity, WavesUserRegistrationDto>().ReverseMap();
            CreateMap<WavesUserRegistrationDto, WavesUserEntity>().ReverseMap();
            CreateMap<WavesUserEntity, WavesUserDto>().ReverseMap();
            CreateMap<WavesUserDto, WavesUserRegistrationOutputApiEntity>().ReverseMap();
            CreateMap<WavesUserLoginInputApiEntity, WavesUserLoginDto>().ReverseMap();
            CreateMap<WavesUserDto, WavesUserLoginOutputApiEntity>().ReverseMap();
        }
    }
}
