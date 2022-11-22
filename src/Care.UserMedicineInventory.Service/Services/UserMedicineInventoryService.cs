using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Care.Common;
using Care.UserMedicineInventory.Service.Dtos;
using Care.UserMedicineInventory.Service.Interfaces;
using Care.UserMedicineInventory.Service.Models;

namespace Care.UserMedicineInventory.Service.Services
{
    public class UserMedicineInventoryService : IUserMedicineInventoryService
    {
        private readonly IRepository<UserMedicineInventoryItem> userMedicineInventoryItemsRepository;
        private readonly IRepository<MedicineItem> medicineItemsRepository;

        public UserMedicineInventoryService(IRepository<UserMedicineInventoryItem> userMedicineInventoryItemsRepository, IRepository<MedicineItem> medicineItemsRepository)
        {
            this.userMedicineInventoryItemsRepository = userMedicineInventoryItemsRepository;
            this.medicineItemsRepository = medicineItemsRepository;
        }

        public async Task<IEnumerable<UserMedicineInventoryDto>> GetAsync(Guid userId)
        {
            var userMedicineInventoryItemEntities = await userMedicineInventoryItemsRepository.GetAllAsync(medicine => medicine.UserId == userId);
            //gives a list of medicines that belong to a user
            var medicineIds = userMedicineInventoryItemEntities.Select(medicine => medicine.MedicineId);
            var medicineItemEntities = await medicineItemsRepository.GetAllAsync(medicine => medicineIds.Contains(medicine.Id));

            var userMedicineInventoryItemDtos = userMedicineInventoryItemEntities.Select(userMedicineInventoryItem =>
            {
                var medicineItem = medicineItemEntities.Single(medicineItem => medicineItem.Id == userMedicineInventoryItem.MedicineId);
                return userMedicineInventoryItem.AsDto(medicineItem.Name, medicineItem.Description);
            });

            return userMedicineInventoryItemDtos;
        }

        public async Task<IEnumerable<UserMedicineInventoryDto>> GetMedicineByUsersAsync(Guid medicineId)
        {
            var userWithMedicineInventoryItemEntities = await userMedicineInventoryItemsRepository.GetAllAsync(users => users.MedicineId == medicineId);

            return ((IEnumerable<UserMedicineInventoryDto>)userWithMedicineInventoryItemEntities);
        }

        public async Task<UserMedicineInventoryItem> PostAsync(AssignMedicineDto assignMedicineDto)
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
            return userMedicineInventoryItem;
        }

        public async Task<UserMedicineInventoryItem> PutAsync(Guid id, UpdateAssignMedicineDto updateAssignMedicineDto)
        {
            var existingMedicine = await userMedicineInventoryItemsRepository.GetAsync(id);

            if (existingMedicine == null) return null!;

            existingMedicine.MedicineId = updateAssignMedicineDto.MedicineId;
            existingMedicine.MeasurementType = updateAssignMedicineDto.MeasurementType;
            existingMedicine.DoseAmount = updateAssignMedicineDto.DoseAmount;
            existingMedicine.IntakeHour = updateAssignMedicineDto.IntakeHour;
            existingMedicine.IntakeMinute = updateAssignMedicineDto.IntakeMinute;

            await userMedicineInventoryItemsRepository.UpdateAsync(existingMedicine);

            return existingMedicine;
        }

        public async Task<UserMedicineInventoryItem> DeleteAsync(Guid id)
        {
            var userMedicine = await userMedicineInventoryItemsRepository.GetAsync(id);

            if (userMedicine == null) return null!;

            await userMedicineInventoryItemsRepository.RemoveAsync(userMedicine.Id);

            return userMedicine;
        }
    }
}