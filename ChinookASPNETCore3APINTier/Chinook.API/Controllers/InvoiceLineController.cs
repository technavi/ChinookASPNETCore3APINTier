﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using Chinook.Domain.Supervisor;
using Chinook.Domain.ApiModels;
using Microsoft.AspNetCore.Cors;

namespace Chinook.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class InvoiceLineController : ControllerBase
    {
        private readonly IChinookSupervisor _chinookSupervisor;

        public InvoiceLineController(IChinookSupervisor chinookSupervisor)
        {
            _chinookSupervisor = chinookSupervisor;
        }

        [HttpGet]
        [Produces(typeof(List<InvoiceLineApiModel>))]
        public async Task<ActionResult<List<InvoiceLineApiModel>>> Get()
        {
            try
            {
                return new ObjectResult(await _chinookSupervisor.GetAllInvoiceLine());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        [Produces(typeof(InvoiceLineApiModel))]
        public async Task<ActionResult<InvoiceLineApiModel>> Get(int id)
        {
            try
            {
                var invoiceLine = await _chinookSupervisor.GetInvoiceLineById(id);
                if ( invoiceLine == null)
                {
                    return NotFound();
                }

                return Ok(invoiceLine);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("invoice/{id}")]
        [Produces(typeof(List<InvoiceLineApiModel>))]
        public async Task<ActionResult<InvoiceLineApiModel>> GetByInvoiceId(int id)
        {
            try
            {
                var invoice = await _chinookSupervisor.GetInvoiceById(id);
                if ( invoice == null)
                {
                    return NotFound();
                }

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("track/{id}")]
        [Produces(typeof(List<InvoiceLineApiModel>))]
        public async Task<ActionResult<InvoiceLineApiModel>> GetByArtistId(int id)
        {
            try
            {
                var track = await _chinookSupervisor.GetTrackById(id);
                if (track == null)
                {
                    return NotFound();
                }

                return Ok(track);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceLineApiModel>> Post([FromBody] InvoiceLineApiModel input)
        {
            try
            {
                if (input == null)
                    return BadRequest();

                return StatusCode(201, await _chinookSupervisor.AddInvoiceLine(input));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InvoiceLineApiModel>> Put(int id, [FromBody] InvoiceLineApiModel input)
        {
            try
            {
                if (input == null)
                    return BadRequest();
                if (!_chinookSupervisor.InvoiceLineExists(id))
                {
                    return NotFound();
                }

                var errors = JsonConvert.SerializeObject(ModelState.Values
                    .SelectMany(state => state.Errors)
                    .Select(error => error.ErrorMessage));
                Debug.WriteLine(errors);

                if (await _chinookSupervisor.UpdateInvoiceLine(input))
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
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (!_chinookSupervisor.InvoiceLineExists(id))
                {
                    return NotFound();
                }

                if (await _chinookSupervisor.DeleteInvoiceLine(id))
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