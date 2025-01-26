using System.Collections.Generic;
using System.Threading.Tasks;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;

namespace FinalProjAPI.Data
{
    public interface ICarRepository
    {
        public Task<bool> SaveChangesAsync();
        public List<CarBrief> GetAllCarsAsync();
        public List<CarBrief> GetAllCarsByRegistrationId(int registrationId);
        public List<CarBrief> GetAllCarsByMake(string Make);
        public List<CarBrief> GetAllCarsByFuelType(string FuelType);
        public List<CarBrief> GetAllCarsByTransmissionType(string TransmissionType);
        public List<CarBrief> GetCarsByFuelAndTransmission(string FuelType, string TransmissionType);
        public List<CarBrief> GetCarsByFuelAndMake(string FuelType, string Make);
        public List<CarBrief> GetCarsByTransmissionAndMake(string TransmissionType, string Make);
        public List<CarBrief> GetCarsByMakeSorted(string Make, string sortOrder);
        public List<CarBrief> GetCarsByFuelAndMakeSorted(string FuelType, string Make, string sortOrder);
        public List<CarBrief> GetCarsByTransmissionAndMakeSorted(string TransmissionType, string Make, string sortOrder);
        public List<CarBrief> GetCarsByFuelAndTransmissionSorted(string FuelType, string TransmissionType, string sortOrder);
        public List<CarBrief> GetCarsByFuelAndTransmissionAndMake(string FuelType, string TransmissionType, string Make);
        public List<CarBrief> GetCarsByFuelAndTransmissionAndMakeSorted(string FuelType, string TransmissionType, string Make, string sortOrder);
        public List<CarBrief> GetCarsByFuelSorted(string FuelType, string sortOrder);
        public List<CarBrief> GetCarsByTransmissionSorted(string TransmissionType, string sortOrder);
        public List<CarBrief> GetCarsSortedByDailyRate(string sortOrder = "asc");
        Task<Car?> GetCarByIdAsync(int id);
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(int id);
    }
}
