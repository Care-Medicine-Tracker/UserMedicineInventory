using System;
using Care.Common;
using Care.UserMedicineInventory.Service.Dtos;

namespace Care.UserMedicineInventory.Service.Models
{
    public class UserMedicineInventoryItem : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid MedicineId { get; set; }

        public string MeasurementType { get; set; }

        public double DoseAmount { get; set; }

        public int IntakeHour { get; set; }

        public int IntakeMinute { get; set; }


        // public UserMedicineInventoryDto AsDto(this UserMedicineInventoryItem medicine, string name, string description)
        // {
        //     return new UserMedicineInventoryDto(medicine.Id, name, description, medicine.MedicineId, medicine.MeasurementType, medicine.DoseAmount, medicine.IntakeHour, medicine.IntakeMinute);
        // }

    }
}