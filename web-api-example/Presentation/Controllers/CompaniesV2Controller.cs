using Asp.Versioning;
using Entities.Responses;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers;

// will send in header "api-deprecated-versions" instead of "api-supported-versions"
[ApiVersion("2.0", Deprecated = true)]
// with version in Uri
//[Route("api/{v:apiVersion}/companies")]
[Route("api/companies")]
[ApiController]
[ApiExplorerSettings(GroupName = "v2")]
public class CompaniesV2Controller : ApiControllerBase
{
    private readonly IServiceManager _service;
    
    public CompaniesV2Controller(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _service.CompanyService
            .GetAllCompaniesAsync(false);
        
        var companiesV2 = companies.Select(x => $"{x.Name} V2");
        return Ok(companiesV2);
    }
    
    [HttpGet("/flow")]
    public async Task<IActionResult> GetCompaniesFlow()
    {
        var baseResult = await _service.CompanyService.GetAllCompaniesAsyncFlow(false);

        var companies = baseResult.GetResult<IEnumerable<CompanyDto>>();

        return Ok(companies);
    }
}