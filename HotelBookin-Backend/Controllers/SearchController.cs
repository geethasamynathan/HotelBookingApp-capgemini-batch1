using HotelBookin_Backend.IServices;
using HotelBookin_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookin_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        public ISearchServices _searchServices;

        public SearchController(ISearchServices searchServices)
        {
            _searchServices = searchServices;
        }
        // GET: api/<SearchController>
        [HttpGet]
        public async Task<IActionResult> SearchHotels(string location)
        {
            try
            {
                var hotels = await _searchServices.SearchHotelsAsync(location);
                if (hotels == null || !hotels.Any())
                {
                    return NotFound("no hotel found");
                }
                return Ok(hotels);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("available")]
        public async Task<ActionResult<List<Room>>> GetAvailableRooms(int HotelId,[FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut)
        {
            if (checkOut <= checkIn)
                return BadRequest("Check-out date must be after check-in date.");
            var availableRooms = await _searchServices.GetAvailableRoomAsync(HotelId,checkIn, checkOut);
            if (availableRooms == null || availableRooms.Count == 0)
                return NotFound("No available rooms found for the given dates.");
            return Ok(availableRooms);
        }
    }
}
