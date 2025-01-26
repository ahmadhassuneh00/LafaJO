namespace FinalProjAPI.Controller;
using AutoMapper;
using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("[controller]")]
public class BookTripController : ControllerBase
{
    private readonly IBookTripRepositry _bookTripRepositry;
    DataContextDapper _dapper;

    public BookTripController(IConfiguration configuration, IBookTripRepositry bookTripRepositry)
    {
        _bookTripRepositry = bookTripRepositry;
        _dapper = new DataContextDapper(configuration);
    }

    [HttpGet("GetBookTrip")]
    public IEnumerable<BookTrip> GetBookTrips()
    {
        var bookTrips = _bookTripRepositry.GetBookingTrip();
        if (bookTrips == null)
        {
            throw new Exception("Failed to get Trip");
        }
        return bookTrips;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookTrip>> GetBookTrip(int id)
    {
        var trip = await _bookTripRepositry.GetBookTripByIdAsync(id);
        if (trip == null)
        {
            return NotFound();
        }

        var BookTripDto = new BookTripDto
        {
            TripId = trip.TripId,
            UserId = trip.UserId,
            numberOfPersons = trip.numberOfPersons,
            TotalCost = trip.TotalCost,
        };

        return Ok(BookTripDto);

    }


    [HttpPost("addBookTrip")]
    public IActionResult AddBookTrip([FromForm] BookTripDto trip)
    {
        try
        {
            // Get maximum number of tourists for the trip
            string sqlForMaxNumber = string.Format("SELECT [NumOfTourist] FROM FinalProjPost.Trips WHERE TripId = {0}", trip.TripId);
            var maxOfTravellers = _dapper.LoadDataSingle<int>(sqlForMaxNumber);

            // Get the current total number of persons already booked on the trip
            string sqlForNumberBookingTrip = string.Format("SELECT [numberOfPersons] FROM FinalProjPost.bookTrip WHERE TripId = {0}", trip.TripId);
            IEnumerable<BookTrip> dateBookTrip = _dapper.LoadData<BookTrip>(sqlForNumberBookingTrip);

            int totalNumber = dateBookTrip.Sum(b => b.numberOfPersons);

            var bookTrip = new BookTrip
            {
                TripId = trip.TripId,
                UserId = trip.UserId,
                numberOfPersons = trip.numberOfPersons,
                TotalCost = trip.TotalCost,
            };

            // Check if the user has already booked the trip
            if (_bookTripRepositry.CheckIfUserHasBooking(trip.UserId, trip.TripId))
            {
                return BadRequest(new { message = "The user has already booked this trip." });
            }

            // Check if adding the new booking would exceed the maximum allowed travelers
            if (totalNumber + bookTrip.numberOfPersons <= maxOfTravellers)
            {
                _bookTripRepositry.AddBookTrip(bookTrip);
                if (_bookTripRepositry.SaveChanges())
                {
                    return Ok(new { message = "Trip booked successfully" });
                }
                return StatusCode(500, new { message = "Failed to save changes" });
            }
            else
            {
                return BadRequest(new { message = "Booking exceeds the maximum number of travelers allowed for this trip." });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }



    [HttpDelete("DeleteBookTrip")]
    public async Task<ActionResult> DeleteTrip(int id)
    {
        var trip = await _bookTripRepositry.GetBookTripByIdAsync(id);
        if (trip == null)
        {
            return NotFound();  
        }

        _bookTripRepositry.RemoveEntitys(trip); ;
        if(_bookTripRepositry.SaveChanges())
        {
            return Ok("BookTrip deleted successfully");
        }else
        {
            return BadRequest("Failed to delete BookTrip");
        }
    }

    [HttpGet("TicketsAvailable")]
    public int GetTicketsAvailable(int id)
    {
        // Get maximum number of tourists for the trip
        string sqlForMaxNumber = "SELECT [NumOfTourist] FROM FinalProjPost.Trips WHERE TripId=" + id;
        var maxOfTravellers = _dapper.LoadDataSingle<int>(sqlForMaxNumber);

        // Get current total number of persons already booked on the trip
        string sqlForNumberBookingTrip = "SELECT [numberOfPersons] FROM FinalProjPost.bookTrip WHERE TripId=" + id;
        IEnumerable<BookTrip> dateBookTrip = _dapper.LoadData<BookTrip>(sqlForNumberBookingTrip);

        int totalNumber = 0;
        foreach (var booking in dateBookTrip)
        {
            totalNumber += booking.numberOfPersons;
        }
        return maxOfTravellers - totalNumber;
    }

    [HttpGet("GetTripIdByUserId/{UserId}")]
    public IEnumerable<int> GetTripIdByUserId(int UserId)
    {
        string sqlForTripId = "SELECT [TripId] FROM FinalProjPost.bookTrip WHERE UserId=" + UserId;
        IEnumerable<int> tripId = _dapper.LoadData<int>(sqlForTripId);
        return tripId;

    }

}