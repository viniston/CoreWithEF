using AutoMapper;
using Entities = BusinessEntitties;

namespace ReviewAPIMicroService.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region Mapping Between Domain Model and View Model - START
            CreateMap<ViewModels.ReviewSummary, Entities.Review>().AfterMap((src, dest) => { });
            CreateMap<ViewModels.Review, Entities.Review>().AfterMap((src, dest) => { });
            CreateMap<Entities.Review, ViewModels.Review>().AfterMap((src, dest) => { });
            #endregion
        }
    }

}
