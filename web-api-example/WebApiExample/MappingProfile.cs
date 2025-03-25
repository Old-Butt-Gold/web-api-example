using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace WebApiExample;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress, expression =>     
                expression.MapFrom(company => 
                    string.Concat(company.Address, " ", company.Country)));
        
        CreateMap<Employee, EmployeeDto>();
        
        CreateMap<CompanyForCreationDto, Company>();

        CreateMap<EmployeeForCreationDto, Employee>();
        
        CreateMap<EmployeeForUpdateDto, Employee>();
        
        CreateMap<CompanyForUpdateDto, Company>();
    }

}