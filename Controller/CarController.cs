using AutoMapper;
using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProjAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly DataContextDapper _dapper;
        private readonly DataContextEF _entityFramWork;

        public CarController(DbContextOptions<DataContextEF> options, ICarRepository carRepository, ICarRentRepository carRentRepository, IConfiguration configuration)
        {
            _carRepository = carRepository;
            _dapper = new DataContextDapper(configuration);
            _entityFramWork = new DataContextEF(options, configuration);
        }

        // Get all cars
        [HttpGet("GetCar")]
        public IActionResult GetCars()
        {
            var cars = _carRepository.GetAllCarsAsync();
            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No cars found" });
            }
            return Ok(cars);
        }

        // Get cars by registration ID
        [HttpGet("GetCarsbyRegistrationId/{RegistrationId}")]
        public IActionResult GetCarsbyRegistrationId(int RegistrationId)
        {
            var cars = _carRepository.GetAllCarsByRegistrationId(RegistrationId);
            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No cars found for the given registration ID" });
            }
            return Ok(cars);
        }
        [HttpGet("GetCarsbyMake/{Make}")]
        public IActionResult GetCarsbyMake(string Make)
        {
            var cars = _carRepository.GetAllCarsByMake(Make);
            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No cars found for the given registration ID" });
            }
            return Ok(cars);
        }
        [HttpGet("GetCarByFuelType/{FuelType}")]
        public IActionResult GetCarsByFuelType(string FuelType)
        {
            var cars = _carRepository.GetAllCarsByFuelType(FuelType);
            if (cars == null)
            {
                return NotFound(new { message = "No cars found" });
            }
            return Ok(cars);
        }
        [HttpGet("GetCarByTransmissionType/{TransmissionType}")]
        public IActionResult GetCarsByTransmissionType(string TransmissionType)
        {
            var cars = _carRepository.GetAllCarsByTransmissionType(TransmissionType);
            if (cars == null)
            {
                return NotFound(new { message = "No cars found" });
            }
            return Ok(cars);
        }
        [HttpGet("GetCarsByFuelAndTransmission")]
        public IActionResult GetCarsByFuelAndTransmission(string FuelType, string TransmissionType)
        {
            var cars = _carRepository.GetCarsByFuelAndTransmission(FuelType, TransmissionType);
            if (cars == null)
            {
                return NotFound(new { message = "No cars found" });
            }
            return Ok(cars);
        }

        [HttpGet("GetCarsByFuelAndTransmissionSorted")]
        public IActionResult GetCarsByFuelAndTransmissionSorted(string FuelType, string TransmissionType, string sortOrder)
        {
            // Check for invalid sortOrder and set default if necessary
            if (sortOrder != "asc" && sortOrder != "desc")
            {
                return BadRequest(new { message = "Invalid sort order. Use 'asc' or 'desc'." });
            }

            var cars = _carRepository.GetCarsByFuelAndTransmissionSorted(FuelType, TransmissionType, sortOrder);

            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No cars found" });
            }

            return Ok(cars);
        }
        [HttpGet("GetCarsByFuelAndTransmissionSortedwithSearch")]
        public IActionResult GetCarsByFuelAndTransmissionSortedwithSearch(string make, string FuelType, string TransmissionType, string sortOrder)
        {
            // Check for invalid sortOrder and set default if necessary
            if (sortOrder != "asc" && sortOrder != "desc")
            {
                return BadRequest(new { message = "Invalid sort order. Use 'asc' or 'desc'." });
            }

            var cars = _carRepository.GetCarsByFuelAndTransmissionSorted(FuelType, TransmissionType, sortOrder);

            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No cars found" });
            }

            return Ok(cars);
        }
        [HttpGet("GetCarsByMakeSorted")]
        public IActionResult GetCarsByMakeSorted(string Make,string sortOrder)
        {
            // Check for invalid sortOrder and set default if necessary
            if (sortOrder != "asc" && sortOrder != "desc")
            {
                return BadRequest(new { message = "Invalid sort order. Use 'asc' or 'desc'." });
            }

            var cars = _carRepository.GetCarsByMakeSorted(Make, sortOrder);

            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No cars found" });
            }

            return Ok(cars);
        }
        [HttpGet("GetCarsByFuelSorted")]
        public IActionResult GetCarsByFuelSorted(string FuelType, string sortOrder)
        {
            // Check for invalid sortOrder and set default if necessary
            if (sortOrder != "asc" && sortOrder != "desc")
            {
                return BadRequest(new { message = "Invalid sort order. Use 'asc' or 'desc'." });
            }

            var cars = _carRepository.GetCarsByFuelSorted(FuelType, sortOrder);

            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No cars found" });
            }

            return Ok(cars);
        }

        [HttpGet("GetCarsByFuelAndMake")]
        public IActionResult GetCarsByFuelAndMake(string FuelType, string Make)
        {
            var cars = _carRepository.GetCarsByFuelAndMake(FuelType, Make);
            if (cars == null)
            {
                return NotFound(new { message = "No cars found" });
            }
            return Ok(cars);
        }

        [HttpGet("GetCarsByTransmissionAndMake")]
        public IActionResult GetCarsByTransmissionAndMake(string TransmissionType, string Make)
        {
            var cars = _carRepository.GetCarsByTransmissionAndMake(TransmissionType, Make);
            if (cars == null)
            {
                return NotFound(new { message = "No cars found" });
            }
            return Ok(cars);
        }

        [HttpGet("GetCarsByFuelAndMakeSorted")]
        public IActionResult GetCarsByFuelAndMakeSorted(string FuelType, string Make, string sortOrder)
        {
            // Check for invalid sortOrder and set default if necessary
            if (sortOrder != "asc" && sortOrder != "desc")
            {
                return BadRequest(new { message = "Invalid sort order. Use 'asc' or 'desc'." });
            }

            var cars = _carRepository.GetCarsByFuelAndMakeSorted(FuelType, Make, sortOrder);

            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No cars found" });
            }

            return Ok(cars);
        }
        [HttpGet("GetCarsByTransmissionAndMakeSorted")]
        public IActionResult GetCarsByTransmissionAndMakeSorted(string TransmissionType, string Make, string sortOrder)
        {
            // Check for invalid sortOrder and set default if necessary
            if (sortOrder != "asc" && sortOrder != "desc")
            {
                return BadRequest(new { message = "Invalid sort order. Use 'asc' or 'desc'." });
            }

            var cars = _carRepository.GetCarsByTransmissionAndMakeSorted(TransmissionType, Make, sortOrder);

            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No cars found" });
            }

            return Ok(cars);
        }
        [HttpGet("GetCarsByTransmissionSortedwithSearch")]
        public IActionResult GetCarsByTransmissionSortedwithSearch(string make, string TransmissionType, string sortOrder)
        {
            // Check for invalid sortOrder and set default if necessary
            if (sortOrder != "asc" && sortOrder != "desc")
            {
                return BadRequest(new { message = "Invalid sort order. Use 'asc' or 'desc'." });
            }

            var cars = _carRepository.GetCarsByTransmissionSorted(TransmissionType, sortOrder);

            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No cars found" });
            }

            return Ok(cars);
        }
        [HttpGet("GetCarsByTransmissionSorted")]
        public IActionResult GetCarsByTransmissionSorted(string TransmissionType, string sortOrder)
        {
            // Check for invalid sortOrder and set default if necessary
            if (sortOrder != "asc" && sortOrder != "desc")
            {
                return BadRequest(new { message = "Invalid sort order. Use 'asc' or 'desc'." });
            }

            var cars = _carRepository.GetCarsByTransmissionSorted(TransmissionType, sortOrder);

            if (cars == null || !cars.Any())
            {
                return NotFound(new { message = "No cars found" });
            }

            return Ok(cars);
        }
        [HttpGet("GetCarsSortedByPrice")]
        public IActionResult GetCarsSortedByPrice(string sortOrder = "asc")
        {
            var cars = _carRepository.GetCarsSortedByDailyRate(sortOrder);
            return Ok(cars);
        }
        [HttpGet("GetCarsByFuelAndTransmissionAndMake")]
        public IActionResult GetCarsByFuelAndTransmissionAndMake(string FuelType, string TransmissionType, string Make)
        {
            var cars = _carRepository.GetCarsByFuelAndTransmissionAndMake(FuelType, TransmissionType, Make);
            return Ok(cars);
        }
        [HttpGet("GetCarsByFuelAndTransmissionAndMakeSorted")]
        public IActionResult GetCarsByFuelAndTransmissionAndMakeSorted(string FuelType, string TransmissionType, string Make, string sortOrder)
        {
            var cars = _carRepository.GetCarsByFuelAndTransmissionAndMakeSorted(FuelType, TransmissionType, Make, sortOrder);
            return Ok(cars);
        }

        // Get a car by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await _carRepository.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound(new { message = "Car not found" });
            }

            var carDto = new CarDto
            {
                CarId = car.CarID,
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                LicensePlate = car.LicensePlate,
                FuelType = car.FuelType,
                TransmissionType = car.TransmissionType,
                Color = car.Color,
                DailyRate = car.DailyRate,
                RegistrationId = car.RegistrationId
            };

            return Ok(carDto);
        }
        [HttpPost("addCar")]
        public async Task<IActionResult> addCar([FromForm] CreateCarDto car)
        {
            // Check if the car image is provided
            if (car.ImageURL == null || car.ImageURL.Length == 0)
            {
                return BadRequest("Car image is required.");
            }

            try
            {
                // Save the image to a memory stream
                using var memoryStream = new MemoryStream();
                await car.ImageURL.CopyToAsync(memoryStream);

                // Create a new Car entity
                var carEntity = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    LicensePlate = car.LicensePlate,
                    FuelType = car.FuelType,
                    TransmissionType = car.TransmissionType,
                    Color = car.Color,
                    DailyRate = car.DailyRate,
                    RegistrationId = car.RegistrationId,
                    ImageURL = memoryStream.ToArray(), // Store image as byte array
                };

                // Add the new car to the repository
                await _carRepository.AddCarAsync(carEntity); // Use async method for adding

                // Save changes asynchronously
                var saveResult = await _carRepository.SaveChangesAsync();

                if (saveResult)
                {
                    return Ok(new { message = "Car added successfully!" });
                }
                else
                {
                    return StatusCode(500, "A problem occurred while saving the car.");
                }
            }
            catch (Exception ex)
            {
                // Log the actual exception message for debugging purposes
                // Example: _logger.LogError(ex, "An error occurred while adding a car.");
                return StatusCode(500, $"Failed to add car: {ex.Message}");
            }
        }

        // Update a car by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, [FromForm] UpdateCarDto updateCarDto)
        {
            // Fetch the existing car by ID
            var existingCar = await _carRepository.GetCarByIdAsync(id);
            if (existingCar == null)
            {
                return NotFound(new { message = "Car not found" });
            }

            try
            {
                // If a new image is uploaded, update the image
                if (updateCarDto.ImageURL != null && updateCarDto.ImageURL.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await updateCarDto.ImageURL.CopyToAsync(memoryStream);
                    existingCar.ImageURL = memoryStream.ToArray(); // Update image binary data
                }

                // Update other car properties
                existingCar.Make = updateCarDto.Make;
                existingCar.Model = updateCarDto.Model;
                existingCar.Year = updateCarDto.Year;
                existingCar.LicensePlate = updateCarDto.LicensePlate;
                existingCar.FuelType = updateCarDto.FuelType;
                existingCar.TransmissionType = updateCarDto.TransmissionType;
                existingCar.Color = updateCarDto.Color;
                existingCar.DailyRate = updateCarDto.DailyRate;
                existingCar.RegistrationId = updateCarDto.RegistrationId;

                // Call repository method to update the car
                await _carRepository.UpdateCarAsync(existingCar);

                // Save the changes asynchronously
                await _carRepository.SaveChangesAsync();

                return Ok(new { message = "Car updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the error and return a status code
                // Optionally use a logging framework here
                return StatusCode(500, $"Failed to update car: {ex.Message}");
            }
        }

        // Delete a car by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _carRepository.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound(new { message = "Car not found" });
            }

            await _carRepository.DeleteCarAsync(id);
            await _carRepository.SaveChangesAsync();

            return NoContent();
        }


        // Get a car image
        [HttpGet("GetImageCar/{carid}")]
        public IActionResult GetImageCar(int carid)
        {
            var query = $@"
                SELECT [ImageURL]
                FROM FinalProjPost.Cars
                WHERE CarID = {carid}";

            var carImage = _dapper.LoadDataSingle<byte[]>(query);
            if (carImage == null)
            {
                return NotFound(new { message = "Car image not found" });
            }

            return File(carImage, "image/jpeg");
        }
    }
}
