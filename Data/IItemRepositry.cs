using System.Collections.Generic;
using System.Threading.Tasks;
using FinalProjAPI.Models;
using FinalProjAPI.Dto;

namespace FinalProjAPI.Data
{
    public interface IItemRepository
    {
        public List<ItemBrief> GetAllItemsAsync();
        public List<ItemBrief> GetAllItemsByRegistrationId(int registrationId);
        public List<ItemBrief> GetAllItemsByTitle(string title);

        public List<ItemBrief> GetItemsSortedByPrice(string sortOrder = "asc");
        public List<ItemBrief> GetItemsSortedByPriceAndTitle(string title, string sortOrder = "asc");


        Task<Item?> GetItemByIdAsync(int id);
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(int id);

        public Item GetItemByName(string title);
        bool SaveChanges();
        public int GetTypeID(string title);
        public Task DeleteItemByNameAndRegitrationId(string title, int RegistrationId);
        public bool AddItem<T>(T entityToAdd);
        public bool UpdateItem<T>(T entityToUpdate);
        public Task<List<Item>> GetItemsByUserIdAsync(int UserId);
    }
}
