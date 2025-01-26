namespace FinalProjAPI.Controller;
using AutoMapper;
using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("[controller]")]
public class BuyItemController : ControllerBase
{
    private readonly IBuyItemRepositry _buyItemRepositry;
    DataContextDapper _dapper;

    public BuyItemController(IConfiguration configuration, IBuyItemRepositry buyItemRepositry)
    {
        _buyItemRepositry = buyItemRepositry;
        _dapper = new DataContextDapper(configuration);
    }

    [HttpGet("GetBuyItem")]
    public IEnumerable<BuyItem> GetBuyItems()
    {
        var buyItems = _buyItemRepositry.GetBuyingItem();
        if (buyItems == null)
        {
            throw new Exception("Failed to get Item");
        }
        return buyItems;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BuyItem>> GetBuyItem(int id)
    {
        var item = await _buyItemRepositry.GetBuyItemByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        var BuyItemDto = new BuyItemDto
        {
            ItemId = item.ItemId,
            UserId = item.UserId,
            numberOfItems = item.numberOfItems,
            TotalCost = item.TotalCost,
        };

        return Ok(BuyItemDto);

    }


    [HttpPost("addBuyItem")]
    public IActionResult AddBuyItem([FromForm] BuyItemDto item)
    {
        try
        {
            // Get maximum number of items
            string sqlForMaxNumber = string.Format("SELECT [NumOfItems] FROM FinalProjPost.Items WHERE ItemId = {0}", item.ItemId);
            var maxOfItems = _dapper.LoadDataSingle<int>(sqlForMaxNumber);

            // Get the current total number of persons already bought the item
            string sqlForNumberBuyingItem = string.Format("SELECT [numberOfItems] FROM FinalProjPost.buyItem WHERE ItemId = {0}", item.ItemId);
            IEnumerable<BuyItem> dateBuyItem = _dapper.LoadData<BuyItem>(sqlForNumberBuyingItem);

            int totalNumber = dateBuyItem.Sum(b => b.numberOfItems);

            var buyItem = new BuyItem
            {
                ItemId = item.ItemId,
                UserId = item.UserId,
                numberOfItems = item.numberOfItems,
                TotalCost = item.TotalCost,
            };

            // Check if the user has already bought the item
            if (_buyItemRepositry.CheckIfUserHasBought(item.UserId, item.ItemId))
            {
                return BadRequest(new { message = "The user has already bought this item." });
            }

            // Check if adding the new item would exceed the maximum allowed items
            if (totalNumber + buyItem.numberOfItems <= maxOfItems)
            {
                _buyItemRepositry.AddBuyItem(buyItem);
                if (_buyItemRepositry.SaveChanges())
                {
                    return Ok(new { message = "Item bought successfully" });
                }
                return StatusCode(500, new { message = "Failed to save changes" });
            }
            else
            {
                return BadRequest(new { message = "Buying exceeds the maximum number of items allowed." });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }



    [HttpDelete("DeleteBuyItem")]
    public async Task<ActionResult> DeleteItem(int id)
    {
        var item = await _buyItemRepositry.GetBuyItemByIdAsync(id);
        if (item == null)
        {
            return NotFound();  
        }

        _buyItemRepositry.RemoveEntitys(item); ;
        if(_buyItemRepositry.SaveChanges())
        {
            return Ok("BuyItem deleted successfully");
        }else
        {
            return BadRequest("Failed to delete BuyItem");
        }
    }

    [HttpGet("ItemsAvailable")]
    public int GetItemsAvailable(int id)
    {
        // Get maximum number of Items
        string sqlForMaxNumber = "SELECT [NumOfItems] FROM FinalProjPost.Items WHERE ItemId=" + id;
        var maxOfItems = _dapper.LoadDataSingle<int>(sqlForMaxNumber);

        // Get current total number of persons already bought the item
        string sqlForNumberBuyingItem = "SELECT [numberOfItems] FROM FinalProjPost.buyItem WHERE ItemId=" + id;
        IEnumerable<BuyItem> dateBuyItem = _dapper.LoadData<BuyItem>(sqlForNumberBuyingItem);

        int totalNumber = 0;
        foreach (var booking in dateBuyItem)
        {
            totalNumber += booking.numberOfItems;
        }
        return maxOfItems- totalNumber;
    }


    [HttpGet("GetItemIdByUserId/{UserId}")]
    public IEnumerable<int> GetItemIdByUserId(int UserId)
    {
        string sqlForItemId = "SELECT [ItemId] FROM FinalProjPost.buyItem WHERE UserId=" + UserId;
        IEnumerable<int> itemId = _dapper.LoadData<int>(sqlForItemId);
        return itemId;

    }

}