using FinalProjAPI.Dto;
using FinalProjAPI.Models;

namespace FinalProjAPI.Data
{
    public interface IBookTripRepositry
    {
        public bool SaveChanges();
        public bool RemoveEntitys<T>(T entityToRemove) where T : class;
        public IEnumerable<BookTrip> GetBookingTrip();
        public Task<BookTrip?> GetBookTripByIdAsync(int id);
        public bool AddBookTrip<T>(T entityToAdd);
        public bool UpdateBookTrip<T>(T entityToUpdate);
        public bool CheckIfUserHasBooking(int userId, int tripId);
    }
}
