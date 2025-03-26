using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Presentation.Controllers;

[Route("api/companies/{companyId}/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;
    
    public EmployeesController(IServiceManager service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
    {
        var employees = await _service.EmployeeService.GetEmployeesAsync(companyId, false);
        
        return Ok(employees);
    }

    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = await _service.EmployeeService.
            GetEmployeeAsync(companyId, id, false);
        return Ok(employee);
    }
    
    [HttpPost]
    [ValidationFilter]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto? employee)
    {
        var employeeToReturn = await
            _service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, false);
        
        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id },
            employeeToReturn);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        await _service.EmployeeService.
            DeleteEmployeeForCompanyAsync(companyId, id, false);
        
        return NoContent();
    }
    
    [HttpPut("{id:guid}")]
    [ValidationFilter]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id,
        [FromBody] EmployeeForUpdateDto? employee)
    {
        await _service.EmployeeService.
            UpdateEmployeeForCompanyAsync(companyId, id, employee, false, true);
        return NoContent();
    }
}