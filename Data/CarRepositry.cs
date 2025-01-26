using Microsoft.EntityFrameworkCore;
using FinalProjAPI.Models;
using FinalProjAPI.Dto;

namespace FinalProjAPI.Data
{
    public class CarRepository : ICarRepository
    {
        private readonly DataContextEF _entityFrameWork;

        public CarRepository(DbContextOptions<DataContextEF> options, IConfiguration configuration)
        {
            _entityFrameWork = new DataContextEF(options, configuration);
        }
        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                // Use async save changes
                return await _entityFrameWork.SaveChangesAsync() > 0; // Return true if save was successful
            }
            catch (DbUpdateException ex)
            {
                // Log the exception using a logging framework
                // Example: _logger.LogError(ex, "An error occurred while saving changes.");
                Console.Error.WriteLine(ex.Message); // For development, replace with proper logging
                return false; // Indicate failure
            }
        }
        public List<CarBrief> GetAllCarsAsync()
        {
            List<CarBrief> cars = _entityFrameWork.Cars.Select(x => new CarBrief()
            {
                CarId = x.CarID,
                Make = x.Make,
                Model = x.Model,
                Year = x.Year,
                LicensePlate = x.LicensePlate,
                FuelType = x.FuelType,
                TransmissionType = x.TransmissionType,
                Color = x.Color,
                DailyRate = x.DailyRate,
                RegistrationId = x.RegistrationId
            }).ToList();
            return cars;
        }
        public List<CarBrief> GetAllCarsByRegistrationId(int registrationId)
        {
            List<CarBrief> cars = _entityFrameWork.Cars
                .Where(x => x.RegistrationId == registrationId) // Filter by RegistrationId
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                }).ToList();

            return cars;
        }
        public List<CarBrief> GetAllCarsByMake(string Make)
        {
            List<CarBrief> cars = _entityFrameWork.Cars
                .Where(x => x.Make == Make || x.Model == Make) // Filter by RegistrationId
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                }).ToList();

            return cars;
        }
        public List<CarBrief> GetAllCarsByFuelType(string FuelType)
        {
            List<CarBrief> cars = _entityFrameWork.Cars
                .Where(x => x.FuelType == FuelType) // Filter by RegistrationId
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                }).ToList();

            return cars;
        }
        public List<CarBrief> GetAllCarsByTransmissionType(string TransmissionType)
        {
            List<CarBrief> cars = _entityFrameWork.Cars
                .Where(x => x.TransmissionType == TransmissionType) // Filter by RegistrationId
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                }).ToList();

            return cars;
        }

        public List<CarBrief> GetCarsByFuelAndTransmission(string FuelType, string TransmissionType)
        {
            List<CarBrief> cars = _entityFrameWork.Cars
                .Where(x => x.TransmissionType == TransmissionType && x.FuelType == FuelType) // Filter by RegistrationId
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                }).ToList();

            return cars;
        }
        public List<CarBrief> GetCarsByFuelAndMake(string FuelType, string Make)
        {
            List<CarBrief> cars = _entityFrameWork.Cars
                .Where(x => x.FuelType == FuelType && x.Make == Make) // Filter by RegistrationId
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                }).ToList();

            return cars;
        }

        public List<CarBrief> GetCarsByMakeSorted(string Make , string sortOrder){
            var carsQuery = _entityFrameWork.Cars
                .Where(x => x.Make == Make || x.Model == Make)
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                });

            // Sort based on the sortOrder parameter (asc or desc)
            if (sortOrder == "asc")
            {
                carsQuery = carsQuery.OrderBy(x => x.DailyRate);  // Sort in ascending order
            }
            else if (sortOrder == "desc")
            {
                carsQuery = carsQuery.OrderByDescending(x => x.DailyRate);  // Sort in descending order
            }

            // Convert to list and return the sorted result
            var cars = carsQuery.ToList();

            return cars;
        }
        
        public List<CarBrief> GetCarsByFuelAndMakeSorted(string FuelType, string Make, string sortOrder)
        {
            // Fetch cars by FuelType and Make
            var carsQuery = _entityFrameWork.Cars
                .Where(x => x.FuelType == FuelType && x.Make == Make)
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                });

            // Sort based on the sortOrder parameter (asc or desc)
            if (sortOrder == "asc")
            {
                carsQuery = carsQuery.OrderBy(x => x.DailyRate);  // Sort in ascending order
            }
            else if (sortOrder == "desc")
            {
                carsQuery = carsQuery.OrderByDescending(x => x.DailyRate);  // Sort in descending order
            }

            // Convert to list and return the sorted result
            var cars = carsQuery.ToList();

            return cars;
        }
        public List<CarBrief> GetCarsByTransmissionAndMakeSorted(string TransmissionType, string Make, string sortOrder)
        {
            // Fetch cars by FuelType and Make
            var carsQuery = _entityFrameWork.Cars
                .Where(x => x.TransmissionType == TransmissionType && x.Make == Make)
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                });

            // Sort based on the sortOrder parameter (asc or desc)
            if (sortOrder == "asc")
            {
                carsQuery = carsQuery.OrderBy(x => x.DailyRate);  // Sort in ascending order
            }
            else if (sortOrder == "desc")
            {
                carsQuery = carsQuery.OrderByDescending(x => x.DailyRate);  // Sort in descending order
            }

            // Convert to list and return the sorted result
            var cars = carsQuery.ToList();

            return cars;
        }

        public List<CarBrief> GetCarsByTransmissionAndMake(string TransmissionType, string Make)
        {
            List<CarBrief> cars = _entityFrameWork.Cars
                .Where(x => x.TransmissionType == TransmissionType && x.Make == Make) // Filter by RegistrationId
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                }).ToList();

            return cars;
        }

        public List<CarBrief> GetCarsByFuelAndTransmissionSorted(string FuelType, string TransmissionType, string sortOrder)
        {
            // Fetch cars by FuelType and TransmissionType
            var carsQuery = _entityFrameWork.Cars
                .Where(x => x.TransmissionType == TransmissionType && x.FuelType == FuelType)
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                });

            // Sort based on the sortOrder parameter (asc or desc)
            if (sortOrder == "asc")
            {
                carsQuery = carsQuery.OrderBy(x => x.DailyRate);  // Sort in ascending order
            }
            else if (sortOrder == "desc")
            {
                carsQuery = carsQuery.OrderByDescending(x => x.DailyRate);  // Sort in descending order
            }

            // Convert to list and return the sorted result
            var cars = carsQuery.ToList();

            return cars;
        }
        public List<CarBrief> GetCarsByFuelAndTransmissionAndMake(string FuelType, string TransmissionType, string Make)
        {
            List<CarBrief> cars = _entityFrameWork.Cars
                .Where(x => x.TransmissionType == TransmissionType && x.FuelType == FuelType && x.Make == Make) // Filter by RegistrationId
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                }).ToList();

            return cars;
        }
        public List<CarBrief> GetCarsByFuelAndTransmissionAndMakeSorted(string FuelType, string TransmissionType, string Make, string sortOrder)
        {
            // Fetch cars by FuelType and TransmissionType
            var carsQuery = _entityFrameWork.Cars
                .Where(x => x.TransmissionType == TransmissionType && x.FuelType == FuelType && x.Make == Make)
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                });

            // Sort based on the sortOrder parameter (asc or desc)
            if (sortOrder == "asc")
            {
                carsQuery = carsQuery.OrderBy(x => x.DailyRate);  // Sort in ascending order
            }
            else if (sortOrder == "desc")
            {
                carsQuery = carsQuery.OrderByDescending(x => x.DailyRate);  // Sort in descending order
            }

            // Convert to list and return the sorted result
            var cars = carsQuery.ToList();

            return cars;
        }
        public List<CarBrief> GetCarsByFuelSorted(string FuelType, string sortOrder)
        {
            // Fetch cars by FuelType and TransmissionType
            var carsQuery = _entityFrameWork.Cars
                .Where(x => x.FuelType == FuelType)
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                });

            // Sort based on the sortOrder parameter (asc or desc)
            if (sortOrder == "asc")
            {
                carsQuery = carsQuery.OrderBy(x => x.DailyRate);  // Sort in ascending order
            }
            else if (sortOrder == "desc")
            {
                carsQuery = carsQuery.OrderByDescending(x => x.DailyRate);  // Sort in descending order
            }

            // Convert to list and return the sorted result
            var cars = carsQuery.ToList();

            return cars;
        }
        public List<CarBrief> GetCarsByTransmissionSorted(string TransmissionType, string sortOrder)
        {
            // Fetch cars by FuelType and TransmissionType
            var carsQuery = _entityFrameWork.Cars
                .Where(x => x.TransmissionType == TransmissionType)
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                });

            // Sort based on the sortOrder parameter (asc or desc)
            if (sortOrder == "asc")
            {
                carsQuery = carsQuery.OrderBy(x => x.DailyRate);  // Sort in ascending order
            }
            else if (sortOrder == "desc")
            {
                carsQuery = carsQuery.OrderByDescending(x => x.DailyRate);  // Sort in descending order
            }

            // Convert to list and return the sorted result
            var cars = carsQuery.ToList();

            return cars;
        }

        public List<CarBrief> GetCarsSortedByDailyRate(string sortOrder = "asc")
        {
            // Start with the Cars collection
            IQueryable<Car> carsQuery = _entityFrameWork.Cars;

            // Sort the cars based on the provided sort order
            if (sortOrder.ToLower() == "desc")
            {
                // Sort in descending order
                carsQuery = carsQuery.OrderByDescending(c => c.DailyRate);
            }
            else
            {
                // Sort in ascending order (default)
                carsQuery = carsQuery.OrderBy(c => c.DailyRate);
            }

            // Select the necessary properties and return the list of CarBrief
            List<CarBrief> cars = carsQuery
                .Select(x => new CarBrief()
                {
                    CarId = x.CarID,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    LicensePlate = x.LicensePlate,
                    FuelType = x.FuelType,
                    TransmissionType = x.TransmissionType,
                    Color = x.Color,
                    DailyRate = x.DailyRate,
                    RegistrationId = x.RegistrationId
                })
                .ToList();

            return cars;
        }

        public async Task<Car?> GetCarByIdAsync(int id)
        {
            return await _entityFrameWork.Cars.FindAsync(id);
        }

        public async Task AddCarAsync(Car car)
        {
            await _entityFrameWork.Cars.AddAsync(car);

        }

        public async Task UpdateCarAsync(Car car)
        {
            _entityFrameWork.Cars.Update(car);
            await _entityFrameWork.SaveChangesAsync();
        }

        public async Task DeleteCarAsync(int id)
        {
            var car = await _entityFrameWork.Cars.FindAsync(id);
            if (car != null)
            {
                _entityFrameWork.Cars.Remove(car);
            }
        }
    }
}
