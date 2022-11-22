using System;

namespace Care.UserMedicineInventory.Service.Dtos
{
    public record AssignMedicineDto(
        Guid UserId,
        Guid MedicineId,
        string MeasurementType,
        double DoseAmount,
        int IntakeHour,
        int IntakeMinute
        );
}