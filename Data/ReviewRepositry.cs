using Microsoft.EntityFrameworkCore;
using FinalProjAPI.Models;
using FinalProjAPI.Dto;

namespace FinalProjAPI.Data
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContextEF _entityFrameWork;

        // Accept DbContextOptions and IConfiguration in the constructor
        public ReviewRepository(DbContextOptions<DataContextEF> options, IConfiguration configuration)
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

        public List<ReviewBrief> GetAllReviewsAsync()
        {
            List<ReviewBrief> reviews = _entityFrameWork.Reviews.Select(x => new ReviewBrief()
            {
                ReviewId = x.ReviewId,
                Name = x.Name,
                Content = x.Content,
                RegistrationId = x.RegistrationId,
            }).ToList();
            return reviews;
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _entityFrameWork.Reviews.FindAsync(id);
        }

        public async Task AddReviewAsync(Review review)
        {
            await _entityFrameWork.Reviews.AddAsync(review);
            await _entityFrameWork.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _entityFrameWork.Reviews.Update(review);
            await _entityFrameWork.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _entityFrameWork.Reviews.FindAsync(id);
            if (review != null)
            {
                _entityFrameWork.Reviews.Remove(review);
                await _entityFrameWork.SaveChangesAsync();
            }
        }

        public Review GetReviewByName(string name)
        {
            Review? review = _entityFrameWork.Reviews.Where(u => u.Name == name).FirstOrDefault<Review>();
            if (review != null)
            {
                return review;
            }
            else
            {
                throw new Exception("Failed to get review");
            }
        }

        public int GetTypeID(string name)
        {
            Review? Review = _entityFrameWork.Reviews.Where(u => u.Name == name).FirstOrDefault<Review>();
            if (Review != null)
            {
                return Review.ReviewId;
            }
            else
            {
                throw new Exception("Failed to get review");
            }
        }

        public async Task DeleteReviewByNameAndRegitrationId(string name, int RegistrationId)
        {
            var review = await _entityFrameWork.Reviews.FirstOrDefaultAsync(c => c.Name == name && c.RegistrationId == RegistrationId);
            if (review == null)
            {
                throw new KeyNotFoundException($"Review with name '{name}' not found.");
            }

            _entityFrameWork.Reviews.Remove(review);
            await _entityFrameWork.SaveChangesAsync();
        }

        public bool AddReview<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFrameWork.Add(entityToAdd);
                return true;
            }
            return false;
        }

        Task<List<ReviewBrief>> IReviewRepository.GetAllReviewsAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteReviewByNameAndRegistrationId(string name, int registrationId)
        {
            throw new NotImplementedException();
        }
    }
}
