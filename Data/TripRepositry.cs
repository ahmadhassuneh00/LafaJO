using Microsoft.EntityFrameworkCore;
using FinalProjAPI.Models;
using FinalProjAPI.Dto;

namespace FinalProjAPI.Data
{
    public class TripRepository : ITripRepository
    {
        private readonly DataContextEF _entityFrameWork;

        public TripRepository(DbContextOptions<DataContextEF> options, IConfiguration configuration)
        {
            _entityFrameWork = new DataContextEF(options, configuration);
        }
        public bool SaveChanges()
        {
            return _entityFrameWork.SaveChanges() > 0;
        }
        public bool RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFrameWork.Remove(entityToRemove);
                return true;
            }
            return false;
        }

        public List<TripBrief> GetAllTripsAsync()
        {
            List<TripBrief> trips = _entityFrameWork.Trips.Select(x => new TripBrief()
            {
                TripId = x.TripId,
                Title = x.Title,
                Content = x.Content,
                Price = x.Price,
                RegistrationId = x.RegistrationId,
                DepartureDate = x.DepartureDate,
                ReturnDate = x.ReturnDate,
                NumOfTourist = x.NumOfTourist
            }).ToList();
            return trips;
        }
        public List<TripBrief> GetAllTrpsByRegistrationId(int registrationId)
        {
            List<TripBrief> trips = _entityFrameWork.Trips
                .Where(x => x.RegistrationId == registrationId) // Filter by RegistrationId
                .Select(x => new TripBrief()
                {
                    TripId = x.TripId,
                    Title = x.Title,
                    Content = x.Content,
                    Price = x.Price,
                    RegistrationId = x.RegistrationId,
                    DepartureDate = x.DepartureDate,
                    ReturnDate = x.ReturnDate,
                    NumOfTourist = x.NumOfTourist
                }).ToList();

            return trips;
        }

        public List<TripBrief> GetAllTrpsByTitle(string title)
        {
            List<TripBrief> trips = _entityFrameWork.Trips
                .Where(x => x.Title == title) // Filter by RegistrationId
                .Select(x => new TripBrief()
                {
                    TripId = x.TripId,
                    Title = x.Title,
                    Content = x.Content,
                    Price = x.Price,
                    RegistrationId = x.RegistrationId,
                    DepartureDate = x.DepartureDate,
                    ReturnDate = x.ReturnDate,
                    NumOfTourist = x.NumOfTourist
                }).ToList();

            return trips;
        }

        public List<TripBrief> GetTripsSortedByPrice(string sortOrder = "asc")
        {
            // Start with the Cars collection
            IQueryable<Trip> tripsQuery = _entityFrameWork.Trips;

            // Sort the cars based on the provided sort order
            if (sortOrder.ToLower() == "desc")
            {
                // Sort in descending order
                tripsQuery = tripsQuery.OrderByDescending(c => c.Price);
            }
            else
            {
                // Sort in ascending order (default)
                tripsQuery = tripsQuery.OrderBy(c => c.Price);
            }

            // Select the necessary properties and return the list of CarBrief
            List<TripBrief> trips = tripsQuery
                .Select(x => new TripBrief()
                {
                    TripId = x.TripId,
                    Title = x.Title,
                    Content = x.Content,
                    Price = x.Price,
                    RegistrationId = x.RegistrationId,
                    DepartureDate = x.DepartureDate,
                    ReturnDate = x.ReturnDate,
                    NumOfTourist = x.NumOfTourist
                })
                .ToList();
            return trips;
        }

        public List<TripBrief> GetTripsSortedByPriceAndTitle(string title, string sortOrder = "asc")
        {
            // Validate the title
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));

            // Filter trips by title
            IQueryable<Trip> tripsQuery = _entityFrameWork.Trips.Where(x => x.Title == title);

            // Apply sorting based on price
            tripsQuery = sortOrder?.ToLower() switch
            {
                "desc" => tripsQuery.OrderByDescending(c => c.Price),
                "asc" or null => tripsQuery.OrderBy(c => c.Price), // Default to ascending
                _ => throw new ArgumentException("Invalid sort order. Use 'asc' or 'desc'.", nameof(sortOrder))
            };

            // Project and return the sorted trips
            return tripsQuery
                .Select(x => new TripBrief
                {
                    TripId = x.TripId,
                    Title = x.Title,
                    Content = x.Content,
                    Price = x.Price,
                    RegistrationId = x.RegistrationId,
                    DepartureDate = x.DepartureDate,
                    ReturnDate = x.ReturnDate,
                    NumOfTourist = x.NumOfTourist
                })
                .ToList();
        }


        public async Task<Trip?> GetTripByIdAsync(int id)
        {
            return await _entityFrameWork.Trips.FindAsync(id);
        }


        public async Task UpdateTripAsync(Trip trip)
        {
            _entityFrameWork.Trips.Update(trip);
            await _entityFrameWork.SaveChangesAsync();
        }

        public async Task DeleteTripAsync(int id)
        {
            var trip = await _entityFrameWork.Trips.FindAsync(id);
            if (trip != null)
            {
                _entityFrameWork.Trips.Remove(trip);
                await _entityFrameWork.SaveChangesAsync();
            }
        }
        public Trip GetTripByTitle(string title)
        {
            Trip? trip = _entityFrameWork.Trips.Where(u => u.Title == title).FirstOrDefault<Trip>();
            if (trip != null)
            {
                return trip;
            }
            else
            {
                throw new Exception("Failed to get trip");
            }
        }

        public int GetTypeID(string title)
        {
            Trip? Trip = _entityFrameWork.Trips.Where(u => u.Title == title).FirstOrDefault<Trip>();
            if (Trip != null)
            {
                return Trip.TripId;
            }
            else
            {
                throw new Exception("Failed to get trip");
            }
        }
        public async Task DeleteTripByNameAndRegitrationId(string title, int RegistrationId)
        {
            var trip = await _entityFrameWork.Trips.FirstOrDefaultAsync(t => t.Title == title && t.RegistrationId == RegistrationId);
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with title '{title}' not found.");
            }

            _entityFrameWork.Trips.Remove(trip);
            await _entityFrameWork.SaveChangesAsync();
        }
        public bool AddTrip<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFrameWork.Add(entityToAdd);
                return true;
            }
            return false;
        }
        public bool UpdateTrip<T>(T entityToUpdate)
        {
            if (entityToUpdate != null)
            {
                _entityFrameWork.Update(entityToUpdate);
                return true;
            }
            return false;
        }

    }
}
