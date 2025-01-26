using FinalProjAPI.Dto;
using FinalProjAPI.Models;

namespace FinalProjAPI.Data
{
    public interface IBuyItemRepositry
    {
        public bool SaveChanges();
        public bool RemoveEntitys<T>(T entityToRemove) where T : class;
        public IEnumerable<BuyItem> GetBuyingItem();
        public Task<BuyItem?> GetBuyItemByIdAsync(int id);
        public bool AddBuyItem<T>(T entityToAdd);
        public bool UpdateBuyItem<T>(T entityToUpdate);
        public bool CheckIfUserHasBought(int userId, int itemId);
    }
}
