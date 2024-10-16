using HotelBookin_Backend.DTO;
using HotelBookin_Backend.Custom_Exceptions;
using HotelBookin_Backend.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookin_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }
        
        [HttpPost]
        public async Task<ActionResult<HotelDTO>> AddHotel([FromBody] HotelDTO hotelDto)
        {
            var createdHotel = await _hotelService.AddHotelAsync(hotelDto);
            if (createdHotel != null)
            {
                return Ok("Added");
            }
            else return BadRequest();
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelDTO hotelDto)
        {
            try
            {
                var updatedHotel = await _hotelService.UpdateHotelAsync(id, hotelDto);
                return Ok("Updated");
            }
            catch (HotelNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            try
            {
                await _hotelService.DeleteHotelAsync(id);
                return Ok("Deleted");
            }
            catch (HotelNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<HotelDTO>>> GetAllHotels()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDTO>> GetHotelById(int id)
        {
            try
            {
                var hotel = await _hotelService.GetHotelByIdAsync(id);
                return Ok(hotel);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/rooms")]
        public async Task<ActionResult<List<RoomDTO>>> GetRoomsByHotelId(int id)
        {
            var rooms = await _hotelService.GetRoomsByHotelIdAsync(id);
            if (rooms == null || !rooms.Any())
            {
                return NotFound("No rooms found for this hotel.");
            }
            return Ok(rooms);
        }
    }

}
