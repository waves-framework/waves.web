using AutoMapper;
using Waves.Web.Identity.Entities.ApiEntities;
using Waves.Web.Identity.Entities.DbEntities;
using Waves.Web.Identity.Entities.DtoEntities;

namespace Waves.Web.Identity.Mappings
{
    /// <summary>
    /// Default mapping profile.
    /// </summary>
    public class IdentityMappingProfile : Profile
    {
        /// <summary>
        /// Creates new instance of <see cref="IdentityMappingProfile"/>.
        /// </summary>
        public IdentityMappingProfile()
        {
            CreateUserMaps();
        }

        /// <summary>
        /// Creates user maps.
        /// </summary>
        private void CreateUserMaps()
        {
            CreateMap<UserRegistrationInputApiEntity, UserRegistrationDto>().ReverseMap();
            CreateMap<UserRegistrationDto, UserDbEntity>().ReverseMap();
            CreateMap<UserDbEntity, UserDto>().ReverseMap();
            CreateMap<UserDto, UserRegistrationOutputApiEntity>().ReverseMap();
            CreateMap<UserLoginInputApiEntity, UserLoginDto>().ReverseMap();
            CreateMap<UserDto, UserLoginOutputApiEntity>().ReverseMap();
        }
    }
}
