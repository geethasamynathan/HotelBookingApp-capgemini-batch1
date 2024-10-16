﻿using HotelBookin_Backend.DTO;
using HotelBookin_Backend.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookin_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentDTO processPaymentDto)
        {
            var result = await _paymentService.ProcessPayment(processPaymentDto);
             return CreatedAtAction(nameof(GetPaymentById), new { id = result.PaymentID }, result);
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var result = await _paymentService.GetPaymentDetails(id);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPaymentsByUserId(int userId)
        {
            var result = await _paymentService.GetUserPayments(userId);
            return Ok(result);
        }

    }
}
