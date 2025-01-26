namespace FinalProjAPI.Controller;
using AutoMapper;
using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("[controller]")]
public class TripController : ControllerBase
{
    private readonly ITripRepository _tripRepository;

    DataContextDapper _dapper;

    public TripController(ITripRepository tripRepository, IConfiguration configuration)
    {
        _tripRepository = tripRepository;
        _dapper = new DataContextDapper(configuration);
    }

    [HttpGet("GetTrip")]
    public List<TripBrief> GetTrips()
    {
        var trips = _tripRepository.GetAllTripsAsync();
        if (trips == null)
        {
            throw new Exception("Failed to get Trip");
        }
        return trips;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TripDto>> GetTrip(int id)
    {
        var trip = await _tripRepository.GetTripByIdAsync(id);
        if (trip == null)
        {
            return NotFound();
        }

        var tripDto = new TripDto
        {
            TripId = trip.TripId,
            Title = trip.Title,
            Content = trip.Content,
            RegistrationId = trip.RegistrationId,
            DepartureDate = trip.DepartureDate,
            ReturnDate = trip.ReturnDate,
            Price = trip.Price,
            NumOfTourist = trip.NumOfTourist
        };

        return Ok(tripDto);
    }


    [HttpPost("addTrip")]
    public async Task<IActionResult> AddTrip([FromForm] CreateTripDto trip)
    {
        if (trip.ImageURL == null || trip.ImageURL.Length == 0)
        {
            return BadRequest("Trip image is required.");
        }

        using var memoryStream = new MemoryStream();
        await trip.ImageURL.CopyToAsync(memoryStream);

        var tripEntity = new Trip
        {
            Title = trip.Title,
            Content = trip.Content,
            Price = trip.Price,
            DepartureDate = trip.DepartureDate,
            ReturnDate = trip.ReturnDate,
            ImageURL = memoryStream.ToArray(),
            RegistrationId = trip.RegistrationId,
            NumOfTourist = trip.NumOfTourist
        };

        _tripRepository.AddTrip(tripEntity);
        if (_tripRepository.SaveChanges())
        {
            return Ok(new { message = "Trip added successfully" });
        }

        throw new Exception("Failed to add trip");
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrip(int id, [FromForm] UpdateTripDto updateTripDto)
    {
        var existingTrip = await _tripRepository.GetTripByIdAsync(id);
        if (existingTrip == null)
        {
            return NotFound();
        }

        // If the image is being updated, handle it here
        if (updateTripDto.ImageURL != null && updateTripDto.ImageURL.Length > 0)
        {
            using var memoryStream = new MemoryStream();
            await updateTripDto.ImageURL.CopyToAsync(memoryStream);
            existingTrip.ImageURL = memoryStream.ToArray();
        }

        // Update other properties
        existingTrip.Title = updateTripDto.Title;
        existingTrip.Content = updateTripDto.Content;
        existingTrip.RegistrationId = updateTripDto.RegistrationId;
        existingTrip.Price = updateTripDto.Price;
        existingTrip.DepartureDate = updateTripDto.DepartureDate;
        existingTrip.ReturnDate = updateTripDto.ReturnDate;
        existingTrip.NumOfTourist = updateTripDto.NumOfTourist;
        // Call the repository method to update the trip
        _tripRepository.UpdateTrip(existingTrip);
        if (_tripRepository.SaveChanges())
        {
            return Ok(new { message = "Trip updated successfully" });
        }

        throw new Exception("Failed to update trip");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTrip(int id)
    {
        var trip = await _tripRepository.GetTripByIdAsync(id);
        if (trip == null)
        {
            return NotFound();
        }

        await _tripRepository.DeleteTripAsync(id);
        return NoContent();
    }

    [HttpGet("GetTripsbyRegistrationId/{RegistrationId}")]
    public List<TripBrief> GetTripsbyRegistrationId(int RegistrationId)
    {
        var trips = _tripRepository.GetAllTrpsByRegistrationId(RegistrationId);
        if (trips == null)
        {
            throw new Exception("Failed to get Trip");
        }
        return trips;
    }

    [HttpGet("GetImageTrip/{tripid}")]
    public IActionResult GetImageTrip(int tripid)
    {
        // SQL query to get full user information based on userId
        string userQuery = $@"
        SELECT [ImageURL]
        FROM FinalProjPost.Trips
        WHERE TripId = " + tripid;

        // Load the user information from the database
        var trip = _dapper.LoadDataSingle<byte[]>(userQuery);  // Pass just the SQL query as a string

        // If the user is not found, return a 404 Not Found
        if (trip == null)
        {
            throw new Exception("not found");
        }

        return File(trip, "image/jpeg");

    }
    [HttpGet("GetTripsSortedByTitle")]
    public IActionResult GetCarsSortedByTitle(string title)
    {
        var trips = _tripRepository.GetAllTrpsByTitle(title);
        return Ok(trips);
    }
    [HttpGet("GetTripsSortedByPrice")]
    public IActionResult GetCarsSortedByPrice(string sortOrder = "asc")
    {
        var trips = _tripRepository.GetTripsSortedByPrice(sortOrder);
        return Ok(trips);
    }
    [HttpGet("GetTripsSortedByTitleandPrice")]
    public IActionResult GetCarsSortedByTitleandPrice(string title,string sortOrder = "asc")
    {
        var trips = _tripRepository.GetTripsSortedByPriceAndTitle(title,sortOrder);
        return Ok(trips);
    }
}
