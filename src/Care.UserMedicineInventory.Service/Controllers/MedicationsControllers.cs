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
    // each of the webapi controllers should be derived from ControllerBase.
    // the ControllerBase provides many properties and methods for handling HTTP requests.
    public class MedicinesController : ControllerBase
    {
        private readonly IRepository<UserMedicineInventoryItem> userMedicineInventoryItemsRepository;
        private readonly IRepository<MedicineItem> medicineItemsRepository;

        public MedicinesController(IRepository<UserMedicineInventoryItem> userMedicineInventoryItemsRepository, IRepository<MedicineItem> medicineItemsRepository)
        {
            this.userMedicineInventoryItemsRepository = userMedicineInventoryItemsRepository;
            this.medicineItemsRepository = medicineItemsRepository;
        }

        // returns list of registered medications under a specific user
        [HttpGet("/users/{userId}")]
        public async Task<ActionResult< IEnumerable<UserMedicineInventoryDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }
 
            var userMedicineInventoryItemEntities = await userMedicineInventoryItemsRepository.GetAllAsync(medicine => medicine.UserId == userId);
            //gives a list of medicines that belong to a user
            var medicineIds = userMedicineInventoryItemEntities.Select(medicine => medicine.MedicineId);
            var medicineItemEntities = await medicineItemsRepository.GetAllAsync(medicine => medicineIds.Contains(medicine.Id));

            var userMedicineInventoryItemDtos = userMedicineInventoryItemEntities.Select(userMedicineInventoryItem =>
            {
                var medicineItem = medicineItemEntities.Single(medicineItem => medicineItem.Id == userMedicineInventoryItem.MedicineId);
                return userMedicineInventoryItem.AsDto(medicineItem.Name, medicineItem.Description);
            });

            return Ok(userMedicineInventoryItemDtos);
        }


        // returns list of user registered under a specific medication
        [HttpGet("{medicineId}")]
        public async Task<ActionResult<IEnumerable<UserMedicineInventoryDto>>> GetAsyncMedicineByUsers(Guid medicineId)
        {
            if (medicineId == Guid.Empty)
            {
                return BadRequest();
            }

            var userWithMedicineInventoryItemEntities = await userMedicineInventoryItemsRepository.GetAllAsync(users => users.MedicineId == medicineId);

            return Ok(userWithMedicineInventoryItemEntities);
        }

        //creates medication
        [HttpPost]
        public async Task<ActionResult> PostAsync(AssignMedicineDto assignMedicineDto)
        {
            var userMedicineInventoryItem = await userMedicineInventoryItemsRepository.GetAsync(
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

                await userMedicineInventoryItemsRepository.CreateAsync(userMedicineInventoryItem);
            }
            return Ok();
        }

        //Put updating User medicine info
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateAssignMedicineDto updateAssignMedicineDto)
        {
            var existingMedicine = await userMedicineInventoryItemsRepository.GetAsync(id);

            if (existingMedicine == null)
            {
                return NotFound();
            }

            existingMedicine.MedicineId = updateAssignMedicineDto.MedicineId;
            existingMedicine.MeasurementType = updateAssignMedicineDto.MeasurementType;
            existingMedicine.DoseAmount = updateAssignMedicineDto.DoseAmount;
            existingMedicine.IntakeHour = updateAssignMedicineDto.IntakeHour;
            existingMedicine.IntakeMinute = updateAssignMedicineDto.IntakeMinute;

            await userMedicineInventoryItemsRepository.UpdateAsync(existingMedicine);

            return NoContent();
        }

        //Delete medication
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userMedicine = await userMedicineInventoryItemsRepository.GetAsync(id);

            if (userMedicine == null)
            {
                return NotFound();
            }

            await userMedicineInventoryItemsRepository.RemoveAsync(userMedicine.Id);

            return NoContent();
        }
    }
}