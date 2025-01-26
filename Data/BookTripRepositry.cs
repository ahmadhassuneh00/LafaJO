using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace FinalProjAPI.Data
{
    public class BookTripRepositry : IBookTripRepositry
    {
        private readonly DataContextEF _entityFramework;

        private readonly DataContextDapper _dapper;

        public BookTripRepositry(DataContextEF entityFramework, IConfiguration config)
        {
            _entityFramework = entityFramework;
            _dapper = new DataContextDapper(config);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public bool RemoveEntitys<T>(T entityToRemove) where T : class
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
                return true;
            }
            return false;
        }

        public IEnumerable<BookTrip> GetBookingTrip()
        {
            return _entityFramework.BookTrips.ToList();
        }
        public async Task<BookTrip?> GetBookTripByIdAsync(int id)
        {
            return await _entityFramework.BookTrips.FirstOrDefaultAsync(u => u.TripId == id);
        }

        public bool AddBookTrip<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
                return true;
            }
            return false;
        }
        public bool UpdateBookTrip<T>(T entityToUpdate)
        {
            if (entityToUpdate != null)
            {
                _entityFramework.Update(entityToUpdate);
                return true;
            }
            return false;
        }

        public bool CheckIfUserHasBooking(int userId, int tripId)
        {
            string sqlForCheckBooking = string.Format("SELECT COUNT(1) FROM FinalProjPost.bookTrip WHERE UserId = {0} AND TripId = {1}", userId, tripId);
            int bookingCount = _dapper.LoadDataSingle<int>(sqlForCheckBooking);
            return bookingCount > 0;
        }



    }

}