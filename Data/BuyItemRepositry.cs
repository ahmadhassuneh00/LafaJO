using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace FinalProjAPI.Data
{
    public class BuyItemRepositry : IBuyItemRepositry
    {
        private readonly DataContextEF _entityFramework;

        private readonly DataContextDapper _dapper;

        public BuyItemRepositry(DataContextEF entityFramework, IConfiguration config)
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

        public IEnumerable<BuyItem> GetBuyingItem()
        {
            return _entityFramework.BuyItems.ToList();
        }
        public async Task<BuyItem?> GetBuyItemByIdAsync(int id)
        {
            return await _entityFramework.BuyItems.FirstOrDefaultAsync(u => u.ItemId == id);
        }

        public bool AddBuyItem<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
                return true;
            }
            return false;
        }
        public bool UpdateBuyItem<T>(T entityToUpdate)
        {
            if (entityToUpdate != null)
            {
                _entityFramework.Update(entityToUpdate);
                return true;
            }
            return false;
        }

        public bool CheckIfUserHasBought(int userId, int itemId)
        {
            string sqlForCheckBuying = string.Format("SELECT COUNT(1) FROM FinalProjPost.buyItem WHERE UserId = {0} AND ItemId = {1}", userId, itemId);
            int buyingCount = _dapper.LoadDataSingle<int>(sqlForCheckBuying);
            return buyingCount > 0;
        }



    }

}