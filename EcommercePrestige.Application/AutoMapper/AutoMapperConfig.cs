using AutoMapper;

namespace EcommercePrestige.Application.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper Mapper { get; private set; }
        public static void RegisterMappings()
        {
            var _mapper = new MapperConfiguration((mapper) =>
            {
                mapper.AddProfile<DomainToViewModelMappingProfile>();
                mapper.AddProfile<ViewModelToDomainMappingProfile>();
            });

            Mapper = _mapper.CreateMapper();
        }
    }
}
