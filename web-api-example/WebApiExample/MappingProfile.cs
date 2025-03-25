using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace WebApiExample;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            // For Member for classes without constructors, we have record
            //.ForMember(c => c.FullAddress, expression =>
            .ForCtorParam("FullAddress", expression =>     
                expression.MapFrom(company => 
                    string.Concat(company.Address, " ", company.Country)));
        
        CreateMap<Employee, EmployeeDto>();
    }

}