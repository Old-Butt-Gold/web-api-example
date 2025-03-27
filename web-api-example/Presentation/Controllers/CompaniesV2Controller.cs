﻿using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace Presentation.Controllers;

// will send in header "api-deprecated-versions" instead of "api-supported-versions"
[ApiVersion("2.0", Deprecated = true)]
// with version in Uri
//[Route("api/{v:apiVersion}/companies")]
[Route("api/companies")]
[ApiController]
public class CompaniesV2Controller : ControllerBase
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
}
