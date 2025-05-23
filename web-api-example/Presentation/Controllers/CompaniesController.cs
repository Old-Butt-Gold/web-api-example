﻿using Asp.Versioning;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Presentation.ModelBinders;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
// with apiVersion in URI
//[Route("api/{v:apiVersion}/companies")]
[Route("api/companies")]
[ApiController]
// this cache rule will apply to all the actions inside
// the controller except the ones that already have the ResponseCache
// attribute applied.
//[ResponseCache(CacheProfileName = "120SecondsDuration")]
// P.S it's commented because of Marvin.Cache.Headers.Library
[ApiExplorerSettings(GroupName = "v1")]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;
    public CompaniesController(IServiceManager service)
    {
        _service = service;
    }
    
    [HttpGet(Name = "GetCompanies")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await
            _service.CompanyService.GetAllCompaniesAsync(false);
        return Ok(companies);
    }
    
    [HttpGet("{id:guid}", Name = "CompanyById")]
    //[ResponseCache(Duration = 60)]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
    [HttpCacheValidation(MustRevalidate = false)]
    // will override the global configuration in extensions
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await _service.CompanyService.GetCompanyAsync(id, false);
        
        return Ok(company);
    }
    
    [HttpPost(Name = "CreateCompany")]
    [ServiceFilter(typeof(ValidationActionFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto? company)
    {
        var createdCompany = await _service.CompanyService.CreateCompanyAsync(company);
        
        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id },
            createdCompany);
    }
    
    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
    {
        var companies = await _service.CompanyService.GetByIdsAsync(ids, false);
        return Ok(companies);
    }
    
    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
    {
        var result = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);
        return CreatedAtRoute("CompanyCollection", new { result.ids },
            result.companies);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _service.CompanyService.DeleteCompanyAsync(id, false);
        return NoContent();
    }
    
    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationActionFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto? company)
    {
        await _service.CompanyService.UpdateCompanyAsync(id, company, true);
        
        return NoContent();
    }
    
    [HttpOptions]
    public IActionResult GetCompaniesOptions()
    {
        //Content-Length also should be set to zero, but ASP.NET Core takes care of that
        
        Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

        return Ok();
    }

}