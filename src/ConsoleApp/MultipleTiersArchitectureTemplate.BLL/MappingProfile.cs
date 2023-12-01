using AutoMapper;
using MultipleTiersArchitectureTemplate.BLL.Test.Models;
using MultipleTiersArchitectureTemplate.DAL.DataAccessModels;


namespace MultipleTiersArchitectureTemplate.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonBLLModel , PersonDALModel>(); // Define the mapping between Person and PersonMongoModel
        }
    }
}
