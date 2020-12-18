﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Chinook.Domain.Supervisor;

using Microsoft.AspNetCore.Cors;
using Chinook.Domain.Entities;

namespace Chinook.API.Controllers
{
    [Route("api/[controller]")]
    [ResponseCache(Duration = 604800)] // cache for a week
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class MediaTypeController : ControllerBase
    {
        private readonly IChinookSupervisor _chinookSupervisor;

        public MediaTypeController(IChinookSupervisor chinookSupervisor)
        {
            _chinookSupervisor = chinookSupervisor;
        }

        [HttpGet]
        [Produces(typeof(List<MediaType>))]
        [ResponseCache(Duration = 604800)] // cache for a week
        public ActionResult<List<MediaType>> Get()
        {
            try
            {
                return new ObjectResult(_chinookSupervisor.GetAllMediaType());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        [Produces(typeof(MediaType))]
        public ActionResult<MediaType> Get(int id)
        {
            try
            {
                var mediaType = _chinookSupervisor.GetMediaTypeById(id);
                if ( mediaType == null)
                {
                    return NotFound();
                }

                return Ok(mediaType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public ActionResult<MediaType> Post([FromBody] MediaType input)
        {
            try
            {
                if (input == null)
                    return BadRequest();

                return StatusCode(201, _chinookSupervisor.AddMediaType(input));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<MediaType> Put(int id, [FromBody] MediaType input)
        {
            try
            {
                if (input == null)
                    return BadRequest();
                if (_chinookSupervisor.GetMediaTypeById(id) == null)
                {
                    return NotFound();
                }

                // var errors = JsonConvert.SerializeObject(ModelState.Values
                //     .SelectMany(state => state.Errors)
                //     .Select(error => error.ErrorMessage));
                // Debug.WriteLine(errors);

                if (_chinookSupervisor.UpdateMediaType(input))
                {
                    return Ok(input);
                }

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_chinookSupervisor.GetMediaTypeById(id) == null)
                {
                    return NotFound();
                }

                if (_chinookSupervisor.DeleteMediaType(id))
                {
                    return Ok();
                }

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}