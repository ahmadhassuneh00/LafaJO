using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FinalProjAPI.Data
{
    public class CarRentRepository : ICarRentRepository
    {
        private readonly DataContextEF dataContextEF;

        public CarRentRepository(DbContextOptions<DataContextEF> options, IConfiguration configuration)
        {
            dataContextEF = new DataContextEF(options, configuration);
        }
        public bool SaveChanges()
        {
            return dataContextEF.SaveChanges() > 0;
        }
        public bool RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                dataContextEF.Remove(entityToRemove);
                return true;
            }
            return false;
        }


        public List<RentCarBrief> GetRentCars()
        {
            return dataContextEF.RentCars.Select(x => new RentCarBrief()
            {
                CarID = x.CarID,
                UserId = x.UserId,
                RentalStartDate = x.RentalStartDate,
                RentalEndDate = x.RentalEndDate,
                TotalCost = x.TotalCost,
            }).ToList();
        }

        public async Task AddCarAsync(RentCar rentCar)
        {
            if (rentCar != null)
            {
                await dataContextEF.RentCars.AddAsync(rentCar);
                dataContextEF.SaveChanges();
            }
        }

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        public RentCar? GetRentCarBriefByUserId(int UserId)
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            // Fetch the rented car for the given user
            RentCar? rentCar = dataContextEF.RentCars
                .Where(u => u.UserId == UserId)
                .FirstOrDefault();

            if (rentCar != null)
            {
                // Fetch car details using the CarID from the rented car
                var car = dataContextEF.Cars.Find(rentCar.CarID); // Assuming you have a Cars DbSet in your context
                if (car == null)
                {
                    throw new Exception("Car details not found for the rented car.");
                }

                int rentalDays = CalculateNumberOfDays(rentCar.RentalStartDate, rentCar.RentalEndDate);
                rentCar.TotalCost = CalculateTotalCost(car.DailyRate, rentalDays); // Access DailyRate from the Car entity
            }
            else
            {
                throw new Exception("No rented car found for the specified user.");
            }

            return rentCar;
        }


#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        public RentCar? GetRentCarByCarId(int CarId)
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            return dataContextEF.RentCars.FirstOrDefault(u => u.CarID == CarId);
        }

        public int CalculateNumberOfDays(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException("End date must be greater than or equal to the start date.");
            }

            return (endDate - startDate).Days;
        }

        public decimal CalculateTotalCost(decimal dailyRate, int numberOfDays)
        {
            if (dailyRate < 0)
            {
                throw new ArgumentException("Daily rate cannot be negative.");
            }

            return dailyRate * numberOfDays;
        }

        public decimal GetTotalCost(int CarID, int UserId)
        {
            RentCar? rentCar = dataContextEF.RentCars.FirstOrDefault(x => x.CarID == CarID && x.UserId == UserId);
            if (rentCar == null)
            {
                throw new Exception($"No rental found for CarID {CarID}");
            }

            decimal? TotalCost = rentCar.TotalCost;

            if (TotalCost.HasValue)
            {
                return TotalCost.Value;
            }

            throw new Exception("TotalCost is null for this rental.");
        }

        public bool IsCarAvailable(int carId, DateTime startDate, DateTime endDate)
        {
            return !dataContextEF.RentCars
                .Any(rentCar => rentCar.CarID == carId &&
                                (rentCar.RentalStartDate <= endDate && rentCar.RentalEndDate >= startDate));
        }

        public IEnumerable<int> GetCarByUserId(int userId)
        {
            // Fetch the cars rented by the user
            var carIds = dataContextEF.RentCars
                .Where(r => r.UserId == userId) // Filter by UserId
                .Select(r => r.CarID) // Select only the CarID property
                .ToList(); // Materialize the query into a list

            if (carIds.Any()) // Check if there are any car IDs
            {
                return carIds; // Return the car IDs as an IEnumerable<int>
            }
            else
            {
                throw new Exception("Failed to get CarId");
            }   
        }

    }
}
