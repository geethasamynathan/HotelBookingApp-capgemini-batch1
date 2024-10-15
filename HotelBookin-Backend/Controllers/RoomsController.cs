using AutoMapper;
using HotelBookin_Backend.DTO;
using HotelBookin_Backend.Custom_Exceptions;
using HotelBookin_Backend.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookin_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomManagement _roomManagement;
        private readonly IMapper _mapper;

        public RoomsController(IRoomManagement roomManagement,IMapper mapper)
        {
            _roomManagement = roomManagement;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var result = await _roomManagement.GetAllRooms();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var result = await _roomManagement.GetRoomById(id);
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddRoom([FromBody] RoomDTO roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var message = await _roomManagement.AddRoomAsync(roomDto);
                return CreatedAtAction(nameof(GetRoomById), new { id = roomDto.RoomId }, message);
            }
            catch (InvalidRoomDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id,[FromBody] RoomDTO updatedRoom)
        {
            try
            {
                var result = await _roomManagement.UpdateRoomAsync(updatedRoom);
                return Ok(result);
            }
            catch (RoomNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidRoomDataException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {               
                    var result = await _roomManagement.DeleteRoomAsync(id);
                    return Ok(result);                
            }
            catch (RoomNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }       
    }
}
