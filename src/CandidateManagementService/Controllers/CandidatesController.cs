using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CandidateManagementService.Context;
using CandidateManagementService.Models;
using AutoMapper;
using CandidateManagementService.Dtos;
using CandidateManagementService.Repository;

namespace CandidateManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private ICandidateRepository repository;
        public CandidatesController(ICandidateRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("{id}", Name = "FindById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CandidateResponseDto>> FindById(long id)
        {
            var candidate = await repository.FindByIdAsync(id);
            if (candidate == null)
                return NotFound();
            return Ok(candidate);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CandidateResponseDto>>> FindAll()
        {
            return Ok(await repository.FindAllAsync());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CandidateResponseDto>> Save([FromBody] CandidateRequestDto candidateDto)
        {
            if (candidateDto != null)
            {
                var candidate = await repository.SaveAsync(candidateDto);
                return Created(nameof(Save), candidate);
            }
            return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CandidateResponseDto>> Update(long id, [FromBody] CandidateRequestDto candidateDto)
        {
            var candidate = await repository.FindByIdAsync(id);
            if (candidate != null)
            {
                await repository.UpdateAsync(id, candidateDto);
                return Ok(candidate);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(long id)
        {
            var candidate = await repository.FindByIdAsync(id);
            if (candidate != null)
            {
                await repository.DeleteByIdAsync(id);
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete()
        {
            await repository.DeleteAllAsync();
            return NoContent();
        }
    }
}