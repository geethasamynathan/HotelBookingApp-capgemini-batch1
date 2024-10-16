using HotelBookin_Backend.DTO;
using HotelBookin_Backend.IServices;
using HotelBookin_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookin_Backend.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationDTO createReservationDto)
        {
            var result = await _reservationService.AddNewReservation(createReservationDto);
            if (result != null)
            {
                return Ok("Added");
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] UpdateReservationDto updateReservationDto)
        {
            try
            {
                var result = await _reservationService.UpdateReservation(id, updateReservationDto);
                return Ok("Updated");
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            await _reservationService.CancelReservation(id);
            return Ok("Cancelled");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            var result = await _reservationService.GetAllReservation();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            try
            {
                var result = await _reservationService.GetReservationById(id);
                return Ok(result);
            }catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserReservations(int userId)
        {
            try
            {
                var result = await _reservationService.GetReservationByUserId(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetHotelReservations(int hotelId)
        {
            try
            {
                var result = await _reservationService.GetReservationByHotelId(hotelId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

}
