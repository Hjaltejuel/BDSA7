using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BDSA2017.Assignment07.Models;

namespace BDSA2017.Assignment07.Web.Properties
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TracksController : Controller
    {
        private readonly ITrackRepository repository;

        public TracksController(ITrackRepository _repository)
        {
            repository = _repository;
        }
        // GET: api/
        [HttpGet]
        public IActionResult Get()
        {
            var result =   repository.Read();
            if(result == null)
            {
                return NoContent();
            }
            return Ok(result);

        }

        // GET: api/Track/5
        [HttpGet("{id}", Name ="Get")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await repository.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        
        // POST: api/Track
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TrackCreateDTO trackCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var id = await repository.Create(trackCreateDTO);
            return CreatedAtAction(nameof(Get), new {id}, null);

        }
        
        // PUT: api/Track/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]TrackUpdateDTO trackUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool IsThere = await repository.Update(trackUpdateDTO);

            if (!IsThere)
            {
                return NotFound();
            }
            else
            {
                return Ok(trackUpdateDTO);
            }
            
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isThere = await repository.Delete(id);
            if (isThere)
            {
                return Ok();
            } else
            {
                return NotFound();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
