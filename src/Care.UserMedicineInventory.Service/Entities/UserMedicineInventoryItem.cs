using System;
using Care.Common;

namespace Care.UserMedicineInventory.Service.Entities
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

    }
}