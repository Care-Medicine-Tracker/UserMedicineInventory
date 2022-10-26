using System;
using Care.Common;

namespace Care.UserMedicineInventory.Service.Entities
{
    public class MedicineItem : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}