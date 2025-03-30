using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/companiesCQRS")]
[ApiController]
public class CompaniesControllerCQRS : ControllerBase
{
    private readonly ISender _sender;
    
    public CompaniesControllerCQRS(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = 
            await _sender.Send(new GetCompaniesQuery(TrackChanges: false));
        
        return Ok(companies);
    }
    
    [HttpGet("{id:guid}", Name = "CompanyByIdCQRS")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await _sender.Send(new GetCompanyQuery(id, TrackChanges: false));
        return Ok(company);
    }

}