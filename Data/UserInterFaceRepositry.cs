using FinalProjAPI.Models;

namespace FinalProjAPI.Data
{
    public interface interfaceRepastory
    {
        public bool SaveChanges();
        public bool RemoveEntitys<T>(T entityToRemove) where T : class;
        public IEnumerable<Users> GetUsers();
        public Task<Users?> GetSingleUserAsync(int userId);
        public Task<string?> GetUserNameByUserIdAsync(int userId);

    }

}