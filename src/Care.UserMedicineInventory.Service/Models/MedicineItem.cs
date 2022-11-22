using System;
using Care.Common;
using Care.UserMedicineInventory.Service.Dtos;

namespace Care.UserMedicineInventory.Service.Models
{
    public class MedicineItem : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public MedicineDto AsDto()
        {
            return new MedicineDto(
                Id, Name, Description
                );
        }
    }
}