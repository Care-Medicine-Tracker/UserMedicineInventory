using System;

namespace Care.UserMedicineInventory.Service.Dtos
{
    //DTO for returning medicines assign to a user's inventory

    public record UserMedicineInventoryDto(
        Guid Id,
        string Name,
        string Description,
        Guid MedicineId,
        string MeasurementType,
        double DoseAmount,
        int IntakeHour,
        int IntakeMinute
        );

}