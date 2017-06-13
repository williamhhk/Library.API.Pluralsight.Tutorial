using Library.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Controllers
{
    [Route("api")]
    public class RootController : Controller
    {

        public RootController()
        {
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if (mediaType == "application/vnd.william.hateoas+json")
            {
                var links = new List<LinkDto>();

                links.Add(
                  new LinkDto(Url.Link("GetRoot", new { }),
                  "self",
                  "GET"));

                links.Add(
                 new LinkDto(Url.Link("GetAuthors", new { }),
                 "authors",
                 "GET"));

                links.Add(
                  new LinkDto(Url.Link("CreateAuthor", new { }),
                  "create_author",
                  "POST"));

                return Ok(links);
            }

            return NoContent();
        }
    }
}
