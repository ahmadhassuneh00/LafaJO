namespace FinalProjAPI.Controller;
using AutoMapper;
using FinalProjAPI.Data;
using FinalProjAPI.Dto;
using FinalProjAPI.Models;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemRepository _itemRepository;

    DataContextDapper _dapper;

    public ItemController(IItemRepository itemRepository, IConfiguration configuration)
    {
        _itemRepository = itemRepository;
        _dapper = new DataContextDapper(configuration);
    }

    [HttpGet("GetItem")]
    public List<ItemBrief> GetItems()
    {
        var items = _itemRepository.GetAllItemsAsync();
        if (items == null)
        {
            throw new Exception("Failed to get Item");
        }
        return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItem(int id)
    {
        var item = await _itemRepository.GetItemByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        var itemDto = new ItemDto
        {
            ItemId = item.ItemId,
            Name = item.Name,
            NumOfItems= item.NumOfItems,
            RegistrationId = item.RegistrationId,
            Price = item.Price
        };

        return Ok(itemDto);
    }


    [HttpPost("addItem")]
    public async Task<IActionResult> AddItem([FromForm] CreateItemDto item)
    {
        if (item.ImageURL == null || item.ImageURL.Length == 0)
        {
            return BadRequest("Item image is required.");
        }

        using var memoryStream = new MemoryStream();
        await item.ImageURL.CopyToAsync(memoryStream);

        var itemEntity = new Item
        {
            Name = item.Name,
            Price = item.Price,
            NumOfItems= item.NumOfItems,
            ImageURL = memoryStream.ToArray(),
            RegistrationId = item.RegistrationId,
        };

        _itemRepository.AddItem(itemEntity);
        if (_itemRepository.SaveChanges())
        {
            return Ok(new { message = "Item added successfully" });
        }

        throw new Exception("Failed to add item");
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, [FromForm] UpdateItemDto updateItemDto)
    {
        var existingItem = await _itemRepository.GetItemByIdAsync(id);
        if (existingItem == null)
        {
            return NotFound();
        }

        // If the image is being updated, handle it here
        if (updateItemDto.ImageURL != null && updateItemDto.ImageURL.Length > 0)
        {
            using var memoryStream = new MemoryStream();
            await updateItemDto.ImageURL.CopyToAsync(memoryStream);
            existingItem.ImageURL = memoryStream.ToArray();
        }

        // Update other properties
        existingItem.Name = updateItemDto.Name;
        existingItem.RegistrationId = updateItemDto.RegistrationId;
        existingItem.NumOfItems= updateItemDto.NumOfItems;
        existingItem.Price = updateItemDto.Price;
        // Call the repository method to update the item
        _itemRepository.UpdateItem(existingItem);
        if (_itemRepository.SaveChanges())
        {
            return Ok(new { message = "Item updated successfully" });
        }

        throw new Exception("Failed to update item");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItem(int id)
    {
        var item = await _itemRepository.GetItemByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        await _itemRepository.DeleteItemAsync(id);
        return NoContent();
    }

    [HttpGet("GetItemsbyRegistrationId/{RegistrationId}")]
    public List<ItemBrief> GetItemsbyRegistrationId(int RegistrationId)
    {
        var items = _itemRepository.GetAllItemsByRegistrationId(RegistrationId);
        if (items == null)
        {
            throw new Exception("Failed to get Item");
        }
        return items;
    }

    [HttpGet("GetImageItem/{itemid}")]
    public IActionResult GetImageItem(int itemid)
    {
        // SQL query to get full user information based on userId
        string userQuery = $@"
        SELECT [ImageURL]
        FROM FinalProjPost.Items
        WHERE ItemId = " + itemid;

        // Load the user information from the database
        var item = _dapper.LoadDataSingle<byte[]>(userQuery);  // Pass just the SQL query as a string

        // If the user is not found, return a 404 Not Found
        if (item == null)
        {
            throw new Exception("not found");
        }

        return File(item, "image/jpeg");

    }
    [HttpGet("GetItemsSortedByTitle")]
    public IActionResult GetItemsSortedByTitle(string title)
    {
        var items = _itemRepository.GetAllItemsByTitle(title);
        return Ok(items);
    }
    [HttpGet("GetItemsSortedByPrice")]
    public IActionResult GetItemsSortedByPrice(string sortOrder = "asc")
    {
        var items = _itemRepository.GetItemsSortedByPrice(sortOrder);
        return Ok(items);
    }
    [HttpGet("GetItemsSortedByTitleandPrice")]
    public IActionResult GetItemsSortedByTitleandPrice(string title,string sortOrder = "asc")
    {
        var items = _itemRepository.GetItemsSortedByPriceAndTitle(title,sortOrder);
        return Ok(items);
    }
}
