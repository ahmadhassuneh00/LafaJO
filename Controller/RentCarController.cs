using AutoMapper;
using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentCarController : ControllerBase
    {
        private readonly ICarRentRepository _carRentRepository;
        private readonly ICarRepository _carRepository;
        private readonly DataContextDapper _dapper;
        public RentCarController(ICarRepository carRepository, ICarRentRepository carRentRepository, IConfiguration configuration)
        {
            _dapper = new DataContextDapper(configuration);
            _carRepository = carRepository;
            _carRentRepository = carRentRepository;
        }

        // Get all rented cars
        [HttpGet("GetRentCar")]
        public IActionResult GetRentCars()
        {
            var cars = _carRentRepository.GetRentCars();
            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No rent cars found" });
            }
            return Ok(cars);
        }

        // Get rented car ID by user ID
        [HttpGet("GetCarId/{UserId}")]
        public IActionResult GetCarIdbyUserId(int UserId)
        {
            var rentCarId = _carRentRepository.GetCarByUserId(UserId);

            if (rentCarId == null)
            {
                return NotFound(new { message = "No rented car found for the given user ID" });
            }

            return Ok(rentCarId); // Return the car ID if found
        }

        // Get rented cars by user ID
        [HttpGet("GetRentCarsbyUserId/{UserId}")]
        public IActionResult GetRentCarsbyUserId(int UserId)
        {
            var rentCars = _carRentRepository.GetRentCarBriefByUserId(UserId);
            if (rentCars == null)
            {
                return NotFound(new { message = "No rent cars found for the given user ID" });
            }
            return Ok(rentCars);
        }

        // Get rented cars by Car ID
        [HttpGet("GetRentCarsbyCarId/{CarId}")]
        public IActionResult GetRentCarsbyCarId(int CarId)
        {
            var rentCars = _carRentRepository.GetRentCarByCarId(CarId);
            if (rentCars == null)
            {
                return NotFound(new { message = "No rent cars found for the given Car ID" });
            }
            return Ok(rentCars);
        }

        // Get total cost
        [HttpGet("GetTotalCost/{CarId}")]
        public IActionResult GetTotalCost(int CarId, int UserId)
        {
            var totalCost = _carRentRepository.GetTotalCost(CarId, UserId);
            return Ok(totalCost);
        }

        // Add rented car
        [HttpPost("addRentCar")]
        public async Task<IActionResult> AddRentCar([FromForm] CreateRentCar car)
        {
            try
            {
                // Fetch car details
                var carDetails = await _carRepository.GetCarByIdAsync(car.CarID);
                if (carDetails == null)
                {
                    return NotFound(new { message = "Car not found" });
                }

                // Calculate total cost
                int numberOfDays = _carRentRepository.CalculateNumberOfDays(car.RentalStartDate, car.RentalEndDate);
                decimal totalCost = _carRentRepository.CalculateTotalCost(carDetails.DailyRate, numberOfDays);

                // Create RentCar entity
                var rentCarEntity = new RentCar
                {
                    CarID = car.CarID,
                    UserId = car.UserId,
                    RentalStartDate = car.RentalStartDate,
                    RentalEndDate = car.RentalEndDate,
                    TotalCost = totalCost,
                };

                // Add to the repository
                await _carRentRepository.AddCarAsync(rentCarEntity);

                return Ok(new { message = "Car rental added successfully", rentCar = rentCarEntity });
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework for production)
                return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
            }
        }

        // Check car availability
        [HttpGet("IsCarAvailable/{carId}")]
        public IActionResult IsCarAvailable(int carId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                bool available = _carRentRepository.IsCarAvailable(carId, startDate, endDate);
                return Ok(new { available });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred", detail = ex.Message });
            }
        }

        [HttpDelete("deleteRentCar/{CarId}")]
        public IActionResult DeleteRentCar(int CarId)
        {
            try
            {
                RentCar rentCar1 = _carRentRepository.GetRentCarByCarId(CarId);
                // Attempt to remove the entity
                _carRentRepository.RemoveEntity<RentCar>(rentCar1);

                // Save changes
                if (_carRentRepository.SaveChanges())
                {
                    return Ok("RentCar deleted successfully.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete RentCar.");
                }
            }
            catch (Exception ex)
            {
                // Log exception (consider using a logging framework here)
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetImageRentCar/{RentalID}")]
        public IActionResult GetImageCar(int RentalID)
        {
            var CarID = $@"SELECT CarID From FinalProjPost.Rentals WHERE RentalID = {RentalID}";
            _dapper.LoadData<int>(CarID);
            if (_dapper.ExecuteSql(CarID))
            {
                var query = $@"
                SELECT [ImageURL]
                FROM FinalProjPost.Cars
                WHERE CarID = {CarID}";

                var carImage = _dapper.LoadDataSingle<byte[]>(query);
                if (carImage == null)
                {
                    return NotFound(new { message = "Car image not found" });
                }

                return File(carImage, "image/jpeg");
            }
            else
            {
                throw new Exception("Failed to get image");
            }
        }
    }
}
