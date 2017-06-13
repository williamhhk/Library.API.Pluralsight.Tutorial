using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Library.API.Services;
using Library.API.Models;
using AutoMapper;
using System.Linq;
using Library.API.Entities;
using Library.API.Helpers;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private ILibraryRepository _libraryRepository;

        public AuthorsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpGet(Name = "GetAuthors")]
        [HttpHead]

        public IActionResult GetAuthors()
        {
            var authorsFromRepo = _libraryRepository.GetAuthors();

            var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            return Ok(authors);
        }

        [HttpGet("{id}", Name ="GetAuthor")]
        public IActionResult GetAuthor(Guid id, [FromHeader(Name ="Accept")] string mediaType)
        {
            var authorFromRepo = _libraryRepository.GetAuthor(id);

            if (!authorFromRepo.Any())
            {
                return NotFound();
            }

            if (mediaType == "application/vnd.william+json")
            {
                var author = Mapper.Map<AuthorDto>(authorFromRepo.SingleOrDefault());
                return Ok(author);

            }
            else
            {
                var author = Mapper.Map<AuthorDto>(authorFromRepo.SingleOrDefault());
                return Ok(author);
            }

        }

        [HttpPost("", Name = "CreateAuthor")]
        [RequestHeaderMatchesMediaType("Content-Type",
            new[] { "application/vnd.marvin.author.full+json" })]

        public IActionResult CreateAuthor([FromBody] AuthorForCreationDto author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            var authorEntity = Mapper.Map<Author>(author);

            _libraryRepository.AddAuthor(authorEntity);

            var authorToReturn = Mapper.Map<AuthorDto>(authorEntity);

            return CreatedAtRoute("GetAuthor",
                new { id = authorToReturn.Id },
                authorToReturn);
        }


        [HttpPost("", Name = "CreateAuthorWithDateOfDeath")]
        [RequestHeaderMatchesMediaType("Content-Type",
            new[] { "application/vnd.marvin.authorwithdateofdeath.full+json" })]
        public IActionResult CreateAuthorWithDateOfDeath([FromBody] AuthorForCreationWithDateOfDeathDto author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            var authorEntity = Mapper.Map<Author>(author);

            _libraryRepository.AddAuthor(authorEntity);

            var authorToReturn = Mapper.Map<AuthorDto>(authorEntity);

            return CreatedAtRoute("GetAuthor",
                new { id = authorToReturn.Id },
                authorToReturn);
        }


        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

    }
}
