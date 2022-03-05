using AutoMapper;
using Standard.Customer.Domain;
using Standard.Customer.Domain.DTO;

namespace Standard.Customer.SOR.Configuration
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<CustomerDto, CustomerEntity>().ReverseMap()
                 .ForMember(m => m.FirstName, opt => opt.MapFrom(src => src.first_name))
                 .ForMember(m => m.LastName, opt => opt.MapFrom(src => src.last_name));
        }
    }
}
