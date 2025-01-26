using FinalProjAPI.Dto;
using FinalProjAPI.Models;

namespace FinalProjAPI.Data
{
    public interface ICarRentRepository
    {

        public bool SaveChanges();
        public bool RemoveEntity<T>(T entityToRemove);
        public List<RentCarBrief> GetRentCars();
        // public Task DeleteCarAsync(int id);
        public decimal GetTotalCost(int CarID, int UserId);
        public Task AddCarAsync(RentCar rentCar);
        public RentCar GetRentCarBriefByUserId(int UserId);
        public RentCar GetRentCarByCarId(int CarId);
        public int CalculateNumberOfDays(DateTime startDate, DateTime endDate);
        public decimal CalculateTotalCost(decimal dailyRate, int numberOfDays);
        public bool IsCarAvailable(int carId, DateTime startDate, DateTime endDate);
        public IEnumerable<int> GetCarByUserId(int userId);
    }
}