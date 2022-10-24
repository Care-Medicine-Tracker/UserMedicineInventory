using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Care.Common;
using Care.UserMedicineInventory.Service.Dtos;
using Care.UserMedicineInventory.Service.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Care.UserMedicineInventory.Service.Controller
{
    //API controller attribute is to improve the rest api developer experience.
    [ApiController]
    //Route "https:localhost:5001/medications" will be handled by this controller.
    [Route("medicines")]
    // each of the webapi controllers should be derived from ControllerBase
    // the ControllerBase provides many properties and methods for handling HTTP requests
    public class MedicinesController : ControllerBase
    {
        private readonly IRepository<UserMedicineInventoryItem> medicineRepository;

        public MedicinesController(IRepository<UserMedicineInventoryItem> medicineRepository)
        {
            this.medicineRepository = medicineRepository;
        }

        // returns list of registered medications under a specific user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMedicineInventoryDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var medicines = (await medicineRepository.GetAllAsync(medicine => medicine.UserId == userId))
                            .Select(medicine => medicine.AsDto());
            return Ok(medicines);
        }

        //creates medication
        [HttpPost]
        public async Task<ActionResult> PostAsync(AssignMedicineDto assignMedicineDto)
        {
            var userMedicineInventoryItem = await medicineRepository.GetAsync(
                medicine => medicine.UserId == assignMedicineDto.UserId && medicine.MedicineId == assignMedicineDto.MedicineId);

            if (userMedicineInventoryItem == null)
            {
                userMedicineInventoryItem = new UserMedicineInventoryItem
                {
                    UserId = assignMedicineDto.UserId,
                    MedicineId = assignMedicineDto.MedicineId,
                    MeasurementType = assignMedicineDto.MeasurementType,
                    DoseAmount = assignMedicineDto.DoseAmount,
                    IntakeHour = assignMedicineDto.IntakeHour,
                    IntakeMinute = assignMedicineDto.IntakeMinute,
                };

                await medicineRepository.CreateAsync(userMedicineInventoryItem);
            }
            return Ok();
        }

        //Put updating User medicine info
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateAssignMedicineDto updateAssignMedicineDto)
        {
            var existingMedicine = await medicineRepository.GetAsync(id);

            if (existingMedicine == null)
            {
                return NotFound();
            }

            existingMedicine.MedicineId = updateAssignMedicineDto.MedicineId;
            existingMedicine.MeasurementType = updateAssignMedicineDto.MeasurementType;
            existingMedicine.DoseAmount = updateAssignMedicineDto.DoseAmount;
            existingMedicine.IntakeHour = updateAssignMedicineDto.IntakeHour;
            existingMedicine.IntakeMinute = updateAssignMedicineDto.IntakeMinute;

            await medicineRepository.UpdateAsync(existingMedicine);

            return NoContent();
        }

        //Delete medication
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userMedicine = await medicineRepository.GetAsync(id);

            if (userMedicine == null)
            {
                return NotFound();
            }

            await medicineRepository.RemoveAsync(userMedicine.Id);

            return NoContent();
        }
    }
}