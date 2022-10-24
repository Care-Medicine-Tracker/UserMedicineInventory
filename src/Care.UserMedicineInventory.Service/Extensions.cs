using Care.UserMedicineInventory.Service.Dtos;
using Care.UserMedicineInventory.Service.Entities;

namespace Care.UserMedicineInventory.Service
{
    public static class Extensions
    {
        public static UserMedicineInventoryDto AsDto(this UserMedicineInventoryItem medicine)
        {
            return new UserMedicineInventoryDto(medicine.Id, medicine.MedicineId, medicine.MeasurementType, medicine.DoseAmount, medicine.IntakeHour, medicine.IntakeMinute);
        }
    }
}