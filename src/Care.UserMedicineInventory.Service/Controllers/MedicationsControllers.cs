using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Care.Common;
using Care.UserMedicineInventory.Service.Dtos;
using Care.UserMedicineInventory.Service.Interfaces;
using Care.UserMedicineInventory.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Care.UserMedicineInventory.Service.Controller
{
    //API controller attribute is to improve the rest api developer experience.
    [ApiController]
    //Route "https:localhost:5001/medications" will be handled by this controller.
    [Route("users")]
    // each of the webapi controllers should be derived from ControllerBase.
    // the ControllerBase provides many properties and methods for handling HTTP requests.
    public class MedicinesController : ControllerBase
    {
        private readonly IUserMedicineInventoryService _userMedicineInventoryService;

        public MedicinesController(IUserMedicineInventoryService userMedicineInventoryService)
        {
            this._userMedicineInventoryService = userMedicineInventoryService;
        }

        // returns list of registered medications under a specific user
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserMedicineInventoryDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var userMedicineInventory = await _userMedicineInventoryService.GetAsync(userId);

            return Ok(userMedicineInventory);
        }


        // returns list of user registered under a specific medication
        [HttpGet("medicine/{medicineId}")]
        public async Task<ActionResult<IEnumerable<UserMedicineInventoryDto>>> GetMedicineByUsersAsync(Guid medicineId)
        {
            if (medicineId == Guid.Empty)
            {
                return BadRequest();
            }

            var userMedicineInventory = await _userMedicineInventoryService.GetMedicineByUsersAsync(medicineId);

            return Ok(userMedicineInventory);
        }

        //creates medication
        [HttpPost]
        public async Task<ActionResult> PostAsync(AssignMedicineDto assignMedicineDto)
        {

            var userMedicine = await _userMedicineInventoryService.PostAsync(assignMedicineDto);
            if (userMedicine == null) return BadRequest();

            return Ok();
        }

        //Put updating User medicine info
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateAssignMedicineDto updateAssignMedicineDto)
        {
            var userMedicine = await _userMedicineInventoryService.PutAsync(id, updateAssignMedicineDto);
            if (userMedicine == null) return NotFound();

            return NoContent();
        }

        //Delete medication
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userMedicine = await _userMedicineInventoryService.DeleteAsync(id);

            if (userMedicine == null) return NotFound();

            return NoContent();
        }
    }
}