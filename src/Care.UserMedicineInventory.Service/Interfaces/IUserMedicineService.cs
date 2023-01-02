using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Care.UserMedicineInventory.Service.Dtos;
using Care.UserMedicineInventory.Service.Models;

namespace Care.UserMedicineInventory.Service.Interfaces
{
    public interface IUserMedicineInventoryService
    {
        public Task<IEnumerable<UserMedicineInventoryDto>> GetAsync(Guid userId);
        public Task<UserMedicineInventoryItem> PostAsync(AssignMedicineDto assignMedicineDto);
        public Task<UserMedicineInventoryItem> PutAsync(Guid id, UpdateAssignMedicineDto updateAssignMedicineDto);
        public Task<UserMedicineInventoryItem> DeleteAsync(Guid id);
    }
}