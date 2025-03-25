﻿using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;
    public CompaniesController(IServiceManager service)
    {
        _service = service;
    }
    
    [HttpGet]
    public IActionResult GetCompanies()
    {
        var companies = _service.CompanyService.GetAllCompanies(false);
        return Ok(companies);
    }
    
    [HttpGet("{id:guid}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _service.CompanyService.GetCompany(id, false);
        return Ok(company);
    }
    
    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto? company)
    {
        if (company is null)
            return BadRequest("CompanyForCreationDto object is null");
        
        var createdCompany = _service.CompanyService.CreateCompany(company);
        
        // Аналог: CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, createdCompany);
        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id },
            createdCompany);
    }
    
    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public IActionResult GetCompanyCollection(IEnumerable<Guid> ids)
    {
        var companies = _service.CompanyService.GetByIds(ids, false);
        
        return Ok(companies);
    }
    
    [HttpPost("collection")]
    public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
    {
        var result = _service.CompanyService.CreateCompanyCollection(companyCollection);
        
        return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
    }

    
}