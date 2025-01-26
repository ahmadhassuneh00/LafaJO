using System.Collections.Generic;
using System.Threading.Tasks;
using FinalProjAPI.Models;
using FinalProjAPI.Dto;

namespace FinalProjAPI.Data
{
    public interface ITripRepository
    {
        public List<TripBrief> GetAllTripsAsync();
        public List<TripBrief> GetAllTrpsByRegistrationId(int registrationId);
        public List<TripBrief> GetTripsSortedByPrice(string sortOrder = "asc");
        public List<TripBrief> GetAllTrpsByTitle(string title);
        public List<TripBrief> GetTripsSortedByPriceAndTitle(string title, string sortOrder = "asc");
        Task<Trip?> GetTripByIdAsync(int id);
        Task UpdateTripAsync(Trip trip);
        Task DeleteTripAsync(int id);
        public Trip GetTripByTitle(string title);
        bool SaveChanges();
        public int GetTypeID(string title);
        public Task DeleteTripByNameAndRegitrationId(string title, int RegistrationId);
        public bool AddTrip<T>(T entityToAdd);
        public bool UpdateTrip<T>(T entityToUpdate);
    }
}
