using System.Collections.Generic;
using System.Threading.Tasks;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;

namespace FinalProjAPI.Data
{
    public interface IReviewRepository
    {
        Task<List<ReviewBrief>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(int id);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int id);
        Review? GetReviewByName(string name);
        bool SaveChanges();
        int GetTypeID(string name);
        public bool RemoveEntity<T>(T entityToRemove);
        Task DeleteReviewByNameAndRegistrationId(string name, int registrationId);
        bool AddReview<T>(T entityToAdd);
    }
}
