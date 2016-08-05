using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private GeoCoordsService _coordsService;
        private ILogger<StopsController> _logger;
        private IWorldRepository _repository;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger, GeoCoordsService coordService)
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordService;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName);

                if (trip == null)
                {
                    return Json(null);
                }

                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order)));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get stops for trip {tripName} : {0}", ex);
                return BadRequest("Failed to get stops");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Map to the entity
                    var newStop = Mapper.Map<Stop>(vm);

                    // Look up the coordinates
                    var result = await _coordsService.GetCoordsAsync(newStop.Name);

                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;
                    }

                    // Save to the database
                    _repository.AddStop(tripName,newStop);

                    if ( await _repository.SaveChangesAsync())
                    {
                        return Created($"/api/trips/{tripName}/stops/{newStop.Name}",Mapper.Map <StopViewModel>(newStop));
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Failed to save new stop: {0}", ex);
            }

            return BadRequest(new { message = "Failed to save new stop", ModelState = ModelState });
        }
    }
}
