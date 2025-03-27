using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api")]
[ApiController]
public class RootController : ControllerBase
{
    [HttpGet(Name = "GetRoot")]
    public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
    {
        if (mediaType.Contains("application/vnd.codemaze.apiroot"))
        {
            var list = new List<Link>
            {
                new Link
                {
                    Href = Url.Link(nameof(GetRoot), new { }),
                    Rel = "self",
                    Method = "GET"
                },
                new Link
                {
                    Href = Url.Link("GetCompanies", new { }),
                    Rel = "companies",
                    Method = "GET"
                },
                new Link
                {
                    Href = Url.Link("CreateCompany", new { }),
                    Rel = "create_company",
                    Method = "POST"
                }
            };
            return Ok(list);
        }

        return NoContent();
    }

}
