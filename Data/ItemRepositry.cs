using Microsoft.EntityFrameworkCore;
using FinalProjAPI.Models;
using FinalProjAPI.Dto;

namespace FinalProjAPI.Data
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataContextEF _entityFrameWork;

         public ItemRepository(DbContextOptions<DataContextEF> options, IConfiguration configuration)
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

        public List<ItemBrief> GetAllItemsAsync()
        {
            List<ItemBrief> items = _entityFrameWork.Items.Select(x => new ItemBrief()
            {
                ItemId = x.ItemId,
                Name = x.Name,
                NumOfItems=x.NumOfItems,
                Price = x.Price,
                RegistrationId = x.RegistrationId

            }).ToList();
            return items;
        }
        public List<ItemBrief> GetAllItemsByRegistrationId(int registrationId)
        {
            List<ItemBrief> items = _entityFrameWork.Items
                .Where(x => x.RegistrationId == registrationId) // Filter by RegistrationId
                .Select(x => new ItemBrief()
                {
                    ItemId = x.ItemId,
                    Name = x.Name,
                    NumOfItems=x.NumOfItems,
                    Price = x.Price,
                    RegistrationId = x.RegistrationId
                }).ToList();

            return items;
        }

        public List<ItemBrief> GetAllItemsByTitle(string Name)
        {
            List<ItemBrief> items = _entityFrameWork.Items
                .Where(x => x.Name == Name) // Filter by RegistrationId
                .Select(x => new ItemBrief()
                {
                    ItemId = x.ItemId,
                    Name = x.Name,
                    NumOfItems=x.NumOfItems,
                    Price = x.Price,
                    RegistrationId = x.RegistrationId,
                }).ToList();

            return items;
        }

        public List<ItemBrief> GetItemsSortedByPrice(string sortOrder = "asc")
        {
            // Start with the Cars collection
            IQueryable<Item> itemsQuery = _entityFrameWork.Items;

            // Sort the cars based on the provided sort order
            if (sortOrder.ToLower() == "desc")
            {
                // Sort in descending order
                itemsQuery = itemsQuery.OrderByDescending(c => c.Price);
            }
            else
            {
                // Sort in ascending order (default)
                itemsQuery = itemsQuery.OrderBy(c => c.Price);
            }

            // Select the necessary properties and return the list of CarBrief
            List<ItemBrief> items = itemsQuery
                .Select(x => new ItemBrief()
                {
                    ItemId = x.ItemId,
                    Name = x.Name,
                    NumOfItems=x.NumOfItems,
                    Price = x.Price,
                    RegistrationId = x.RegistrationId,
                })
                .ToList();
            return items;
        }

        public List<ItemBrief> GetItemsSortedByPriceAndTitle(string title, string sortOrder = "asc")
        {
            // Validate the title
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));

            // Filter items by title
            IQueryable<Item> itemsQuery = _entityFrameWork.Items.Where(x => x.Name == title);

            // Apply sorting based on price
            itemsQuery = sortOrder?.ToLower() switch
            {
                "desc" => itemsQuery.OrderByDescending(c => c.Price),
                "asc" or null => itemsQuery.OrderBy(c => c.Price), // Default to ascending
                _ => throw new ArgumentException("Invalid sort order. Use 'asc' or 'desc'.", nameof(sortOrder))
            };

            // Project and return the sorted items
            return itemsQuery
                .Select(x => new ItemBrief
                {
                    ItemId = x.ItemId,
                    Name = x.Name,
                    NumOfItems=x.NumOfItems,
                    Price = x.Price,
                    RegistrationId = x.RegistrationId,
                })
                .ToList();
        }



        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _entityFrameWork.Items.FindAsync(id);
        }

        public async Task AddItemAsync(Item item)
        {
            await _entityFrameWork.Items.AddAsync(item);
            await _entityFrameWork.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            _entityFrameWork.Items.Update(item);
            await _entityFrameWork.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _entityFrameWork.Items.FindAsync(id);
            if (item != null)
            {
                _entityFrameWork.Items.Remove(item);
                await _entityFrameWork.SaveChangesAsync();
            }
        }
        public Item GetItemByName(string title)
        {
            Item? item = _entityFrameWork.Items.Where(u => u.Name == title).FirstOrDefault<Item>();
            if (item != null)
            {
                return item;
            }
            else
            {
                throw new Exception("Failed to get item");
            }
        }
        public async Task<List<Item>> GetItemsByUserIdAsync(int UserId)
        {
            List<Item> items = await _entityFrameWork.Items
                .Where(u => u.userId == UserId)
                .ToListAsync();

            if (items.Any())
            {
                return items;
            }
            else
            {
                throw new Exception("No items found for this user.");
            }
        }
        
        public int GetTypeID(string title)
        {
            Item? Item = _entityFrameWork.Items.Where(u => u.Name == title).FirstOrDefault<Item>();
            if (Item != null)
            {
                return Item.ItemId;
            }
            else
            {
                throw new Exception("Failed to get item");
            }
        }
        public async Task DeleteItemByNameAndRegitrationId(string title, int RegistrationId)
        {
            var item = await _entityFrameWork.Items.FirstOrDefaultAsync(t => t.Name == title && t.RegistrationId == RegistrationId);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with name '{title}' not found.");
            }

            _entityFrameWork.Items.Remove(item);
            await _entityFrameWork.SaveChangesAsync();
        }
        public bool AddItem<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFrameWork.Add(entityToAdd);
                return true;
            }
            return false;
        }
        public bool UpdateItem<T>(T entityToUpdate)
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
