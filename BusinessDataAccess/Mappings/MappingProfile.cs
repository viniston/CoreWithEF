using AutoMapper;
using DataModels = BusinessDataAccessDefinition.Models;
using Entities = BusinessEntitties;

namespace BusinessDataAccess.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Review

            CreateMap<Entities.Review, DataModels.Review>();

            CreateMap<DataModels.Review, Entities.Review>();

            #endregion
        }
    }
}
